using Microsoft.EntityFrameworkCore;
using Server.Database.Converters;
using Server.Database.Import;
using Server.Database.PersistenceModels;
using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using Server.Library.MirDatabase;

namespace Server.Database.Persistence;

public sealed class EfCorePersistence : IMirPersistence
{
    private readonly string _connectionString;
    private readonly Action<string> _log;

    public EfCorePersistence(string connectionString, Action<string> log = null)
    {
        _connectionString = connectionString;
        _log = log ?? (_ => { });
    }

    public void Initialize()
    {
        using var db = CreateDb();
        db.Database.Migrate();

        if (!db.DbMeta.Any())
        {
            _log("EF database is empty; importing from binary files.");
            var importer = new BinaryFileImporter(_connectionString, _log);
            importer.ImportIfEmpty();
        }
    }

    public void LoadServerData(Envir envir)
    {
        using var db = CreateDb();

        var meta = db.DbMeta.AsNoTracking().Single();

        Envir.LoadVersion = meta.Version;
        Envir.LoadCustomVersion = meta.CustomVersion;

        envir.MapIndex = meta.MapIndex;
        envir.ItemIndex = meta.ItemIndex;
        envir.MonsterIndex = meta.MonsterIndex;
        envir.NPCIndex = meta.NpcIndex;
        envir.QuestIndex = meta.QuestIndex;
        envir.GameshopIndex = meta.GameshopIndex;
        envir.ConquestIndex = meta.ConquestIndex;
        envir.RespawnIndex = meta.RespawnIndex;

        // Clear and load static lists from payload tables.
        envir.MapInfoList.Clear();
        envir.ItemInfoList.Clear();
        envir.MonsterInfoList.Clear();
        envir.NPCInfoList.Clear();
        envir.QuestInfoList.Clear();
        envir.MagicInfoList.Clear();
        envir.GameShopList.Clear();
        envir.ConquestInfoList.Clear();
        envir.GTMapList.Clear();

        // MapInfos
        foreach (var row in db.MapInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.MapInfoList.Add(new MapInfo(reader));
        }

        // ItemInfos
        foreach (var row in db.ItemInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            var item = new ItemInfo(reader, meta.Version, meta.CustomVersion);
            if (item != null && item.RandomStatsId < Settings.RandomItemStatsList.Count)
                item.RandomStats = Settings.RandomItemStatsList[item.RandomStatsId];
            envir.ItemInfoList.Add(item);
        }

        // BindGameShop uses Edit.ItemInfoList; keep it in sync early.
        Envir.Edit.ItemInfoList = envir.ItemInfoList;

        // MonsterInfos
        foreach (var row in db.MonsterInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.MonsterInfoList.Add(new MonsterInfo(reader));
        }

        // NPCInfos
        foreach (var row in db.NpcInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.NPCInfoList.Add(new NPCInfo(reader));
        }

        // QuestInfos
        foreach (var row in db.QuestInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.QuestInfoList.Add(new QuestInfo(reader));
        }

        // DragonInfo
        var dragon = db.DragonInfoState.AsNoTracking().SingleOrDefault();
        if (dragon != null)
        {
            using var reader = CreateReader(dragon.Data);
            envir.DragonInfo = new DragonInfo(reader);
        }
        else
        {
            envir.DragonInfo = new DragonInfo();
        }

        // MagicInfos + add defaults
        foreach (var row in db.MagicInfos.AsNoTracking().OrderBy(x => x.Spell))
        {
            using var reader = CreateReader(row.Data);
            var mi = new MagicInfo(reader, meta.Version, meta.CustomVersion);
            if (mi != null)
                envir.MagicInfoList.Add(mi);
        }
        envir.FillMagicInfoList();
        if (meta.Version <= 70)
            envir.UpdateMagicInfo();

        // GameShopItems (bind against Edit env item list; keep parity with legacy LoadDB)
        foreach (var row in db.GameShopItems.AsNoTracking().OrderBy(x => x.GIndex))
        {
            using var reader = CreateReader(row.Data);
            var item = new GameShopItem(reader, meta.Version, meta.CustomVersion);
            if (envir.BindGameShop(item))
                envir.GameShopList.Add(item);
        }

        // ConquestInfos
        foreach (var row in db.ConquestInfos.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.ConquestInfoList.Add(new ConquestInfo(reader));
        }

        // Respawn tick
        var respawn = db.RespawnTimerState.AsNoTracking().SingleOrDefault();
        if (respawn != null)
        {
            using var reader = CreateReader(respawn.Data);
            envir.RespawnTick = new RespawnTimer(reader);
        }
        else
        {
            envir.RespawnTick = new RespawnTimer();
        }

        // GT maps
        foreach (var row in db.GtMaps.AsNoTracking().OrderBy(x => x.Index))
        {
            using var reader = CreateReader(row.Data);
            envir.GTMapList.Add(new GTMap(reader));
        }

        Settings.LinkGuildCreationItems(envir.ItemInfoList);

        // Keep Edit environment in sync for lookups/binding.
        MirrorStaticToEditEnvir(envir);
    }

    public void LoadUserData(Envir envir)
    {
        using var db = CreateDb();

        // reset ranking (parity with LoadAccounts)
        for (var i = 0; i < envir.RankClass.Count(); i++)
        {
            if (envir.RankClass[i] != null)
                envir.RankClass[i].Clear();
            else
                envir.RankClass[i] = new List<RankCharacterInfo>();
        }
        envir.RankTop.Clear();

        var meta = db.DbMeta.AsNoTracking().Single();
        envir.NextAccountID = meta.NextAccountId;
        envir.NextCharacterID = meta.NextCharacterId;
        envir.NextUserItemID = meta.NextUserItemId;
        envir.NextHeroID = meta.NextHeroId;
        envir.GuildCount = meta.GuildCount;
        envir.NextGuildID = meta.NextGuildId;
        envir.NextAuctionID = meta.NextAuctionId;
        envir.NextMailID = meta.NextMailId;

        // Build UserItem objects (graph + slots)
        var userItemRows = db.UserItems.AsNoTracking().ToList();
        var itemById = new Dictionary<ulong, UserItem>(userItemRows.Count);

        foreach (var row in userItemRows)
        {
            var info = envir.ItemInfoList.FirstOrDefault(x => x.Index == row.ItemIndex);
            if (info == null) continue;

            var ui = new UserItem(info)
            {
                UniqueID = row.UserItemId,
                ItemIndex = row.ItemIndex,
                Info = info,
                CurrentDura = row.CurrentDura,
                MaxDura = row.MaxDura,
                Count = row.Count,
                GemCount = row.GemCount,
                SoulBoundId = row.SoulBoundId,
                Identified = row.Identified,
                Cursed = row.Cursed,
                WeddingRing = row.WeddingRing,
                RefinedValue = (RefinedValue)row.RefinedValue,
                RefineAdded = row.RefineAdded,
                RefineSuccessChance = row.RefineSuccessChance,
                IsShopItem = row.IsShopItem,
                GMMade = row.GmMade,
            };

            ui.SetSlotSize(row.SlotsLength);

            // AddedStats
            using (var r = CreateReader(row.AddedStatsData))
                ui.AddedStats = new Stats(r, meta.Version, meta.CustomVersion);

            // Awake
            using (var r = CreateReader(row.AwakeData))
                ui.Awake = new Awake(r);

            // Optional sub-objects: empty array = absent
            if (row.ExpireInfoData.Length != 0)
            {
                using var r = CreateReader(row.ExpireInfoData);
                ui.ExpireInfo = new ExpireInfo(r, meta.Version, meta.CustomVersion);
            }

            if (row.RentalInformationData.Length != 0)
            {
                using var r = CreateReader(row.RentalInformationData);
                ui.RentalInformation = new RentalInformation(r, meta.Version, meta.CustomVersion);
            }

            if (row.SealedInfoData.Length != 0)
            {
                using var r = CreateReader(row.SealedInfoData);
                ui.SealedInfo = new SealedInfo(r, meta.Version, meta.CustomVersion);
            }

            itemById[row.UserItemId] = ui;
        }

        foreach (var slot in db.UserItemSlots.AsNoTracking())
        {
            if (!itemById.TryGetValue(slot.ParentUserItemId, out var parent)) continue;
            if (!itemById.TryGetValue(slot.ChildUserItemId, out var child)) continue;

            if (slot.SlotIndex < 0 || slot.SlotIndex >= parent.Slots.Length) continue;
            parent.Slots[slot.SlotIndex] = child;
        }

        // Heroes
        envir.HeroList.Clear();
        foreach (var h in db.Heroes.AsNoTracking().OrderBy(x => x.Index))
        {
            var hero = new HeroInfo
            {
                Index = h.Index,
                Name = h.Name,
                Level = (ushort)h.Level,
                Class = (MirClass)h.Class,
                Gender = (MirGender)h.Gender,
                Hair = h.Hair,
                CreationDate = h.CreationDate,
                Deleted = h.Deleted,
                DeleteDate = h.DeleteDate,
                HP = h.Hp,
                MP = h.Mp,
                Experience = h.Experience,
                AutoPot = h.AutoPot,
                Grade = h.Grade,
                HPItemIndex = h.HpItemIndex,
                MPItemIndex = h.MpItemIndex,
                AutoHPPercent = h.AutoHpPercent,
                AutoMPPercent = h.AutoMpPercent,
                SealCount = (ushort)h.SealCount,
            };

            hero.Inventory = new UserItem[h.InventorySize];
            hero.Equipment = new UserItem[h.EquipmentSize];

            foreach (var inv in db.HeroInventoryItems.AsNoTracking().Where(x => x.HeroIndex == h.Index))
                if (itemById.TryGetValue(inv.UserItemId, out var item) && inv.SlotIndex >= 0 && inv.SlotIndex < hero.Inventory.Length)
                    hero.Inventory[inv.SlotIndex] = item;

            foreach (var eq in db.HeroEquipmentItems.AsNoTracking().Where(x => x.HeroIndex == h.Index))
                if (itemById.TryGetValue(eq.UserItemId, out var item) && eq.SlotIndex >= 0 && eq.SlotIndex < hero.Equipment.Length)
                    hero.Equipment[eq.SlotIndex] = item;

            hero.Magics.Clear();
            foreach (var m in db.HeroMagics.AsNoTracking().Where(x => x.HeroIndex == h.Index))
            {
                var um = new UserMagic((Spell)m.Spell)
                {
                    Level = m.Level,
                    Key = m.Key,
                    Experience = (ushort)m.Experience,
                    IsTempSpell = m.IsTempSpell,
                    CastTime = m.CastTime,
                };
                if (um.Info != null)
                    hero.Magics.Add(um);
            }

            envir.HeroList.Add(hero);
        }

        // Accounts + characters
        envir.AccountList.Clear();
        envir.CharacterList.Clear();

        var charactersByIndex = new Dictionary<int, CharacterInfo>();

        foreach (var accRow in db.Accounts.AsNoTracking().OrderBy(x => x.Index))
        {
            var acc = new AccountInfo
            {
                Index = accRow.Index,
                AccountID = accRow.AccountId,
                UserName = accRow.UserName,
                BirthDate = accRow.BirthDate,
                SecretQuestion = accRow.SecretQuestion,
                SecretAnswer = accRow.SecretAnswer,
                EMailAddress = accRow.EmailAddress,
                CreationIP = accRow.CreationIp,
                CreationDate = accRow.CreationDate,
                Banned = accRow.Banned,
                RequirePasswordChange = accRow.RequirePasswordChange,
                BanReason = accRow.BanReason,
                ExpiryDate = accRow.ExpiryDate,
                LastIP = accRow.LastIp,
                LastDate = accRow.LastDate,
                HasExpandedStorage = accRow.HasExpandedStorage,
                ExpandedStorageExpiryDate = accRow.ExpandedStorageExpiryDate,
                Gold = (uint)Math.Max(0, accRow.Gold),
                Credit = (uint)Math.Max(0, accRow.Credit),
                AdminAccount = accRow.AdminAccount,
            };
            acc.SetPasswordHash(accRow.PasswordHash, accRow.Salt);

            acc.Storage = new UserItem[Math.Max(0, accRow.StorageSize)];
            foreach (var si in db.AccountStorageItems.AsNoTracking().Where(x => x.AccountIndex == accRow.Index))
            {
                if (!itemById.TryGetValue(si.UserItemId, out var item)) continue;
                if (si.SlotIndex < 0 || si.SlotIndex >= acc.Storage.Length) continue;
                acc.Storage[si.SlotIndex] = item;
            }

            // Characters for account
            var chars = db.Characters.AsNoTracking().Where(x => x.AccountIndex == accRow.Index).OrderBy(x => x.Index).ToList();
            foreach (var chRow in chars)
            {
                var info = new CharacterInfo
                {
                    Index = chRow.Index,
                    Name = chRow.Name,
                    Level = (ushort)chRow.Level,
                    Class = (MirClass)chRow.Class,
                    Gender = (MirGender)chRow.Gender,
                    Hair = chRow.Hair,
                    GuildIndex = chRow.GuildIndex,
                    CreationIP = chRow.CreationIp,
                    CreationDate = chRow.CreationDate,
                    Banned = chRow.Banned,
                    BanReason = chRow.BanReason,
                    ExpiryDate = chRow.ExpiryDate,
                    LastIP = chRow.LastIp,
                    LastLogoutDate = chRow.LastLogoutDate,
                    LastLoginDate = chRow.LastLoginDate,
                    Deleted = chRow.Deleted,
                    DeleteDate = chRow.DeleteDate,
                    CurrentMapIndex = chRow.CurrentMapIndex,
                    CurrentLocation = new System.Drawing.Point(chRow.CurrentLocationX, chRow.CurrentLocationY),
                    Direction = (MirDirection)chRow.Direction,
                    BindMapIndex = chRow.BindMapIndex,
                    BindLocation = new System.Drawing.Point(chRow.BindLocationX, chRow.BindLocationY),
                    HP = chRow.Hp,
                    MP = chRow.Mp,
                    Experience = chRow.Experience,
                    AMode = (AttackMode)chRow.AttackMode,
                    PMode = (PetMode)chRow.PetMode,
                    AllowGroup = chRow.AllowGroup,
                    AllowTrade = chRow.AllowTrade,
                    AllowObserve = chRow.AllowObserve,
                    PKPoints = chRow.PkPoints,
                    Thrusting = chRow.Thrusting,
                    HalfMoon = chRow.HalfMoon,
                    CrossHalfMoon = chRow.CrossHalfMoon,
                    DoubleSlash = chRow.DoubleSlash,
                    MentalState = chRow.MentalState,
                    PearlCount = chRow.PearlCount,
                    RefineTimeRemaining = chRow.RefineTimeRemaining,
                    Married = chRow.Married,
                    MarriedDate = chRow.MarriedDate,
                    Mentor = chRow.Mentor,
                    MentorDate = chRow.MentorDate,
                    IsMentor = chRow.IsMentor,
                    MentorExp = chRow.MentorExp,
                    MaximumHeroCount = chRow.MaximumHeroCount,
                    CurrentHeroIndex = chRow.CurrentHeroIndex,
                    HeroSpawned = chRow.HeroSpawned,
                    HeroBehaviour = (HeroBehaviour)chRow.HeroBehaviour,
                    AccountInfo = acc,
                };

                info.Inventory = new UserItem[Math.Max(0, chRow.InventorySize)];
                info.Equipment = new UserItem[Math.Max(0, chRow.EquipmentSize)];
                info.QuestInventory = new UserItem[Math.Max(0, chRow.QuestInventorySize)];

                foreach (var inv in db.CharacterInventoryItems.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                    if (itemById.TryGetValue(inv.UserItemId, out var item) && inv.SlotIndex >= 0 && inv.SlotIndex < info.Inventory.Length)
                        info.Inventory[inv.SlotIndex] = item;

                foreach (var eq in db.CharacterEquipmentItems.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                    if (itemById.TryGetValue(eq.UserItemId, out var item) && eq.SlotIndex >= 0 && eq.SlotIndex < info.Equipment.Length)
                        info.Equipment[eq.SlotIndex] = item;

                foreach (var qi in db.CharacterQuestInventoryItems.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                    if (itemById.TryGetValue(qi.UserItemId, out var item) && qi.SlotIndex >= 0 && qi.SlotIndex < info.QuestInventory.Length)
                        info.QuestInventory[qi.SlotIndex] = item;

                var refine = db.CharacterCurrentRefineItems.AsNoTracking().SingleOrDefault(x => x.CharacterIndex == chRow.Index);
                if (refine != null && itemById.TryGetValue(refine.UserItemId, out var refineItem))
                    info.CurrentRefine = refineItem;

                info.Flags = new bool[chRow.FlagsData.Length];
                for (var i = 0; i < chRow.FlagsData.Length; i++)
                    info.Flags[i] = chRow.FlagsData[i] != 0;

                info.Magics.Clear();
                foreach (var m in db.CharacterMagics.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                {
                    var um = new UserMagic((Spell)m.Spell)
                    {
                        Level = m.Level,
                        Key = m.Key,
                        Experience = (ushort)m.Experience,
                        IsTempSpell = m.IsTempSpell,
                        CastTime = m.CastTime,
                    };
                    if (um.Info != null)
                        info.Magics.Add(um);
                }

                info.Pets.Clear();
                foreach (var p in db.CharacterPets.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index).OrderBy(x => x.SlotIndex))
                {
                    var pet = new PetInfo
                    {
                        MonsterIndex = p.MonsterIndex,
                        HP = p.Hp,
                        Experience = (uint)Math.Max(0, p.Experience),
                        Level = (byte)p.Level,
                        MaxPetLevel = (byte)p.MaxPetLevel,
                    };
                    info.Pets.Add(pet);
                }

                info.CurrentQuests.Clear();
                foreach (var q in db.CharacterQuestProgress.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                {
                    using var r = CreateReader(q.Data);
                    var qp = new QuestProgressInfo(r, meta.Version, meta.CustomVersion);
                    if (qp == null || qp.Info == null || qp.IsOrphan) continue;
                    if (envir.BindQuest(qp))
                        info.CurrentQuests.Add(qp);
                }

                info.Buffs.Clear();
                foreach (var b in db.CharacterBuffs.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index).OrderBy(x => x.SlotIndex))
                {
                    using var r = CreateReader(b.Data);
                    info.Buffs.Add(new Buff(r, meta.Version, meta.CustomVersion));
                }

                info.IntelligentCreatures.Clear();
                foreach (var ic in db.CharacterIntelligentCreatures.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index).OrderBy(x => x.SlotIndex))
                {
                    using var r = CreateReader(ic.Data);
                    var creature = new UserIntelligentCreature(r, meta.Version, meta.CustomVersion);
                    if (creature.Info == null) continue;
                    info.IntelligentCreatures.Add(creature);
                }

                info.CompletedQuests.Clear();
                foreach (var cq in db.CharacterCompletedQuests.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                    info.CompletedQuests.Add(cq.QuestIndex);

                info.Friends.Clear();
                foreach (var fr in db.CharacterFriends.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                {
                    var f = new FriendInfo
                    {
                        Index = fr.FriendIndex,
                        Blocked = fr.Blocked,
                        Memo = fr.Memo,
                    };
                    info.Friends.Add(f);
                }

                info.RentedItems.Clear();
                foreach (var ri in db.CharacterRentedItems.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index).OrderBy(x => x.SlotIndex))
                {
                    using var r = CreateReader(ri.Data);
                    info.RentedItems.Add(new ItemRentalInformation(r, meta.Version, meta.CustomVersion));
                }
                info.HasRentedItem = info.RentedItems.Count > 0;

                info.GSpurchases.Clear();
                foreach (var gsp in db.CharacterGspurchases.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                    info.GSpurchases[gsp.ItemKey] = gsp.Count;

                // Hero slots for this character
                info.Heroes = new HeroInfo[info.MaximumHeroCount];
                foreach (var hs in db.CharacterHeroSlots.AsNoTracking().Where(x => x.CharacterIndex == chRow.Index))
                {
                    var hero = envir.HeroList.FirstOrDefault(x => x.Index == hs.HeroIndex);
                    if (hero == null) continue;
                    if (hs.SlotIndex < 0 || hs.SlotIndex >= info.Heroes.Length) continue;
                    info.Heroes[hs.SlotIndex] = hero;
                }

                acc.Characters.Add(info);
                envir.CharacterList.Add(info);
                charactersByIndex[info.Index] = info;
            }

            envir.AccountList.Add(acc);
        }

        // Auctions + rebuild account->auctions linkage
        envir.Auctions.Clear();
        foreach (var auctionRow in db.Auctions.AsNoTracking().OrderBy(x => x.AuctionId))
        {
            if (!itemById.TryGetValue(auctionRow.UserItemId, out var item)) continue;

            var a = new AuctionInfo
            {
                AuctionID = auctionRow.AuctionId,
                Item = item,
                ConsignmentDate = auctionRow.ConsignmentDate,
                Price = (uint)Math.Max(0, auctionRow.Price),
                CurrentBid = (uint)Math.Max(0, auctionRow.CurrentBid),
                SellerIndex = auctionRow.SellerIndex,
                CurrentBuyerIndex = auctionRow.CurrentBuyerIndex ?? 0,
                Expired = auctionRow.Expired,
                Sold = auctionRow.Sold,
                ItemType = (MarketItemType)auctionRow.ItemType,
            };

            if (charactersByIndex.TryGetValue(a.SellerIndex, out var seller))
                a.SellerInfo = seller;
            if (a.CurrentBuyerIndex != 0 && charactersByIndex.TryGetValue(a.CurrentBuyerIndex, out var buyer))
                a.CurrentBuyerInfo = buyer;

            envir.Auctions.AddLast(a);
            a.SellerInfo?.AccountInfo?.Auctions.AddLast(a);
        }

        // Mail: attach to recipient inbox
        foreach (var c in envir.CharacterList)
            c.Mail.Clear();

        var mailRows = db.Mail.AsNoTracking().ToList();
        var mailItems = db.MailItems.AsNoTracking().ToList();
        var mailItemsByMail = mailItems.GroupBy(x => x.MailId).ToDictionary(g => g.Key, g => g.OrderBy(x => x.SlotIndex).ToList());

        foreach (var mr in mailRows)
        {
            var m = new MailInfo
            {
                MailID = mr.MailId,
                Sender = mr.Sender,
                RecipientIndex = mr.RecipientIndex,
                Message = mr.Message,
                Gold = (uint)Math.Max(0, mr.Gold),
                DateSent = mr.DateSent,
                DateOpened = mr.DateOpened,
                Locked = mr.Locked,
                Collected = mr.Collected,
                CanReply = mr.CanReply,
            };

            if (mailItemsByMail.TryGetValue(m.MailID, out var attachments))
            {
                m.Items.Clear();
                foreach (var att in attachments)
                    if (itemById.TryGetValue(att.UserItemId, out var it))
                        m.Items.Add(it);
            }

            if (charactersByIndex.TryGetValue(m.RecipientIndex, out var recipient))
            {
                m.RecipientInfo = recipient;
                recipient.Mail.Add(m);
            }
        }

        // Gameshop log
        envir.GameshopLog.Clear();
        foreach (var row in db.GameshopLog.AsNoTracking())
            envir.GameshopLog[row.ItemKey] = row.Count;

        // Apply respawn saves if the runtime has respawn instances (parity with legacy LoadAccounts)
        if (envir.SavedSpawns.Count > 0)
        {
            foreach (var saved in db.RespawnSaves.AsNoTracking())
            {
                foreach (var respawn in envir.SavedSpawns)
                {
                    if (respawn.Info.RespawnIndex != saved.RespawnIndex) continue;

                    respawn.NextSpawnTick = (ulong)Math.Max(0, saved.NextSpawnTick);
                    if (!saved.Spawned || respawn.Info.Count * envir.SpawnMultiplier <= respawn.Count)
                        continue;

                    var mobcount = respawn.Info.Count * envir.SpawnMultiplier - respawn.Count;
                    for (var j = 0; j < mobcount; j++)
                        respawn.Spawn();
                }
            }
        }

        // Guilds: rebuild GuildList and GuildObjects
        envir.GuildList.Clear();
        envir.Guilds.Clear();

        var guildRows = db.Guilds.AsNoTracking().OrderBy(x => x.GuildIndex).ToList();
        var ranks = db.GuildRanks.AsNoTracking().ToList();
        var members = db.GuildMembers.AsNoTracking().ToList();
        var notices = db.GuildNotices.AsNoTracking().ToList();
        var buffs = db.GuildBuffs.AsNoTracking().ToList();
        var storage = db.GuildStorageItems.AsNoTracking().ToList();

        foreach (var gr in guildRows)
        {
            var g = new GuildInfo
            {
                GuildIndex = gr.GuildIndex,
                Name = gr.Name,
                Level = (byte)gr.Level,
                SparePoints = (byte)gr.SparePoints,
                Experience = gr.Experience,
                Gold = (uint)Math.Max(0, gr.Gold),
                Votes = gr.Votes,
                LastVoteAttempt = gr.LastVoteAttempt,
                Voting = gr.Voting,
                FlagImage = (ushort)gr.FlagImage,
                FlagColour = System.Drawing.Color.FromArgb(gr.FlagColourArgb),
                GTRent = gr.GtRent,
                GTBegin = gr.GtBegin,
                GTIndex = gr.GtIndex,
                GTKey = gr.GtKey,
                GTPrice = gr.GtPrice,
            };

            // derived fields (parity with GuildInfo(BinaryReader))
            if (g.Level < Settings.Guild_ExperienceList.Count)
                g.MaxExperience = Settings.Guild_ExperienceList[g.Level];

            if (g.Name == Settings.NewbieGuild)
            {
                g.MemberCap = Settings.NewbieGuildMaxSize;
            }
            else if (g.Level < Settings.Guild_MembercapList.Count)
            {
                g.MemberCap = Settings.Guild_MembercapList[g.Level];
            }

            var guildRanks = ranks.Where(x => x.GuildIndex == gr.GuildIndex).OrderBy(x => x.RankIndex).ToList();
            g.Ranks.Clear();
            g.Membercount = 0;

            foreach (var rr in guildRanks)
            {
                var rank = new GuildRank
                {
                    Name = rr.Name,
                    Index = rr.RankIndex,
                    Options = (GuildRankOptions)rr.Options,
                    Members = new List<GuildMember>(),
                };

                var rankMembers = members
                    .Where(x => x.GuildIndex == rr.GuildIndex && x.RankIndex == rr.RankIndex)
                    .OrderBy(x => x.MemberIndex)
                    .ToList();

                foreach (var mr in rankMembers)
                {
                    rank.Members.Add(new GuildMember
                    {
                        Name = mr.Name,
                        Id = mr.Id,
                        Player = null,
                        LastLogin = mr.LastLogin,
                        hasvoted = mr.HasVoted,
                        Online = mr.Online,
                    });
                }

                g.Membercount += rank.Members.Count;
                g.Ranks.Add(rank);
            }

            g.Notice.Clear();
            foreach (var n in notices.Where(x => x.GuildIndex == gr.GuildIndex).OrderBy(x => x.LineIndex))
                g.Notice.Add(n.Text);

            g.BuffList.Clear();
            foreach (var b in buffs.Where(x => x.GuildIndex == gr.GuildIndex).OrderBy(x => x.BuffIndex))
            {
                var gb = new GuildBuff
                {
                    Id = b.Id,
                    Active = b.Active,
                    ActiveTimeRemaining = b.ActiveTimeRemaining,
                };
                gb.Info = envir.FindGuildBuffInfo(gb.Id);
                g.BuffList.Add(gb);
            }

            g.StoredItems = new GuildStorageItem[112];
            foreach (var si in storage.Where(x => x.GuildIndex == gr.GuildIndex))
            {
                if (!itemById.TryGetValue(si.UserItemId, out var it)) continue;
                if (si.SlotIndex < 0 || si.SlotIndex >= g.StoredItems.Length) continue;
                g.StoredItems[si.SlotIndex] = new GuildStorageItem
                {
                    Item = it,
                    UserId = si.UserId,
                };
            }

            envir.GuildList.Add(g);
            _ = new GuildObject(g);
        }

        // Conquests: rebuild ConquestList + ConquestObjects (parity with LoadConquests)
        envir.Conquests.Clear();
        envir.ConquestList.Clear();

        var conquestState = db.Conquests.AsNoTracking().ToDictionary(x => x.ConquestIndex);
        var archers = db.ConquestArchers.AsNoTracking().ToList();
        var gates = db.ConquestGates.AsNoTracking().ToList();
        var walls = db.ConquestWalls.AsNoTracking().ToList();
        var sieges = db.ConquestSieges.AsNoTracking().ToList();

        for (var i = 0; i < envir.ConquestInfoList.Count; i++)
        {
            var info = envir.ConquestInfoList[i];
            var tempMap = envir.GetMap(info.MapIndex);
            if (tempMap == null) continue;

            var state = new ConquestGuildInfo { Info = info };
            if (conquestState.TryGetValue(info.Index, out var s))
            {
                state.Owner = s.Owner;
                state.GoldStorage = (uint)Math.Max(0, s.GoldStorage);
                state.AttackerID = s.AttackerId;
                state.NPCRate = (byte)s.NpcRate;
            }
            else
            {
                state.NeedSave = true;
            }

            state.ArcherList.Clear();
            foreach (var a in archers.Where(x => x.ConquestIndex == info.Index))
                state.ArcherList.Add(new ConquestGuildArcherInfo { Index = a.Index, Alive = a.Alive });

            state.GateList.Clear();
            foreach (var g in gates.Where(x => x.ConquestIndex == info.Index))
                state.GateList.Add(new ConquestGuildGateInfo { Index = g.Index, Health = g.Health });

            state.WallList.Clear();
            foreach (var w in walls.Where(x => x.ConquestIndex == info.Index))
                state.WallList.Add(new ConquestGuildWallInfo { Index = w.Index, Health = w.Health });

            state.SiegeList.Clear();
            foreach (var sg in sieges.Where(x => x.ConquestIndex == info.Index))
                state.SiegeList.Add(new ConquestGuildSiegeInfo { Index = sg.Index, Health = sg.Health });

            var newConquest = new ConquestObject(state) { ConquestMap = tempMap };

            for (var k = 0; k < envir.Guilds.Count; k++)
            {
                if (state.Owner != envir.Guilds[k].Guildindex) continue;
                newConquest.Guild = envir.Guilds[k];
                envir.Guilds[k].Conquest = newConquest;
            }

            envir.ConquestList.Add(state);
            envir.Conquests.Add(newConquest);
            tempMap.Conquest.Add(newConquest);
            newConquest.Bind();
        }

        // NPC used goods: attach to live NPC objects if present
        var goods = db.NpcUsedGoods.AsNoTracking().ToList();
        var goodsByNpc = goods.GroupBy(x => x.NpcIndex).ToDictionary(g => g.Key, g => g.OrderBy(x => x.SlotIndex).ToList());

        foreach (var npc in envir.NPCs)
        {
            npc.UsedGoods.Clear();
            if (!goodsByNpc.TryGetValue(npc.Info.Index, out var list)) continue;

            foreach (var g in list)
                if (itemById.TryGetValue(g.UserItemId, out var it))
                    npc.UsedGoods.Add(it);
        }
    }

    public void SaveAll(Envir envir, bool forced)
    {
        using var db = CreateDb();
        db.Database.Migrate();

        using var tx = db.Database.BeginTransaction();

        // wipe snapshot tables (simple + correct; can be optimized later)
        foreach (var table in TablesInDeleteOrder)
            db.Database.ExecuteSqlRaw("DELETE FROM \"" + table + "\";");

        // reuse the importer-style projection, but from live in-memory state
        var importer = new BinaryFileImporter(_connectionString, _log);
        // ImportInternal is file-driven; so instead we implement a direct projection here:
        // 1) DbMeta
        // 2) Static payload
        // 3) Dynamic state

        var meta = new DbMetaEntity
        {
            Id = 1,
            Version = Envir.Version,
            CustomVersion = Envir.CustomVersion,
            MapIndex = envir.MapIndex,
            ItemIndex = envir.ItemIndex,
            MonsterIndex = envir.MonsterIndex,
            NpcIndex = envir.NPCIndex,
            QuestIndex = envir.QuestIndex,
            GameshopIndex = envir.GameshopIndex,
            ConquestIndex = envir.ConquestIndex,
            RespawnIndex = envir.RespawnIndex,
            NextAccountId = envir.NextAccountID,
            NextCharacterId = envir.NextCharacterID,
            NextUserItemId = envir.NextUserItemID,
            NextHeroId = envir.NextHeroID,
            GuildCount = envir.GuildList.Count,
            NextGuildId = envir.NextGuildID,
            NextAuctionId = envir.NextAuctionID,
            NextMailId = envir.NextMailID,
            ConcurrencyToken = Guid.NewGuid().ToByteArray(),
        };

        db.DbMeta.Add(meta);

        db.MapInfos.AddRange(envir.MapInfoList.Select(m => new MapInfoEntity { Index = m.Index, Data = ToBytes(w => m.Save(w)) }));
        db.ItemInfos.AddRange(envir.ItemInfoList.Select(i => new ItemInfoEntity { Index = i.Index, Data = ToBytes(w => i.Save(w)) }));
        db.MonsterInfos.AddRange(envir.MonsterInfoList.Select(m => new MonsterInfoEntity { Index = m.Index, Data = ToBytes(w => m.Save(w)) }));
        db.NpcInfos.AddRange(envir.NPCInfoList.Select(n => new NpcInfoEntity { Index = n.Index, Data = ToBytes(w => n.Save(w)) }));
        db.QuestInfos.AddRange(envir.QuestInfoList.Select(q => new QuestInfoEntity { Index = q.Index, Data = ToBytes(w => q.Save(w)) }));
        db.MagicInfos.AddRange(envir.MagicInfoList.Select(m => new MagicInfoEntity { Spell = (byte)m.Spell, Data = ToBytes(w => m.Save(w)) }));
        db.GameShopItems.AddRange(envir.GameShopList.Select(g => new GameShopItemEntity { GIndex = g.GIndex, Data = ToBytes(w => g.Save(w)) }));
        db.ConquestInfos.AddRange(envir.ConquestInfoList.Select(c => new ConquestInfoEntity { Index = c.Index, Data = ToBytes(w => c.Save(w)) }));
        db.GtMaps.AddRange(envir.GTMapList.Select(g => new GtMapEntity { Index = g.Index, Data = ToBytes(w => g.Save(w)) }));
        db.DragonInfoState.Add(new DragonInfoStateEntity { Id = 1, Data = ToBytes(w => envir.DragonInfo.Save(w)) });
        db.RespawnTimerState.Add(new RespawnTimerStateEntity { Id = 1, Data = ToBytes(w => envir.RespawnTick.Save(w)) });

        // Collect user items from all known roots (same strategy as legacy importer)
        var userItems = new Dictionary<ulong, UserItemEntity>();
        var userItemSlots = new List<UserItemSlotEntity>();

        void collect(UserItem item)
        {
            if (item == null) return;
            if (!userItems.ContainsKey(item.UniqueID))
            {
                userItems[item.UniqueID] = new UserItemEntity
                {
                    UserItemId = item.UniqueID,
                    ItemIndex = item.ItemIndex,
                    CurrentDura = item.CurrentDura,
                    MaxDura = item.MaxDura,
                    Count = item.Count,
                    GemCount = item.GemCount,
                    SoulBoundId = item.SoulBoundId,
                    Identified = item.Identified,
                    Cursed = item.Cursed,
                    WeddingRing = item.WeddingRing,
                    RefinedValue = (byte)item.RefinedValue,
                    RefineAdded = item.RefineAdded,
                    RefineSuccessChance = item.RefineSuccessChance,
                    SlotsLength = item.Slots?.Length ?? 0,
                    AddedStatsData = ToBytes(w => item.AddedStats.Save(w)),
                    AwakeData = ToBytes(w => item.Awake.Save(w)),
                    ExpireInfoData = item.ExpireInfo == null ? Array.Empty<byte>() : ToBytes(w => item.ExpireInfo.Save(w)),
                    RentalInformationData = item.RentalInformation == null ? Array.Empty<byte>() : ToBytes(w => item.RentalInformation.Save(w)),
                    SealedInfoData = item.SealedInfo == null ? Array.Empty<byte>() : ToBytes(w => item.SealedInfo.Save(w)),
                    IsShopItem = item.IsShopItem,
                    GmMade = item.GMMade,
                };
            }

            if (item.Slots == null) return;
            for (var i = 0; i < item.Slots.Length; i++)
            {
                var child = item.Slots[i];
                if (child == null) continue;
                collect(child);
                userItemSlots.Add(new UserItemSlotEntity { ParentUserItemId = item.UniqueID, SlotIndex = i, ChildUserItemId = child.UniqueID });
            }
        }

        foreach (var acc in envir.AccountList)
        {
            for (var i = 0; i < acc.Storage.Length; i++) collect(acc.Storage[i]);
            foreach (var ch in acc.Characters)
            {
                for (var i = 0; i < ch.Inventory.Length; i++) collect(ch.Inventory[i]);
                for (var i = 0; i < ch.Equipment.Length; i++) collect(ch.Equipment[i]);
                for (var i = 0; i < ch.QuestInventory.Length; i++) collect(ch.QuestInventory[i]);
                collect(ch.CurrentRefine);
                foreach (var mail in ch.Mail)
                    foreach (var it in mail.Items)
                        collect(it);
            }
        }
        foreach (var hero in envir.HeroList)
        {
            for (var i = 0; i < hero.Inventory.Length; i++) collect(hero.Inventory[i]);
            for (var i = 0; i < hero.Equipment.Length; i++) collect(hero.Equipment[i]);
        }
        foreach (var auction in envir.Auctions) collect(auction.Item);
        foreach (var guild in envir.GuildList)
            for (var i = 0; i < guild.StoredItems.Length; i++)
                if (guild.StoredItems[i] != null) collect(guild.StoredItems[i].Item);
        foreach (var npc in envir.NPCs)
            foreach (var it in npc.UsedGoods)
                collect(it);

        db.UserItems.AddRange(userItems.Values);
        db.UserItemSlots.AddRange(userItemSlots);

        // Heroes
        db.Heroes.AddRange(envir.HeroList.Select(h => new HeroEntity
        {
            Index = h.Index,
            Name = h.Name ?? string.Empty,
            Level = h.Level,
            Class = (byte)h.Class,
            Gender = (byte)h.Gender,
            Hair = h.Hair,
            CreationDate = h.CreationDate,
            Deleted = h.Deleted,
            DeleteDate = h.DeleteDate,
            Hp = h.HP,
            Mp = h.MP,
            Experience = h.Experience,
            InventorySize = h.Inventory.Length,
            EquipmentSize = h.Equipment.Length,
            AutoPot = h.AutoPot,
            Grade = h.Grade,
            HpItemIndex = h.HPItemIndex,
            MpItemIndex = h.MPItemIndex,
            AutoHpPercent = h.AutoHPPercent,
            AutoMpPercent = h.AutoMPPercent,
            SealCount = h.SealCount,
        }));

        db.HeroInventoryItems.AddRange(envir.HeroList.SelectMany(h => h.Inventory.Select((it, idx) => new { h.Index, it, idx }))
            .Where(x => x.it != null)
            .Select(x => new HeroInventoryItemEntity { HeroIndex = x.Index, SlotIndex = x.idx, UserItemId = x.it.UniqueID }));

        db.HeroEquipmentItems.AddRange(envir.HeroList.SelectMany(h => h.Equipment.Select((it, idx) => new { h.Index, it, idx }))
            .Where(x => x.it != null)
            .Select(x => new HeroEquipmentItemEntity { HeroIndex = x.Index, SlotIndex = x.idx, UserItemId = x.it.UniqueID }));

        db.HeroMagics.AddRange(envir.HeroList.SelectMany(h => h.Magics.Select(m => new HeroMagicEntity
        {
            HeroIndex = h.Index,
            Spell = (byte)m.Spell,
            Level = m.Level,
            Key = m.Key,
            Experience = m.Experience,
            IsTempSpell = m.IsTempSpell,
            CastTime = m.CastTime,
        })));

        // Accounts + Characters + mail/auctions etc can be projected similarly; for initial cutover we keep correctness over perf.
        foreach (var acc in envir.AccountList)
        {
            db.Accounts.Add(new AccountEntity
            {
                Index = acc.Index,
                AccountId = acc.AccountID ?? string.Empty,
                PasswordHash = acc.Password ?? string.Empty,
                Salt = acc.Salt ?? Array.Empty<byte>(),
                RequirePasswordChange = acc.RequirePasswordChange,
                UserName = acc.UserName ?? string.Empty,
                BirthDate = acc.BirthDate,
                SecretQuestion = acc.SecretQuestion ?? string.Empty,
                SecretAnswer = acc.SecretAnswer ?? string.Empty,
                EmailAddress = acc.EMailAddress ?? string.Empty,
                CreationIp = acc.CreationIP ?? string.Empty,
                CreationDate = acc.CreationDate,
                Banned = acc.Banned,
                BanReason = acc.BanReason ?? string.Empty,
                ExpiryDate = acc.ExpiryDate,
                LastIp = acc.LastIP ?? string.Empty,
                LastDate = acc.LastDate,
                HasExpandedStorage = acc.HasExpandedStorage,
                ExpandedStorageExpiryDate = acc.ExpandedStorageExpiryDate,
                Gold = acc.Gold,
                Credit = acc.Credit,
                StorageSize = acc.Storage?.Length ?? 0,
                AdminAccount = acc.AdminAccount,
            });

            for (var si = 0; si < acc.Storage.Length; si++)
            {
                if (acc.Storage[si] == null) continue;
                db.AccountStorageItems.Add(new AccountStorageItemEntity { AccountIndex = acc.Index, SlotIndex = si, UserItemId = acc.Storage[si].UniqueID });
            }

            foreach (var ch in acc.Characters)
            {
                var flags = new byte[ch.Flags.Length];
                for (var i = 0; i < ch.Flags.Length; i++) flags[i] = ch.Flags[i] ? (byte)1 : (byte)0;

                db.Characters.Add(new CharacterEntity
                {
                    Index = ch.Index,
                    AccountIndex = acc.Index,
                    Name = ch.Name ?? string.Empty,
                    Level = ch.Level,
                    Class = (byte)ch.Class,
                    Gender = (byte)ch.Gender,
                    Hair = ch.Hair,
                    GuildIndex = ch.GuildIndex,
                    CreationIp = ch.CreationIP ?? string.Empty,
                    CreationDate = ch.CreationDate,
                    Banned = ch.Banned,
                    BanReason = ch.BanReason ?? string.Empty,
                    ExpiryDate = ch.ExpiryDate,
                    LastIp = ch.LastIP ?? string.Empty,
                    LastLogoutDate = ch.LastLogoutDate,
                    LastLoginDate = ch.LastLoginDate,
                    Deleted = ch.Deleted,
                    DeleteDate = ch.DeleteDate,
                    CurrentMapIndex = ch.CurrentMapIndex,
                    CurrentLocationX = ch.CurrentLocation.X,
                    CurrentLocationY = ch.CurrentLocation.Y,
                    Direction = (byte)ch.Direction,
                    BindMapIndex = ch.BindMapIndex,
                    BindLocationX = ch.BindLocation.X,
                    BindLocationY = ch.BindLocation.Y,
                    Hp = ch.HP,
                    Mp = ch.MP,
                    Experience = ch.Experience,
                    AttackMode = (byte)ch.AMode,
                    PetMode = (byte)ch.PMode,
                    AllowGroup = ch.AllowGroup,
                    AllowTrade = ch.AllowTrade,
                    AllowObserve = ch.AllowObserve,
                    PkPoints = ch.PKPoints,
                    Thrusting = ch.Thrusting,
                    HalfMoon = ch.HalfMoon,
                    CrossHalfMoon = ch.CrossHalfMoon,
                    DoubleSlash = ch.DoubleSlash,
                    MentalState = ch.MentalState,
                    InventorySize = ch.Inventory.Length,
                    EquipmentSize = ch.Equipment.Length,
                    QuestInventorySize = ch.QuestInventory.Length,
                    FlagsData = flags,
                    PearlCount = ch.PearlCount,
                    RefineTimeRemaining = ch.RefineTimeRemaining,
                    Married = ch.Married,
                    MarriedDate = ch.MarriedDate,
                    Mentor = ch.Mentor,
                    MentorDate = ch.MentorDate,
                    IsMentor = ch.IsMentor,
                    MentorExp = ch.MentorExp,
                    MaximumHeroCount = ch.MaximumHeroCount,
                    CurrentHeroIndex = ch.CurrentHeroIndex,
                    HeroSpawned = ch.HeroSpawned,
                    HeroBehaviour = (byte)ch.HeroBehaviour,
                });

                for (var si = 0; si < ch.Inventory.Length; si++)
                    if (ch.Inventory[si] != null)
                        db.CharacterInventoryItems.Add(new CharacterInventoryItemEntity { CharacterIndex = ch.Index, SlotIndex = si, UserItemId = ch.Inventory[si].UniqueID });

                for (var si = 0; si < ch.Equipment.Length; si++)
                    if (ch.Equipment[si] != null)
                        db.CharacterEquipmentItems.Add(new CharacterEquipmentItemEntity { CharacterIndex = ch.Index, SlotIndex = si, UserItemId = ch.Equipment[si].UniqueID });

                for (var si = 0; si < ch.QuestInventory.Length; si++)
                    if (ch.QuestInventory[si] != null)
                        db.CharacterQuestInventoryItems.Add(new CharacterQuestInventoryItemEntity { CharacterIndex = ch.Index, SlotIndex = si, UserItemId = ch.QuestInventory[si].UniqueID });

                if (ch.CurrentRefine != null)
                    db.CharacterCurrentRefineItems.Add(new CharacterCurrentRefineItemEntity { CharacterIndex = ch.Index, UserItemId = ch.CurrentRefine.UniqueID });

                db.CharacterMagics.AddRange(ch.Magics.Select(m => new CharacterMagicEntity
                {
                    CharacterIndex = ch.Index,
                    Spell = (byte)m.Spell,
                    Level = m.Level,
                    Key = m.Key,
                    Experience = m.Experience,
                    IsTempSpell = m.IsTempSpell,
                    CastTime = m.CastTime,
                }));

                db.CharacterPets.AddRange(ch.Pets.Select((p, idx) => new CharacterPetEntity
                {
                    CharacterIndex = ch.Index,
                    SlotIndex = idx,
                    MonsterIndex = p.MonsterIndex,
                    Hp = p.HP,
                    Experience = p.Experience,
                    Level = p.Level,
                    MaxPetLevel = p.MaxPetLevel,
                }));

                db.CharacterQuestProgress.AddRange(ch.CurrentQuests.Select(q => new CharacterQuestProgressEntity
                {
                    CharacterIndex = ch.Index,
                    QuestIndex = q.Index,
                    Data = ToBytes(w => q.Save(w)),
                }));

                db.CharacterBuffs.AddRange(ch.Buffs.Select((b, idx) => new CharacterBuffEntity
                {
                    CharacterIndex = ch.Index,
                    SlotIndex = idx,
                    Data = ToBytes(w => b.Save(w)),
                }));

                db.CharacterIntelligentCreatures.AddRange(ch.IntelligentCreatures.Select((ic, idx) => new CharacterIntelligentCreatureEntity
                {
                    CharacterIndex = ch.Index,
                    SlotIndex = idx,
                    Data = ToBytes(w => ic.Save(w)),
                }));

                db.CharacterCompletedQuests.AddRange(ch.CompletedQuests.Select(q => new CharacterCompletedQuestEntity { CharacterIndex = ch.Index, QuestIndex = q }));

                db.CharacterFriends.AddRange(ch.Friends.Where(f => f.Info != null).Select(f => new CharacterFriendEntity
                {
                    CharacterIndex = ch.Index,
                    FriendIndex = f.Index,
                    Blocked = f.Blocked,
                    Memo = f.Memo ?? string.Empty,
                }));

                db.CharacterRentedItems.AddRange(ch.RentedItems.Select((r, idx) => new CharacterRentedItemEntity
                {
                    CharacterIndex = ch.Index,
                    SlotIndex = idx,
                    Data = ToBytes(w => r.Save(w)),
                }));

                db.CharacterGspurchases.AddRange(ch.GSpurchases.Select(kvp => new CharacterGsPurchaseEntity
                {
                    CharacterIndex = ch.Index,
                    ItemKey = kvp.Key,
                    Count = kvp.Value,
                }));

                if (ch.Heroes != null)
                {
                    for (var hi = 0; hi < ch.Heroes.Length; hi++)
                    {
                        if (ch.Heroes[hi] == null) continue;
                        db.CharacterHeroSlots.Add(new CharacterHeroSlotEntity { CharacterIndex = ch.Index, SlotIndex = hi, HeroIndex = ch.Heroes[hi].Index });
                    }
                }

                // Mail
                foreach (var m in ch.Mail)
                {
                    db.Mail.Add(new MailEntity
                    {
                        MailId = m.MailID,
                        Sender = m.Sender ?? string.Empty,
                        RecipientIndex = m.RecipientIndex,
                        Message = m.Message ?? string.Empty,
                        Gold = m.Gold,
                        DateSent = m.DateSent,
                        DateOpened = m.DateOpened,
                        Locked = m.Locked,
                        Collected = m.Collected,
                        CanReply = m.CanReply,
                    });

                    for (var mi = 0; mi < m.Items.Count; mi++)
                        db.MailItems.Add(new MailItemEntity { MailId = m.MailID, SlotIndex = mi, UserItemId = m.Items[mi].UniqueID });
                }
            }
        }

        // Auctions
        foreach (var a in envir.Auctions)
        {
            db.Auctions.Add(new AuctionEntity
            {
                AuctionId = a.AuctionID,
                UserItemId = a.Item.UniqueID,
                ConsignmentDate = a.ConsignmentDate,
                Price = a.Price,
                CurrentBid = a.CurrentBid,
                SellerIndex = a.SellerIndex,
                CurrentBuyerIndex = a.CurrentBuyerIndex == 0 ? null : a.CurrentBuyerIndex,
                Expired = a.Expired,
                Sold = a.Sold,
                ItemType = (byte)a.ItemType,
            });
        }

        // GameshopLog
        db.GameshopLog.AddRange(envir.GameshopLog.Select(kvp => new GameshopLogEntryEntity { ItemKey = kvp.Key, Count = kvp.Value }));

        // Respawn saves (if runtime has them)
        db.RespawnSaves.AddRange(envir.SavedSpawns.Select(spawn => new RespawnSaveEntity
        {
            RespawnIndex = spawn.Info.RespawnIndex,
            NextSpawnTick = (long)spawn.NextSpawnTick,
            Spawned = spawn.Count >= spawn.Info.Count * envir.SpawnMultiplier,
        }));

        // Guilds
        foreach (var g in envir.GuildList)
        {
            db.Guilds.Add(new GuildEntity
            {
                GuildIndex = g.GuildIndex,
                Name = g.Name ?? string.Empty,
                Level = g.Level,
                SparePoints = g.SparePoints,
                Experience = g.Experience,
                Gold = g.Gold,
                Votes = g.Votes,
                LastVoteAttempt = g.LastVoteAttempt,
                Voting = g.Voting,
                FlagImage = g.FlagImage,
                FlagColourArgb = g.FlagColour.ToArgb(),
                GtRent = g.GTRent,
                GtBegin = g.GTBegin,
                GtIndex = g.GTIndex,
                GtKey = g.GTKey,
                GtPrice = g.GTPrice,
            });

            for (var ri = 0; ri < g.Ranks.Count; ri++)
            {
                var r = g.Ranks[ri];
                db.GuildRanks.Add(new GuildRankEntity { GuildIndex = g.GuildIndex, RankIndex = ri, Name = r.Name ?? string.Empty, Options = (byte)r.Options });
                for (var mi = 0; mi < r.Members.Count; mi++)
                {
                    var m = r.Members[mi];
                    db.GuildMembers.Add(new GuildMemberEntity
                    {
                        GuildIndex = g.GuildIndex,
                        RankIndex = ri,
                        MemberIndex = mi,
                        Name = m.Name ?? string.Empty,
                        Id = m.Id,
                        LastLogin = m.LastLogin,
                        HasVoted = m.hasvoted,
                        Online = m.Online,
                    });
                }
            }

            for (var ni = 0; ni < g.Notice.Count; ni++)
                db.GuildNotices.Add(new GuildNoticeEntity { GuildIndex = g.GuildIndex, LineIndex = ni, Text = g.Notice[ni] ?? string.Empty });

            for (var bi = 0; bi < g.BuffList.Count; bi++)
            {
                var b = g.BuffList[bi];
                db.GuildBuffs.Add(new GuildBuffEntity { GuildIndex = g.GuildIndex, BuffIndex = bi, Id = b.Id, Active = b.Active, ActiveTimeRemaining = b.ActiveTimeRemaining });
            }

            for (var si = 0; si < g.StoredItems.Length; si++)
            {
                if (g.StoredItems[si] == null) continue;
                db.GuildStorageItems.Add(new GuildStorageItemEntity { GuildIndex = g.GuildIndex, SlotIndex = si, UserItemId = g.StoredItems[si].Item.UniqueID, UserId = g.StoredItems[si].UserId });
            }
        }

        // Conquests
        foreach (var c in envir.ConquestList)
        {
            if (c.Info == null) continue;
            db.Conquests.Add(new ConquestStateEntity { ConquestIndex = c.Info.Index, Owner = c.Owner, GoldStorage = c.GoldStorage, AttackerId = c.AttackerID, NpcRate = c.NPCRate });

            foreach (var a in c.ArcherList)
                db.ConquestArchers.Add(new ConquestArcherStateEntity { ConquestIndex = c.Info.Index, Index = a.Index, Alive = a.Alive });
            foreach (var g in c.GateList)
                db.ConquestGates.Add(new ConquestGateStateEntity { ConquestIndex = c.Info.Index, Index = g.Index, Health = g.Health });
            foreach (var w in c.WallList)
                db.ConquestWalls.Add(new ConquestWallStateEntity { ConquestIndex = c.Info.Index, Index = w.Index, Health = w.Health });
            foreach (var s in c.SiegeList)
                db.ConquestSieges.Add(new ConquestSiegeStateEntity { ConquestIndex = c.Info.Index, Index = s.Index, Health = s.Health });
        }

        // NPC used goods
        foreach (var npc in envir.NPCs)
        {
            for (var i = 0; i < npc.UsedGoods.Count; i++)
            {
                db.NpcUsedGoods.Add(new NpcUsedGoodEntity { NpcIndex = npc.Info.Index, SlotIndex = i, UserItemId = npc.UsedGoods[i].UniqueID });
            }
        }

        db.SaveChanges();
        tx.Commit();
    }

    private Mir2DbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<Mir2DbContext>()
            .UseSqlite(_connectionString)
            .Options;

        return new Mir2DbContext(options);
    }

    private static void MirrorStaticToEditEnvir(Envir main)
    {
        // Keep Edit in sync for lookups (eg BindGameShop uses Edit.ItemInfoList).
        Envir.Edit.MapIndex = main.MapIndex;
        Envir.Edit.ItemIndex = main.ItemIndex;
        Envir.Edit.MonsterIndex = main.MonsterIndex;
        Envir.Edit.NPCIndex = main.NPCIndex;
        Envir.Edit.QuestIndex = main.QuestIndex;
        Envir.Edit.GameshopIndex = main.GameshopIndex;
        Envir.Edit.ConquestIndex = main.ConquestIndex;
        Envir.Edit.RespawnIndex = main.RespawnIndex;

        Envir.Edit.MapInfoList = main.MapInfoList;
        Envir.Edit.ItemInfoList = main.ItemInfoList;
        Envir.Edit.MonsterInfoList = main.MonsterInfoList;
        Envir.Edit.NPCInfoList = main.NPCInfoList;
        Envir.Edit.QuestInfoList = main.QuestInfoList;
        Envir.Edit.MagicInfoList = main.MagicInfoList;
        Envir.Edit.GameShopList = main.GameShopList;
        Envir.Edit.ConquestInfoList = main.ConquestInfoList;
        Envir.Edit.GTMapList = main.GTMapList;
        Envir.Edit.DragonInfo = main.DragonInfo;
        Envir.Edit.RespawnTick = main.RespawnTick;
    }

    private static BinaryReader CreateReader(byte[] data)
    {
        return new BinaryReader(new MemoryStream(data, writable: false));
    }

    private static byte[] ToBytes(Action<BinaryWriter> write)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        write(writer);
        writer.Flush();
        return ms.ToArray();
    }

    private static readonly string[] TablesInDeleteOrder =
    {
        // child tables first
        "NpcUsedGoods",
        "ConquestSieges",
        "ConquestWalls",
        "ConquestGates",
        "ConquestArchers",
        "Conquests",
        "GuildStorageItems",
        "GuildBuffs",
        "GuildNotices",
        "GuildMembers",
        "GuildRanks",
        "Guilds",
        "RespawnSaves",
        "GameshopLog",
        "MailItems",
        "Mail",
        "Auctions",
        "CharacterHeroSlots",
        "CharacterGspurchases",
        "CharacterRentedItems",
        "CharacterFriends",
        "CharacterCompletedQuests",
        "CharacterIntelligentCreatures",
        "CharacterBuffs",
        "CharacterQuestProgress",
        "CharacterPets",
        "CharacterMagics",
        "CharacterCurrentRefineItems",
        "CharacterQuestInventoryItems",
        "CharacterEquipmentItems",
        "CharacterInventoryItems",
        "AccountStorageItems",
        "Characters",
        "Accounts",
        "HeroMagics",
        "HeroEquipmentItems",
        "HeroInventoryItems",
        "Heroes",
        "UserItemSlots",
        "UserItems",
        // static payload tables
        "RespawnTimerState",
        "DragonInfoState",
        "GtMaps",
        "ConquestInfos",
        "GameShopItems",
        "MagicInfos",
        "QuestInfos",
        "NpcInfos",
        "MonsterInfos",
        "ItemInfos",
        "MapInfos",
        // meta last
        "DbMeta",
    };
}

