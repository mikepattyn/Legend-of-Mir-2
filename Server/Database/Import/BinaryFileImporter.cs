using Microsoft.EntityFrameworkCore;
using Server.Database.PersistenceModels;
using Server.MirDatabase;
using Server.MirEnvir;
using Shared;

namespace Server.Database.Import;

public sealed class BinaryFileImportResult
{
    public int Accounts;
    public int Characters;
    public int Heroes;
    public int UserItems;
    public int Auctions;
    public int Mail;
    public int Guilds;
    public int Conquests;
    public int NpcUsedGoods;
}

public sealed class BinaryFileImporter
{
    private readonly string _connectionString;
    private readonly Action<string> _log;

    public BinaryFileImporter(string connectionString, Action<string> log = null)
    {
        _connectionString = connectionString;
        _log = log ?? (_ => { });
    }

    public BinaryFileImportResult ImportIfEmpty()
    {
        var options = new DbContextOptionsBuilder<Mir2DbContext>()
            .UseSqlite(_connectionString)
            .Options;

        using var db = new Mir2DbContext(options);

        db.Database.Migrate();

        if (db.DbMeta.Any())
        {
            _log("EF database already initialized; skipping file import.");
            return new BinaryFileImportResult();
        }

        return ImportInternal(db);
    }

    private BinaryFileImportResult ImportInternal(Mir2DbContext db)
    {
        _log("Loading binary files into memory...");

        // Load server DB lists (Server.MirDB)
        Envir.Main.LoadDB();

        // Load accounts/heroes/characters/auctions/gameshoplog (Server.MirADB)
        Envir.Main.LoadAccounts();

        // Guilds are separate files; load without requiring maps.
        var fileGuilds = ReadGuildFiles();

        // Conquests are separate files; load without requiring MapList to be built.
        var fileConquests = ReadConquestFiles();

        // NPC used goods (per-NPC .msd files)
        var npcUsedGoods = ReadNpcUsedGoods();

        _log("Projecting file state into persistence entities...");

        var result = new BinaryFileImportResult();

        // --- DbMeta ---
        var meta = new DbMetaEntity
        {
            Id = 1,
            Version = Envir.LoadVersion,
            CustomVersion = Envir.LoadCustomVersion,

            MapIndex = Envir.Main.MapIndex,
            ItemIndex = Envir.Main.ItemIndex,
            MonsterIndex = Envir.Main.MonsterIndex,
            NpcIndex = Envir.Main.NPCIndex,
            QuestIndex = Envir.Main.QuestIndex,
            GameshopIndex = Envir.Main.GameshopIndex,
            ConquestIndex = Envir.Main.ConquestIndex,
            RespawnIndex = Envir.Main.RespawnIndex,

            NextAccountId = Envir.Main.NextAccountID,
            NextCharacterId = Envir.Main.NextCharacterID,
            NextUserItemId = Envir.Main.NextUserItemID,
            NextHeroId = Envir.Main.NextHeroID,
            GuildCount = Envir.Main.GuildCount,
            NextGuildId = Envir.Main.NextGuildID,
            NextAuctionId = Envir.Main.NextAuctionID,
            NextMailId = Envir.Main.NextMailID,

            ConcurrencyToken = Guid.NewGuid().ToByteArray(),
        };

        // --- Static/server DB payload tables ---
        var mapInfos = Envir.Main.MapInfoList
            .Select(m => new MapInfoEntity { Index = m.Index, Data = ToBytes(w => m.Save(w)) })
            .ToList();
        var itemInfos = Envir.Main.ItemInfoList
            .Select(i => new ItemInfoEntity { Index = i.Index, Data = ToBytes(w => i.Save(w)) })
            .ToList();
        var monsterInfos = Envir.Main.MonsterInfoList
            .Select(m => new MonsterInfoEntity { Index = m.Index, Data = ToBytes(w => m.Save(w)) })
            .ToList();
        var npcInfos = Envir.Main.NPCInfoList
            .Select(n => new NpcInfoEntity { Index = n.Index, Data = ToBytes(w => n.Save(w)) })
            .ToList();
        var questInfos = Envir.Main.QuestInfoList
            .Select(q => new QuestInfoEntity { Index = q.Index, Data = ToBytes(w => q.Save(w)) })
            .ToList();
        var magicInfos = Envir.Main.MagicInfoList
            .Select(m => new MagicInfoEntity { Spell = (byte)m.Spell, Data = ToBytes(w => m.Save(w)) })
            .ToList();
        var gameshopItems = Envir.Main.GameShopList
            .Select(g => new GameShopItemEntity { GIndex = g.GIndex, Data = ToBytes(w => g.Save(w)) })
            .ToList();
        var conquestInfos = Envir.Main.ConquestInfoList
            .Select(c => new ConquestInfoEntity { Index = c.Index, Data = ToBytes(w => c.Save(w)) })
            .ToList();
        var gtMaps = Envir.Main.GTMapList
            .Select(g => new GtMapEntity { Index = g.Index, Data = ToBytes(w => g.Save(w)) })
            .ToList();

        var dragonState = new DragonInfoStateEntity
        {
            Id = 1,
            Data = ToBytes(w => Envir.Main.DragonInfo.Save(w)),
        };

        var respawnTimerState = new RespawnTimerStateEntity
        {
            Id = 1,
            Data = ToBytes(w => Envir.Main.RespawnTick.Save(w)),
        };

        // --- UserItems (collected globally) ---
        var userItemEntities = new Dictionary<ulong, UserItemEntity>();
        var userItemSlotEntities = new List<UserItemSlotEntity>();

        void collect(UserItem item)
        {
            if (item == null) return;

            if (!userItemEntities.ContainsKey(item.UniqueID))
            {
                userItemEntities[item.UniqueID] = new UserItemEntity
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

                userItemSlotEntities.Add(new UserItemSlotEntity
                {
                    ParentUserItemId = item.UniqueID,
                    SlotIndex = i,
                    ChildUserItemId = child.UniqueID,
                });
            }
        }

        // Collect items from accounts/characters
        foreach (var acc in Envir.Main.AccountList)
        {
            for (var i = 0; i < acc.Storage.Length; i++)
                collect(acc.Storage[i]);

            foreach (var ch in acc.Characters)
            {
                for (var i = 0; i < ch.Inventory.Length; i++)
                    collect(ch.Inventory[i]);

                for (var i = 0; i < ch.Equipment.Length; i++)
                    collect(ch.Equipment[i]);

                for (var i = 0; i < ch.QuestInventory.Length; i++)
                    collect(ch.QuestInventory[i]);

                collect(ch.CurrentRefine);

                foreach (var mail in ch.Mail)
                {
                    foreach (var mailItem in mail.Items)
                        collect(mailItem);
                }
            }
        }

        // Collect items from heroes
        foreach (var hero in Envir.Main.HeroList)
        {
            for (var i = 0; i < hero.Inventory.Length; i++)
                collect(hero.Inventory[i]);

            for (var i = 0; i < hero.Equipment.Length; i++)
                collect(hero.Equipment[i]);
        }

        // Collect items from auctions
        foreach (var auction in Envir.Main.Auctions)
            collect(auction.Item);

        // Collect items from guild storage
        foreach (var guild in fileGuilds)
        {
            for (var i = 0; i < guild.StoredItems.Length; i++)
            {
                if (guild.StoredItems[i] == null) continue;
                collect(guild.StoredItems[i].Item);
            }
        }

        // Collect items from NPC used goods
        foreach (var ug in npcUsedGoods)
            collect(ug.Item);

        // --- Accounts / Characters / Heroes ---
        var accountEntities = new List<AccountEntity>();
        var accountStorageEntities = new List<AccountStorageItemEntity>();

        var characterEntities = new List<CharacterEntity>();
        var charInvEntities = new List<CharacterInventoryItemEntity>();
        var charEquipEntities = new List<CharacterEquipmentItemEntity>();
        var charQuestInvEntities = new List<CharacterQuestInventoryItemEntity>();
        var charRefineEntities = new List<CharacterCurrentRefineItemEntity>();
        var charMagicEntities = new List<CharacterMagicEntity>();
        var charPetEntities = new List<CharacterPetEntity>();
        var charQuestEntities = new List<CharacterQuestProgressEntity>();
        var charBuffEntities = new List<CharacterBuffEntity>();
        var charIcEntities = new List<CharacterIntelligentCreatureEntity>();
        var charCompletedQuestEntities = new List<CharacterCompletedQuestEntity>();
        var charFriendEntities = new List<CharacterFriendEntity>();
        var charRentedEntities = new List<CharacterRentedItemEntity>();
        var charGsPurchaseEntities = new List<CharacterGsPurchaseEntity>();
        var charHeroSlotEntities = new List<CharacterHeroSlotEntity>();

        var heroEntities = new List<HeroEntity>();
        var heroInvEntities = new List<HeroInventoryItemEntity>();
        var heroEquipEntities = new List<HeroEquipmentItemEntity>();
        var heroMagicEntities = new List<HeroMagicEntity>();

        var mailEntities = new List<MailEntity>();
        var mailItemEntities = new List<MailItemEntity>();

        foreach (var acc in Envir.Main.AccountList)
        {
            accountEntities.Add(new AccountEntity
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

            for (var slot = 0; slot < acc.Storage.Length; slot++)
            {
                var item = acc.Storage[slot];
                if (item == null) continue;

                accountStorageEntities.Add(new AccountStorageItemEntity
                {
                    AccountIndex = acc.Index,
                    SlotIndex = slot,
                    UserItemId = item.UniqueID,
                });
            }

            foreach (var ch in acc.Characters)
            {
                var flags = new byte[ch.Flags.Length];
                for (var i = 0; i < ch.Flags.Length; i++)
                    flags[i] = ch.Flags[i] ? (byte)1 : (byte)0;

                characterEntities.Add(new CharacterEntity
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

                for (var slot = 0; slot < ch.Inventory.Length; slot++)
                {
                    var item = ch.Inventory[slot];
                    if (item == null) continue;
                    charInvEntities.Add(new CharacterInventoryItemEntity { CharacterIndex = ch.Index, SlotIndex = slot, UserItemId = item.UniqueID });
                }

                for (var slot = 0; slot < ch.Equipment.Length; slot++)
                {
                    var item = ch.Equipment[slot];
                    if (item == null) continue;
                    charEquipEntities.Add(new CharacterEquipmentItemEntity { CharacterIndex = ch.Index, SlotIndex = slot, UserItemId = item.UniqueID });
                }

                for (var slot = 0; slot < ch.QuestInventory.Length; slot++)
                {
                    var item = ch.QuestInventory[slot];
                    if (item == null) continue;
                    charQuestInvEntities.Add(new CharacterQuestInventoryItemEntity { CharacterIndex = ch.Index, SlotIndex = slot, UserItemId = item.UniqueID });
                }

                if (ch.CurrentRefine != null)
                {
                    charRefineEntities.Add(new CharacterCurrentRefineItemEntity { CharacterIndex = ch.Index, UserItemId = ch.CurrentRefine.UniqueID });
                }

                foreach (var m in ch.Magics)
                {
                    charMagicEntities.Add(new CharacterMagicEntity
                    {
                        CharacterIndex = ch.Index,
                        Spell = (byte)m.Spell,
                        Level = m.Level,
                        Key = m.Key,
                        Experience = m.Experience,
                        IsTempSpell = m.IsTempSpell,
                        CastTime = m.CastTime,
                    });
                }

                for (var i = 0; i < ch.Pets.Count; i++)
                {
                    var p = ch.Pets[i];
                    charPetEntities.Add(new CharacterPetEntity
                    {
                        CharacterIndex = ch.Index,
                        SlotIndex = i,
                        MonsterIndex = p.MonsterIndex,
                        Hp = p.HP,
                        Experience = p.Experience,
                        Level = p.Level,
                        MaxPetLevel = p.MaxPetLevel,
                    });
                }

                foreach (var q in ch.CurrentQuests)
                {
                    charQuestEntities.Add(new CharacterQuestProgressEntity
                    {
                        CharacterIndex = ch.Index,
                        QuestIndex = q.Index,
                        Data = ToBytes(w => q.Save(w)),
                    });
                }

                for (var i = 0; i < ch.Buffs.Count; i++)
                {
                    var b = ch.Buffs[i];
                    charBuffEntities.Add(new CharacterBuffEntity
                    {
                        CharacterIndex = ch.Index,
                        SlotIndex = i,
                        Data = ToBytes(w => b.Save(w)),
                    });
                }

                for (var i = 0; i < ch.IntelligentCreatures.Count; i++)
                {
                    var ic = ch.IntelligentCreatures[i];
                    charIcEntities.Add(new CharacterIntelligentCreatureEntity
                    {
                        CharacterIndex = ch.Index,
                        SlotIndex = i,
                        Data = ToBytes(w => ic.Save(w)),
                    });
                }

                foreach (var completed in ch.CompletedQuests)
                {
                    charCompletedQuestEntities.Add(new CharacterCompletedQuestEntity
                    {
                        CharacterIndex = ch.Index,
                        QuestIndex = completed,
                    });
                }

                foreach (var f in ch.Friends)
                {
                    if (f.Info == null) continue;
                    charFriendEntities.Add(new CharacterFriendEntity
                    {
                        CharacterIndex = ch.Index,
                        FriendIndex = f.Index,
                        Blocked = f.Blocked,
                        Memo = f.Memo ?? string.Empty,
                    });
                }

                for (var i = 0; i < ch.RentedItems.Count; i++)
                {
                    var r = ch.RentedItems[i];
                    charRentedEntities.Add(new CharacterRentedItemEntity
                    {
                        CharacterIndex = ch.Index,
                        SlotIndex = i,
                        Data = ToBytes(w => r.Save(w)),
                    });
                }

                foreach (var kvp in ch.GSpurchases)
                {
                    charGsPurchaseEntities.Add(new CharacterGsPurchaseEntity
                    {
                        CharacterIndex = ch.Index,
                        ItemKey = kvp.Key,
                        Count = kvp.Value,
                    });
                }

                // Mail (separate table + attachments)
                for (var mi = 0; mi < ch.Mail.Count; mi++)
                {
                    var m = ch.Mail[mi];

                    mailEntities.Add(new MailEntity
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

                    for (var si = 0; si < m.Items.Count; si++)
                    {
                        mailItemEntities.Add(new MailItemEntity
                        {
                            MailId = m.MailID,
                            SlotIndex = si,
                            UserItemId = m.Items[si].UniqueID,
                        });
                    }
                }

                // Hero slots (character -> hero indices)
                if (ch.Heroes != null)
                {
                    for (var hi = 0; hi < ch.Heroes.Length; hi++)
                    {
                        var h = ch.Heroes[hi];
                        if (h == null) continue;

                        charHeroSlotEntities.Add(new CharacterHeroSlotEntity
                        {
                            CharacterIndex = ch.Index,
                            SlotIndex = hi,
                            HeroIndex = h.Index,
                        });
                    }
                }
            }
        }

        foreach (var hero in Envir.Main.HeroList)
        {
            heroEntities.Add(new HeroEntity
            {
                Index = hero.Index,
                Name = hero.Name ?? string.Empty,
                Level = hero.Level,
                Class = (byte)hero.Class,
                Gender = (byte)hero.Gender,
                Hair = hero.Hair,
                CreationDate = hero.CreationDate,
                Deleted = hero.Deleted,
                DeleteDate = hero.DeleteDate,
                Hp = hero.HP,
                Mp = hero.MP,
                Experience = hero.Experience,
                InventorySize = hero.Inventory.Length,
                EquipmentSize = hero.Equipment.Length,
                AutoPot = hero.AutoPot,
                Grade = hero.Grade,
                HpItemIndex = hero.HPItemIndex,
                MpItemIndex = hero.MPItemIndex,
                AutoHpPercent = hero.AutoHPPercent,
                AutoMpPercent = hero.AutoMPPercent,
                SealCount = hero.SealCount,
            });

            for (var slot = 0; slot < hero.Inventory.Length; slot++)
            {
                var item = hero.Inventory[slot];
                if (item == null) continue;
                heroInvEntities.Add(new HeroInventoryItemEntity { HeroIndex = hero.Index, SlotIndex = slot, UserItemId = item.UniqueID });
            }

            for (var slot = 0; slot < hero.Equipment.Length; slot++)
            {
                var item = hero.Equipment[slot];
                if (item == null) continue;
                heroEquipEntities.Add(new HeroEquipmentItemEntity { HeroIndex = hero.Index, SlotIndex = slot, UserItemId = item.UniqueID });
            }

            foreach (var m in hero.Magics)
            {
                heroMagicEntities.Add(new HeroMagicEntity
                {
                    HeroIndex = hero.Index,
                    Spell = (byte)m.Spell,
                    Level = m.Level,
                    Key = m.Key,
                    Experience = m.Experience,
                    IsTempSpell = m.IsTempSpell,
                    CastTime = m.CastTime,
                });
            }
        }

        // --- Auctions ---
        var auctionEntities = Envir.Main.Auctions.Select(a => new AuctionEntity
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
        }).ToList();

        // --- Gameshop log ---
        var gameshopLogEntities = Envir.Main.GameshopLog.Select(kvp => new GameshopLogEntryEntity
        {
            ItemKey = kvp.Key,
            Count = kvp.Value,
        }).ToList();

        // --- Respawn saves (parse directly from Server.MirADB to avoid needing MapList) ---
        var respawnSaveEntities = ReadRespawnSavesFromMiradb()
            .Select(s => new RespawnSaveEntity
            {
                RespawnIndex = s.RespawnIndex,
                NextSpawnTick = (long)s.NextSpawnTick,
                Spawned = s.Spawned,
            })
            .ToList();

        // --- Guilds ---
        var guildEntities = new List<GuildEntity>();
        var guildRankEntities = new List<GuildRankEntity>();
        var guildMemberEntities = new List<GuildMemberEntity>();
        var guildNoticeEntities = new List<GuildNoticeEntity>();
        var guildBuffEntities = new List<GuildBuffEntity>();
        var guildStorageEntities = new List<GuildStorageItemEntity>();

        foreach (var g in fileGuilds)
        {
            guildEntities.Add(new GuildEntity
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

                guildRankEntities.Add(new GuildRankEntity
                {
                    GuildIndex = g.GuildIndex,
                    RankIndex = ri,
                    Name = r.Name ?? string.Empty,
                    Options = (byte)r.Options,
                });

                for (var mi2 = 0; mi2 < r.Members.Count; mi2++)
                {
                    var m = r.Members[mi2];
                    guildMemberEntities.Add(new GuildMemberEntity
                    {
                        GuildIndex = g.GuildIndex,
                        RankIndex = ri,
                        MemberIndex = mi2,
                        Name = m.Name ?? string.Empty,
                        Id = m.Id,
                        LastLogin = m.LastLogin,
                        HasVoted = m.hasvoted,
                        Online = m.Online,
                    });
                }
            }

            for (var ni = 0; ni < g.Notice.Count; ni++)
            {
                guildNoticeEntities.Add(new GuildNoticeEntity
                {
                    GuildIndex = g.GuildIndex,
                    LineIndex = ni,
                    Text = g.Notice[ni] ?? string.Empty,
                });
            }

            for (var bi = 0; bi < g.BuffList.Count; bi++)
            {
                var b = g.BuffList[bi];
                guildBuffEntities.Add(new GuildBuffEntity
                {
                    GuildIndex = g.GuildIndex,
                    BuffIndex = bi,
                    Id = b.Id,
                    Active = b.Active,
                    ActiveTimeRemaining = b.ActiveTimeRemaining,
                });
            }

            for (var si = 0; si < g.StoredItems.Length; si++)
            {
                if (g.StoredItems[si] == null) continue;
                guildStorageEntities.Add(new GuildStorageItemEntity
                {
                    GuildIndex = g.GuildIndex,
                    SlotIndex = si,
                    UserItemId = g.StoredItems[si].Item.UniqueID,
                    UserId = g.StoredItems[si].UserId,
                });
            }
        }

        // --- Conquests ---
        var conquestStateEntities = new List<ConquestStateEntity>();
        var conquestArcherEntities = new List<ConquestArcherStateEntity>();
        var conquestGateEntities = new List<ConquestGateStateEntity>();
        var conquestWallEntities = new List<ConquestWallStateEntity>();
        var conquestSiegeEntities = new List<ConquestSiegeStateEntity>();

        foreach (var cg in fileConquests)
        {
            conquestStateEntities.Add(new ConquestStateEntity
            {
                ConquestIndex = cg.Info.Index,
                Owner = cg.Owner,
                GoldStorage = cg.GoldStorage,
                AttackerId = cg.AttackerID,
                NpcRate = cg.NPCRate,
            });

            foreach (var a in cg.ArcherList)
            {
                conquestArcherEntities.Add(new ConquestArcherStateEntity
                {
                    ConquestIndex = cg.Info.Index,
                    Index = a.Index,
                    Alive = a.Alive,
                });
            }

            foreach (var g in cg.GateList)
            {
                conquestGateEntities.Add(new ConquestGateStateEntity
                {
                    ConquestIndex = cg.Info.Index,
                    Index = g.Index,
                    Health = g.Health,
                });
            }

            foreach (var w in cg.WallList)
            {
                conquestWallEntities.Add(new ConquestWallStateEntity
                {
                    ConquestIndex = cg.Info.Index,
                    Index = w.Index,
                    Health = w.Health,
                });
            }

            foreach (var s in cg.SiegeList)
            {
                conquestSiegeEntities.Add(new ConquestSiegeStateEntity
                {
                    ConquestIndex = cg.Info.Index,
                    Index = s.Index,
                    Health = s.Health,
                });
            }
        }

        // --- NPC used goods ---
        var npcUsedGoodEntities = new List<NpcUsedGoodEntity>();
        foreach (var ug in npcUsedGoods)
        {
            npcUsedGoodEntities.Add(new NpcUsedGoodEntity
            {
                NpcIndex = ug.NpcIndex,
                SlotIndex = ug.SlotIndex,
                UserItemId = ug.Item.UniqueID,
            });
        }

        // ---- Persist (FK-safe order) ----
        _log("Writing into EF database...");

        using var tx = db.Database.BeginTransaction();
        db.ChangeTracker.AutoDetectChangesEnabled = false;

        db.DbMeta.Add(meta);

        db.MapInfos.AddRange(mapInfos);
        db.ItemInfos.AddRange(itemInfos);
        db.MonsterInfos.AddRange(monsterInfos);
        db.NpcInfos.AddRange(npcInfos);
        db.QuestInfos.AddRange(questInfos);
        db.MagicInfos.AddRange(magicInfos);
        db.GameShopItems.AddRange(gameshopItems);
        db.ConquestInfos.AddRange(conquestInfos);
        db.GtMaps.AddRange(gtMaps);
        db.DragonInfoState.Add(dragonState);
        db.RespawnTimerState.Add(respawnTimerState);

        db.UserItems.AddRange(userItemEntities.Values);
        db.UserItemSlots.AddRange(userItemSlotEntities);

        db.Heroes.AddRange(heroEntities);
        db.HeroInventoryItems.AddRange(heroInvEntities);
        db.HeroEquipmentItems.AddRange(heroEquipEntities);
        db.HeroMagics.AddRange(heroMagicEntities);

        db.Accounts.AddRange(accountEntities);
        db.Characters.AddRange(characterEntities);

        db.AccountStorageItems.AddRange(accountStorageEntities);
        db.CharacterInventoryItems.AddRange(charInvEntities);
        db.CharacterEquipmentItems.AddRange(charEquipEntities);
        db.CharacterQuestInventoryItems.AddRange(charQuestInvEntities);
        db.CharacterCurrentRefineItems.AddRange(charRefineEntities);
        db.CharacterMagics.AddRange(charMagicEntities);
        db.CharacterPets.AddRange(charPetEntities);
        db.CharacterQuestProgress.AddRange(charQuestEntities);
        db.CharacterBuffs.AddRange(charBuffEntities);
        db.CharacterIntelligentCreatures.AddRange(charIcEntities);
        db.CharacterCompletedQuests.AddRange(charCompletedQuestEntities);
        db.CharacterFriends.AddRange(charFriendEntities);
        db.CharacterRentedItems.AddRange(charRentedEntities);
        db.CharacterGspurchases.AddRange(charGsPurchaseEntities);
        db.CharacterHeroSlots.AddRange(charHeroSlotEntities);

        db.Mail.AddRange(mailEntities);
        db.MailItems.AddRange(mailItemEntities);

        db.Auctions.AddRange(auctionEntities);
        db.GameshopLog.AddRange(gameshopLogEntities);
        db.RespawnSaves.AddRange(respawnSaveEntities);

        db.Guilds.AddRange(guildEntities);
        db.GuildRanks.AddRange(guildRankEntities);
        db.GuildMembers.AddRange(guildMemberEntities);
        db.GuildNotices.AddRange(guildNoticeEntities);
        db.GuildBuffs.AddRange(guildBuffEntities);
        db.GuildStorageItems.AddRange(guildStorageEntities);

        db.Conquests.AddRange(conquestStateEntities);
        db.ConquestArchers.AddRange(conquestArcherEntities);
        db.ConquestGates.AddRange(conquestGateEntities);
        db.ConquestWalls.AddRange(conquestWallEntities);
        db.ConquestSieges.AddRange(conquestSiegeEntities);

        db.NpcUsedGoods.AddRange(npcUsedGoodEntities);

        db.SaveChanges();
        tx.Commit();

        result.Accounts = accountEntities.Count;
        result.Characters = characterEntities.Count;
        result.Heroes = heroEntities.Count;
        result.UserItems = userItemEntities.Count;
        result.Auctions = auctionEntities.Count;
        result.Mail = mailEntities.Count;
        result.Guilds = guildEntities.Count;
        result.Conquests = conquestStateEntities.Count;
        result.NpcUsedGoods = npcUsedGoodEntities.Count;

        ValidateImport(db, result, Envir.Main.AccountList, Envir.Main.CharacterList);

        _log($"File import complete. Accounts={result.Accounts}, Characters={result.Characters}, Heroes={result.Heroes}, UserItems={result.UserItems}.");

        return result;
    }

    private void ValidateImport(Mir2DbContext db, BinaryFileImportResult expected, List<AccountInfo> fileAccounts, List<CharacterInfo> fileCharacters)
    {
        _log("Running post-import parity checks...");

        void check(string name, int expectedCount, int actualCount)
        {
            if (expectedCount != actualCount)
                _log($"[Parity] MISMATCH {name}: expected={expectedCount}, actual={actualCount}");
            else
                _log($"[Parity] OK {name}: {actualCount}");
        }

        check("Accounts", expected.Accounts, db.Accounts.Count());
        check("Characters", expected.Characters, db.Characters.Count());
        check("Heroes", expected.Heroes, db.Heroes.Count());
        check("UserItems", expected.UserItems, db.UserItems.Count());
        check("Auctions", expected.Auctions, db.Auctions.Count());
        check("Mail", expected.Mail, db.Mail.Count());
        check("Guilds", expected.Guilds, db.Guilds.Count());
        check("Conquests", expected.Conquests, db.Conquests.Count());
        check("NpcUsedGoods", expected.NpcUsedGoods, db.NpcUsedGoods.Count());

        // Spot checks
        if (fileAccounts.Count > 0)
        {
            var acc = fileAccounts[0];
            var fileStorageCount = acc.Storage.Count(x => x != null);
            var dbStorageCount = db.AccountStorageItems.Count(x => x.AccountIndex == acc.Index);
            check($"AccountStorageItems(AccountIndex={acc.Index})", fileStorageCount, dbStorageCount);
        }

        if (fileCharacters.Count > 0)
        {
            var ch = fileCharacters[0];
            var fileInvCount = ch.Inventory.Count(x => x != null);
            var dbInvCount = db.CharacterInventoryItems.Count(x => x.CharacterIndex == ch.Index);
            check($"CharacterInventoryItems(CharacterIndex={ch.Index})", fileInvCount, dbInvCount);
        }

        var firstMail = db.Mail.AsNoTracking().OrderBy(x => x.MailId).FirstOrDefault();
        if (firstMail != null)
        {
            var dbMailItemCount = db.MailItems.Count(x => x.MailId == firstMail.MailId);
            _log($"[Parity] Sample MailItems(MailId={firstMail.MailId}): {dbMailItemCount}");
        }
    }

    private sealed class NpcUsedGood
    {
        public int NpcIndex;
        public int SlotIndex;
        public UserItem Item;
    }

    private List<GuildInfo> ReadGuildFiles()
    {
        var list = new List<GuildInfo>();
        if (!Directory.Exists(Settings.GuildPath)) return list;

        // Guilds are indexed from 0..GuildCount-1 in legacy.
        for (var i = 0; i < Envir.Main.GuildCount; i++)
        {
            var path = Path.Combine(Settings.GuildPath, i + ".mgd");
            if (!File.Exists(path)) continue;

            using var stream = File.OpenRead(path);
            using var reader = new BinaryReader(stream);
            list.Add(new GuildInfo(reader));
        }

        return list;
    }

    private List<ConquestGuildInfo> ReadConquestFiles()
    {
        var list = new List<ConquestGuildInfo>();

        if (!Directory.Exists(Settings.ConquestsPath))
            Directory.CreateDirectory(Settings.ConquestsPath);

        foreach (var info in Envir.Main.ConquestInfoList)
        {
            var path = Path.Combine(Settings.ConquestsPath, info.Index + ".mcd");

            ConquestGuildInfo state;

            if (File.Exists(path))
            {
                using var stream = File.OpenRead(path);
                using var reader = new BinaryReader(stream);
                state = new ConquestGuildInfo(reader) { Info = info };
            }
            else
            {
                state = new ConquestGuildInfo { Info = info, NeedSave = true };
            }

            list.Add(state);
        }

        return list;
    }

    private List<NpcUsedGood> ReadNpcUsedGoods()
    {
        var list = new List<NpcUsedGood>();
        if (!Directory.Exists(Settings.GoodsPath)) return list;

        foreach (var path in Directory.GetFiles(Settings.GoodsPath, "*.msd"))
        {
            var npcIndexStr = Path.GetFileNameWithoutExtension(path);
            if (!int.TryParse(npcIndexStr, out var npcIndex)) continue;

            using var stream = File.OpenRead(path);
            using var reader = new BinaryReader(stream);

            int versionOrCount = reader.ReadInt32();
            int version = Envir.LoadVersion;
            int customVersion = Envir.LoadCustomVersion;
            int count = versionOrCount;

            if (versionOrCount == 9999)
            {
                version = reader.ReadInt32();
                customVersion = reader.ReadInt32();
                count = reader.ReadInt32();
            }

            for (var i = 0; i < count; i++)
            {
                var item = new UserItem(reader, version, customVersion);
                if (!Envir.Main.BindItem(item)) continue;

                list.Add(new NpcUsedGood
                {
                    NpcIndex = npcIndex,
                    SlotIndex = i,
                    Item = item,
                });
            }
        }

        return list;
    }

    private List<RespawnSave> ReadRespawnSavesFromMiradb()
    {
        var path = Envir.AccountPath;
        if (!File.Exists(path)) return new List<RespawnSave>();

        using var stream = File.OpenRead(path);
        using var reader = new BinaryReader(stream);

        // Mirror `LoadAccounts()` reading order (handles older versions too).
        var version = reader.ReadInt32();
        var customVersion = reader.ReadInt32();
        _ = reader.ReadInt32(); // NextAccountID
        _ = reader.ReadInt32(); // NextCharacterID
        _ = reader.ReadUInt64(); // NextUserItemID
        if (version > 98)
            _ = reader.ReadInt32(); // NextHeroID

        _ = reader.ReadInt32(); // GuildCount
        _ = reader.ReadInt32(); // NextGuildID

        if (version > 102)
        {
            var heroCount = reader.ReadInt32();
            for (var i = 0; i < heroCount; i++)
                _ = new HeroInfo(reader, version, customVersion);
        }

        var accountCount = reader.ReadInt32();
        for (var i = 0; i < accountCount; i++)
            _ = new AccountInfo(reader);

        _ = reader.ReadUInt64(); // NextAuctionID
        var auctionCount = reader.ReadInt32();
        for (var i = 0; i < auctionCount; i++)
            _ = new AuctionInfo(reader, version, customVersion);

        _ = reader.ReadUInt64(); // NextMailID

        if (version <= 80)
        {
            var mailCount = reader.ReadInt32();
            for (var i = 0; i < mailCount; i++)
                _ = new MailInfo(reader, version, customVersion);
        }

        if (version >= 63)
        {
            var logCount = reader.ReadInt32();
            for (var i = 0; i < logCount; i++)
            {
                _ = reader.ReadInt32();
                _ = reader.ReadInt32();
            }
        }

        var saves = new List<RespawnSave>();
        if (version >= 68)
        {
            var saveCount = reader.ReadInt32();
            saves = new List<RespawnSave>(saveCount);
            for (var i = 0; i < saveCount; i++)
                saves.Add(new RespawnSave(reader));
        }

        return saves;
    }

    private static byte[] ToBytes(Action<BinaryWriter> write)
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        write(writer);
        writer.Flush();
        return ms.ToArray();
    }
}

