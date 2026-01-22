## Binary-file persistence contract (v116 / custom0)

This document captures the **effective persisted contract** of the current “rudimentary” MirDatabase persistence (binary files + per-guild/per-conquest files) as implemented today.

It is intended to be used as the **source of truth** for the EF Core **InitialCreate** migration and for the one-time file importer.

### Persistence entrypoints

- **Server DB**: `Envir.SaveDB()` / `Envir.LoadDB()`
  - File: `./Server.MirDB`
- **User DB**: `Envir.SaveAccounts()` / `Envir.LoadAccounts()`
  - File: `./Server.MirADB`
- **Guild DB**: `Envir.SaveGuilds()` / `Envir.LoadGuilds()`
  - Files: `./Guilds/{i}.mgd`
- **Conquest DB**: `Envir.SaveConquests()` / `Envir.LoadConquests()`
  - Files: `./Conquests/{ConquestInfo.Index}.mcd`

### Global versioning

- Each persisted stream begins with:
  - `Version` (int32)
  - `CustomVersion` (int32)

For the current codebase:
- `Envir.Version = 116`
- `Envir.CustomVersion = 0`

### Server.MirDB (static-ish server data)

Header counters (all `int32`):
- `MapIndex`
- `ItemIndex`
- `MonsterIndex`
- `NPCIndex`
- `QuestIndex`
- `GameshopIndex`
- `ConquestIndex`
- `RespawnIndex`

Then lists in order:
- `MapInfoList` (`MapInfo.Save(writer)`)
- `ItemInfoList` (`ItemInfo.Save(writer)`)
- `MonsterInfoList` (`MonsterInfo.Save(writer)`)
- `NPCInfoList` (`NPCInfo.Save(writer)`)
- `QuestInfoList` (`QuestInfo.Save(writer)`)
- `DragonInfo` (`DragonInfo.Save(writer)`)
- `MagicInfoList` (`MagicInfo.Save(writer)`)
- `GameShopList` (`GameShopItem.Save(writer)`)
- `ConquestInfoList` (`ConquestInfo.Save(writer)`)
- `RespawnTick` (`RespawnTimer.Save(writer)`)
- `GTMapList` (`GTMap.Save(writer)`)

### Server.MirADB (accounts + global counters + auctions + log/spawns)

Header counters:
- `NextAccountID` (int32)
- `NextCharacterID` (int32)
- `NextUserItemID` (uint64)
- `NextHeroID` (int32)
- `GuildCount` (int32)
- `NextGuildID` (int32)

Then:
- `HeroList` (count + `HeroInfo.Save(writer)`)
- `AccountList` (count + `AccountInfo.Save(writer)`; each account contains its characters)
- `NextAuctionID` (uint64)
- `Auctions` (count + `AuctionInfo.Save(writer)`)
- `NextMailID` (uint64)
- `GameshopLog` (count + pairs of `int32 key`, `int32 value`)
- `SavedSpawns` (count + `RespawnSave.Save(writer)`)

### AccountInfo (persisted fields)

Stored inside `Server.MirADB` via `AccountInfo.Save(writer)`:
- Identity & credentials: `Index`, `AccountID`, `Password` (hashed), `Salt`, `RequirePasswordChange`
- Profile: `UserName`, `BirthDate`, `SecretQuestion`, `SecretAnswer`, `EMailAddress`
- Audit: `CreationIP`, `CreationDate`, `Banned`, `BanReason`, `ExpiryDate`, `LastIP`, `LastDate`
- Characters: `Characters.Count` + `CharacterInfo.Save(writer)` per character
- Storage: `HasExpandedStorage`, `ExpandedStorageExpiryDate`, `Gold`, `Credit`
- Storage items: `Storage.Length` + per-slot presence flag + `UserItem.Save(writer)`
- Admin flag: `AdminAccount`

Not persisted (runtime only):
- `Connection`
- derived/linked collections (`Auctions` linkage is rebuilt from global auctions)

### CharacterInfo (persisted fields)

Stored inside `AccountInfo` via `CharacterInfo.Save(writer)`:
- Identity: `Index`, `Name`
- Basic: `Level`, `Class`, `Gender`, `Hair`
- Audit/ban: `CreationIP`, `CreationDate`, `Banned`, `BanReason`, `ExpiryDate`, `LastIP`, `LastLogoutDate`, `LastLoginDate`, `Deleted`, `DeleteDate`
- Location: `CurrentMapIndex`, `CurrentLocation`, `Direction`, `BindMapIndex`, `BindLocation`
- State: `HP`, `MP`, `Experience`, `AMode`, `PMode`, `PKPoints`
- Containers: `Inventory[]`, `Equipment[]`, `QuestInventory[]` (each saved as length + per-slot presence + `UserItem.Save(writer)`)
- Progress: `Magics`, `Pets`, `Flags[]`, `CurrentQuests`, `Buffs`, `Mail`, `IntelligentCreatures`, `PearlCount`, `CompletedQuests`
- Refining: optional `CurrentRefine` (`UserItem.Save(writer)`) + `RefineTimeRemaining`
- Social: `Friends`
- Rentals: `RentedItems`, `HasRentedItem`
- Relationships: `Married`, `MarriedDate`, `Mentor`, `MentorDate`, `IsMentor`, `MentorExp`
- Purchases: `GSpurchases` dictionary
- Heroes: `MaximumHeroCount`, hero indices list, `CurrentHeroIndex`, `HeroSpawned`, `HeroBehaviour`
- Guild link: `GuildIndex`, plus settings toggles `AllowTrade`, `AllowObserve`, `AllowGroup`

Notably not persisted:
- `Rank` array (explicit comment in code)
- runtime backrefs like `AccountInfo`, `Player`

### UserItem (persisted fields)

`UserItem` is saved by value wherever it appears (inventory/storage/mail/auction/guild storage), and includes a recursive slot graph (`Slots[]`).

Persisted by `UserItem.Save(writer)`:
- `UniqueID` (uint64)
- `ItemIndex` (int32)
- `CurrentDura`/`MaxDura` (uint16 each)
- `Count` (uint16)
- `SoulBoundId` (int32)
- Flags byte: `Identified`, `Cursed`
- `Slots.Length` (int32) + per-slot null-flag + nested `UserItem.Save(writer)` for non-null slots
- `GemCount` (uint16)
- `AddedStats` (`Stats.Save(writer)`)
- `Awake` (`Awake.Save(writer)`)
- Refining: `RefinedValue` (byte), `RefineAdded` (byte), `RefineSuccessChance` (int32)
- `WeddingRing` (int32)
- Optional sub-objects with presence flags:
  - `ExpireInfo`
  - `RentalInformation`
  - `IsShopItem` (bool)
  - `SealedInfo`
  - `GMMade` (bool)

Runtime-only binding:
- `UserItem.Info` is re-attached after load via `Envir.BindItem(UserItem)` using `ItemIndex` against `ItemInfoList`.

### Auctions (persisted fields)

Stored in `Server.MirADB` as a global list via `AuctionInfo.Save(writer)`:
- `AuctionID` (uint64)
- `Item` (embedded `UserItem`)
- `ConsignmentDate` (DateTime binary)
- `Price` (uint32)
- `SellerIndex` (int32)
- `Expired` (bool), `Sold` (bool)
- `ItemType` (byte)
- `CurrentBid` (uint32)
- `CurrentBuyerIndex` (int32)

Runtime-only binding:
- Seller/buyer `CharacterInfo` references are resolved after load.

### Mail (persisted fields)

Stored inside `CharacterInfo` via `MailInfo.Save(writer)`:
- `MailID` (uint64)
- `Sender` (string)
- `RecipientIndex` (int32)
- `Message` (string)
- `Gold` (uint32)
- `Items` (count + embedded `UserItem` list)
- `DateSent`, `DateOpened` (DateTime binary)
- `Locked`, `Collected`, `CanReply` (bools)

### Guilds (*.mgd)

Stored in separate files via `GuildInfo.Save(writer)`:
- Header marker `int.MaxValue`, then `Envir.Version`, `Envir.CustomVersion`
- `GuildIndex` (int32), `Name` (string), `Level` (byte), `SparePoints` (byte), `Experience` (int64), `Gold` (uint32)
- Voting: `Votes` (int32), `LastVoteAttempt` (DateTime binary), `Voting` (bool)
- Ranks: count + `GuildRank.Save(writer, true)` (includes members)
- Stored items: fixed length + per-slot presence + embedded `UserItem` + `UserId` (int64)
- Buffs: count + `GuildBuff.Save(writer)`
- Notice lines: count + strings
- Flag: `FlagImage` (uint16), `FlagColour` (ARGB int32)
- GT fields: `GTRent`, `GTIndex`, `GTKey`, `GTPrice`, `GTBegin`

### Conquests (*.mcd)

Stored in separate files via `ConquestGuildInfo.Save(writer)`:
- `Owner` (int32)
- Archer list (count + `ConquestGuildArcherInfo.Save(writer)`)
- Gate list (count + `ConquestGuildGateInfo.Save(writer)`)
- Wall list (count + `ConquestGuildWallInfo.Save(writer)`)
- Siege list (count + `ConquestGuildSiegeInfo.Save(writer)`)
- `GoldStorage` (uint32)
- `NPCRate` (byte)
- `AttackerID` (int32)

### NPC UsedGoods (*.msd)

Each NPC can persist its “used goods” list in a separate file written by `Envir.SaveGoods()` and read by `NPCScript.LoadGoods()`.

- Path: `./Envir/Goods/{NpcInfo.Index}.msd`
- Written as `*.msdn` and then atomically renamed to `*.msd`

Format:
- File marker: first int32 is either:
  - `9999` sentinel (newer format), or
  - count (older format)
- If sentinel `9999`:
  - `Version` (int32)
  - `CustomVersion` (int32)
  - `UsedGoodsCount` (int32)
- Then repeated `UsedGoodsCount` times:
  - embedded `UserItem` (`UserItem.Save(writer)`)

On load, each item is bound via `Envir.BindItem(item)` to re-attach `UserItem.Info`.

