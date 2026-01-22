using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Library.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Salt = table.Column<byte[]>(type: "BLOB", nullable: false),
                    RequirePasswordChange = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SecretQuestion = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    SecretAnswer = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    EmailAddress = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CreationIp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Banned = table.Column<bool>(type: "INTEGER", nullable: false),
                    BanReason = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastIp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    LastDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasExpandedStorage = table.Column<bool>(type: "INTEGER", nullable: false),
                    ExpandedStorageExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Gold = table.Column<long>(type: "INTEGER", nullable: false),
                    Credit = table.Column<long>(type: "INTEGER", nullable: false),
                    StorageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    AdminAccount = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "ConquestInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Conquests",
                columns: table => new
                {
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Owner = table.Column<int>(type: "INTEGER", nullable: false),
                    GoldStorage = table.Column<long>(type: "INTEGER", nullable: false),
                    AttackerId = table.Column<int>(type: "INTEGER", nullable: false),
                    NpcRate = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conquests", x => x.ConquestIndex);
                });

            migrationBuilder.CreateTable(
                name: "DbMeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomVersion = table.Column<int>(type: "INTEGER", nullable: false),
                    MapIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    MonsterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    NpcIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    GameshopIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    RespawnIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    NextAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextCharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextUserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NextHeroId = table.Column<int>(type: "INTEGER", nullable: false),
                    GuildCount = table.Column<int>(type: "INTEGER", nullable: false),
                    NextGuildId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextAuctionId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NextMailId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ConcurrencyToken = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbMeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DragonInfoState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DragonInfoState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameShopItems",
                columns: table => new
                {
                    GIndex = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameShopItems", x => x.GIndex);
                });

            migrationBuilder.CreateTable(
                name: "GameshopLog",
                columns: table => new
                {
                    ItemKey = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameshopLog", x => x.ItemKey);
                });

            migrationBuilder.CreateTable(
                name: "GtMaps",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GtMaps", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    SparePoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    Gold = table.Column<long>(type: "INTEGER", nullable: false),
                    Votes = table.Column<int>(type: "INTEGER", nullable: false),
                    LastVoteAttempt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Voting = table.Column<bool>(type: "INTEGER", nullable: false),
                    FlagImage = table.Column<int>(type: "INTEGER", nullable: false),
                    FlagColourArgb = table.Column<int>(type: "INTEGER", nullable: false),
                    GtRent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GtBegin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    GtIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    GtKey = table.Column<int>(type: "INTEGER", nullable: false),
                    GtPrice = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildIndex);
                });

            migrationBuilder.CreateTable(
                name: "Heroes",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Class = table.Column<byte>(type: "INTEGER", nullable: false),
                    Gender = table.Column<byte>(type: "INTEGER", nullable: false),
                    Hair = table.Column<byte>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Hp = table.Column<int>(type: "INTEGER", nullable: false),
                    Mp = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    InventorySize = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentSize = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoPot = table.Column<bool>(type: "INTEGER", nullable: false),
                    Grade = table.Column<byte>(type: "INTEGER", nullable: false),
                    HpItemIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    MpItemIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    AutoHpPercent = table.Column<byte>(type: "INTEGER", nullable: false),
                    AutoMpPercent = table.Column<byte>(type: "INTEGER", nullable: false),
                    SealCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Heroes", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "ItemInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "MagicInfos",
                columns: table => new
                {
                    Spell = table.Column<byte>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MagicInfos", x => x.Spell);
                });

            migrationBuilder.CreateTable(
                name: "MapInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "MonsterInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "NpcInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "QuestInfos",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestInfos", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "RespawnSaves",
                columns: table => new
                {
                    RespawnIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    NextSpawnTick = table.Column<long>(type: "INTEGER", nullable: false),
                    Spawned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespawnSaves", x => x.RespawnIndex);
                });

            migrationBuilder.CreateTable(
                name: "RespawnTimerState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RespawnTimerState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ItemIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentDura = table.Column<ushort>(type: "INTEGER", nullable: false),
                    MaxDura = table.Column<ushort>(type: "INTEGER", nullable: false),
                    Count = table.Column<ushort>(type: "INTEGER", nullable: false),
                    GemCount = table.Column<ushort>(type: "INTEGER", nullable: false),
                    SoulBoundId = table.Column<int>(type: "INTEGER", nullable: false),
                    Identified = table.Column<bool>(type: "INTEGER", nullable: false),
                    Cursed = table.Column<bool>(type: "INTEGER", nullable: false),
                    WeddingRing = table.Column<int>(type: "INTEGER", nullable: false),
                    RefinedValue = table.Column<byte>(type: "INTEGER", nullable: false),
                    RefineAdded = table.Column<byte>(type: "INTEGER", nullable: false),
                    RefineSuccessChance = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotsLength = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedStatsData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    AwakeData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ExpireInfoData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    RentalInformationData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    SealedInfoData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    IsShopItem = table.Column<bool>(type: "INTEGER", nullable: false),
                    GmMade = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.UserItemId);
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Class = table.Column<byte>(type: "INTEGER", nullable: false),
                    Gender = table.Column<byte>(type: "INTEGER", nullable: false),
                    Hair = table.Column<byte>(type: "INTEGER", nullable: false),
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CreationIp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Banned = table.Column<bool>(type: "INTEGER", nullable: false),
                    BanReason = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastIp = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    LastLogoutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CurrentMapIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentLocationX = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentLocationY = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<byte>(type: "INTEGER", nullable: false),
                    BindMapIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    BindLocationX = table.Column<int>(type: "INTEGER", nullable: false),
                    BindLocationY = table.Column<int>(type: "INTEGER", nullable: false),
                    Hp = table.Column<int>(type: "INTEGER", nullable: false),
                    Mp = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    AttackMode = table.Column<byte>(type: "INTEGER", nullable: false),
                    PetMode = table.Column<byte>(type: "INTEGER", nullable: false),
                    AllowGroup = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowTrade = table.Column<bool>(type: "INTEGER", nullable: false),
                    AllowObserve = table.Column<bool>(type: "INTEGER", nullable: false),
                    PkPoints = table.Column<int>(type: "INTEGER", nullable: false),
                    Thrusting = table.Column<bool>(type: "INTEGER", nullable: false),
                    HalfMoon = table.Column<bool>(type: "INTEGER", nullable: false),
                    CrossHalfMoon = table.Column<bool>(type: "INTEGER", nullable: false),
                    DoubleSlash = table.Column<bool>(type: "INTEGER", nullable: false),
                    MentalState = table.Column<byte>(type: "INTEGER", nullable: false),
                    InventorySize = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipmentSize = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestInventorySize = table.Column<int>(type: "INTEGER", nullable: false),
                    FlagsData = table.Column<byte[]>(type: "BLOB", nullable: false),
                    PearlCount = table.Column<int>(type: "INTEGER", nullable: false),
                    RefineTimeRemaining = table.Column<long>(type: "INTEGER", nullable: false),
                    Married = table.Column<int>(type: "INTEGER", nullable: false),
                    MarriedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Mentor = table.Column<int>(type: "INTEGER", nullable: false),
                    MentorDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsMentor = table.Column<bool>(type: "INTEGER", nullable: false),
                    MentorExp = table.Column<long>(type: "INTEGER", nullable: false),
                    MaximumHeroCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentHeroIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroSpawned = table.Column<bool>(type: "INTEGER", nullable: false),
                    HeroBehaviour = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Index);
                    table.ForeignKey(
                        name: "FK_Characters_Accounts_AccountIndex",
                        column: x => x.AccountIndex,
                        principalTable: "Accounts",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConquestArchers",
                columns: table => new
                {
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Alive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestArchers", x => new { x.ConquestIndex, x.Index });
                    table.ForeignKey(
                        name: "FK_ConquestArchers_Conquests_ConquestIndex",
                        column: x => x.ConquestIndex,
                        principalTable: "Conquests",
                        principalColumn: "ConquestIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConquestGates",
                columns: table => new
                {
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestGates", x => new { x.ConquestIndex, x.Index });
                    table.ForeignKey(
                        name: "FK_ConquestGates_Conquests_ConquestIndex",
                        column: x => x.ConquestIndex,
                        principalTable: "Conquests",
                        principalColumn: "ConquestIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConquestSieges",
                columns: table => new
                {
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestSieges", x => new { x.ConquestIndex, x.Index });
                    table.ForeignKey(
                        name: "FK_ConquestSieges_Conquests_ConquestIndex",
                        column: x => x.ConquestIndex,
                        principalTable: "Conquests",
                        principalColumn: "ConquestIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConquestWalls",
                columns: table => new
                {
                    ConquestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<int>(type: "INTEGER", nullable: false),
                    Health = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConquestWalls", x => new { x.ConquestIndex, x.Index });
                    table.ForeignKey(
                        name: "FK_ConquestWalls_Conquests_ConquestIndex",
                        column: x => x.ConquestIndex,
                        principalTable: "Conquests",
                        principalColumn: "ConquestIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildBuffs",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    BuffIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveTimeRemaining = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildBuffs", x => new { x.GuildIndex, x.BuffIndex });
                    table.ForeignKey(
                        name: "FK_GuildBuffs_Guilds_GuildIndex",
                        column: x => x.GuildIndex,
                        principalTable: "Guilds",
                        principalColumn: "GuildIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildMembers",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    RankIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    MemberIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasVoted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Online = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildMembers", x => new { x.GuildIndex, x.RankIndex, x.MemberIndex });
                    table.ForeignKey(
                        name: "FK_GuildMembers_Guilds_GuildIndex",
                        column: x => x.GuildIndex,
                        principalTable: "Guilds",
                        principalColumn: "GuildIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildNotices",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    LineIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildNotices", x => new { x.GuildIndex, x.LineIndex });
                    table.ForeignKey(
                        name: "FK_GuildNotices_Guilds_GuildIndex",
                        column: x => x.GuildIndex,
                        principalTable: "Guilds",
                        principalColumn: "GuildIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuildRanks",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    RankIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Options = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildRanks", x => new { x.GuildIndex, x.RankIndex });
                    table.ForeignKey(
                        name: "FK_GuildRanks_Guilds_GuildIndex",
                        column: x => x.GuildIndex,
                        principalTable: "Guilds",
                        principalColumn: "GuildIndex",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeroMagics",
                columns: table => new
                {
                    HeroIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Spell = table.Column<byte>(type: "INTEGER", nullable: false),
                    Level = table.Column<byte>(type: "INTEGER", nullable: false),
                    Key = table.Column<byte>(type: "INTEGER", nullable: false),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTempSpell = table.Column<bool>(type: "INTEGER", nullable: false),
                    CastTime = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroMagics", x => new { x.HeroIndex, x.Spell });
                    table.ForeignKey(
                        name: "FK_HeroMagics_Heroes_HeroIndex",
                        column: x => x.HeroIndex,
                        principalTable: "Heroes",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountStorageItems",
                columns: table => new
                {
                    AccountIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStorageItems", x => new { x.AccountIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_AccountStorageItems_Accounts_AccountIndex",
                        column: x => x.AccountIndex,
                        principalTable: "Accounts",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountStorageItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GuildStorageItems",
                columns: table => new
                {
                    GuildIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuildStorageItems", x => new { x.GuildIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_GuildStorageItems_Guilds_GuildIndex",
                        column: x => x.GuildIndex,
                        principalTable: "Guilds",
                        principalColumn: "GuildIndex",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuildStorageItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HeroEquipmentItems",
                columns: table => new
                {
                    HeroIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroEquipmentItems", x => new { x.HeroIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_HeroEquipmentItems_Heroes_HeroIndex",
                        column: x => x.HeroIndex,
                        principalTable: "Heroes",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroEquipmentItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HeroInventoryItems",
                columns: table => new
                {
                    HeroIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroInventoryItems", x => new { x.HeroIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_HeroInventoryItems_Heroes_HeroIndex",
                        column: x => x.HeroIndex,
                        principalTable: "Heroes",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeroInventoryItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NpcUsedGoods",
                columns: table => new
                {
                    NpcIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NpcUsedGoods", x => new { x.NpcIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_NpcUsedGoods_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserItemSlots",
                columns: table => new
                {
                    ParentUserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ChildUserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItemSlots", x => new { x.ParentUserItemId, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_UserItemSlots_UserItems_ChildUserItemId",
                        column: x => x.ChildUserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserItemSlots_UserItems_ParentUserItemId",
                        column: x => x.ParentUserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ConsignmentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<long>(type: "INTEGER", nullable: false),
                    CurrentBid = table.Column<long>(type: "INTEGER", nullable: false),
                    SellerIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentBuyerIndex = table.Column<int>(type: "INTEGER", nullable: true),
                    Expired = table.Column<bool>(type: "INTEGER", nullable: false),
                    Sold = table.Column<bool>(type: "INTEGER", nullable: false),
                    ItemType = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionId);
                    table.ForeignKey(
                        name: "FK_Auctions_Characters_CurrentBuyerIndex",
                        column: x => x.CurrentBuyerIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_Characters_SellerIndex",
                        column: x => x.SellerIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auctions_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterBuffs",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterBuffs", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterBuffs_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCompletedQuests",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCompletedQuests", x => new { x.CharacterIndex, x.QuestIndex });
                    table.ForeignKey(
                        name: "FK_CharacterCompletedQuests_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterCurrentRefineItems",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterCurrentRefineItems", x => x.CharacterIndex);
                    table.ForeignKey(
                        name: "FK_CharacterCurrentRefineItems_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterCurrentRefineItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterEquipmentItems",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterEquipmentItems", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterEquipmentItems_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterEquipmentItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterFriends",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    FriendIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Blocked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Memo = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterFriends", x => new { x.CharacterIndex, x.FriendIndex });
                    table.ForeignKey(
                        name: "FK_CharacterFriends_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterGspurchases",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemKey = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterGspurchases", x => new { x.CharacterIndex, x.ItemKey });
                    table.ForeignKey(
                        name: "FK_CharacterGspurchases_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterHeroSlots",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    HeroIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterHeroSlots", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterHeroSlots_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterHeroSlots_Heroes_HeroIndex",
                        column: x => x.HeroIndex,
                        principalTable: "Heroes",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterIntelligentCreatures",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterIntelligentCreatures", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterIntelligentCreatures_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterInventoryItems",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterInventoryItems", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterInventoryItems_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterInventoryItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterMagics",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Spell = table.Column<byte>(type: "INTEGER", nullable: false),
                    Level = table.Column<byte>(type: "INTEGER", nullable: false),
                    Key = table.Column<byte>(type: "INTEGER", nullable: false),
                    Experience = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTempSpell = table.Column<bool>(type: "INTEGER", nullable: false),
                    CastTime = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMagics", x => new { x.CharacterIndex, x.Spell });
                    table.ForeignKey(
                        name: "FK_CharacterMagics_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterPets",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    MonsterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Hp = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxPetLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterPets", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterPets_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterQuestInventoryItems",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterQuestInventoryItems", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterQuestInventoryItems_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterQuestInventoryItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterQuestProgress",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    QuestIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterQuestProgress", x => new { x.CharacterIndex, x.QuestIndex });
                    table.ForeignKey(
                        name: "FK_CharacterQuestProgress_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterRentedItems",
                columns: table => new
                {
                    CharacterIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRentedItems", x => new { x.CharacterIndex, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_CharacterRentedItems_Characters_CharacterIndex",
                        column: x => x.CharacterIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mail",
                columns: table => new
                {
                    MailId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Sender = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    RecipientIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 4000, nullable: false),
                    Gold = table.Column<long>(type: "INTEGER", nullable: false),
                    DateSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOpened = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Locked = table.Column<bool>(type: "INTEGER", nullable: false),
                    Collected = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanReply = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mail", x => x.MailId);
                    table.ForeignKey(
                        name: "FK_Mail_Characters_RecipientIndex",
                        column: x => x.RecipientIndex,
                        principalTable: "Characters",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MailItems",
                columns: table => new
                {
                    MailId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SlotIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    UserItemId = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailItems", x => new { x.MailId, x.SlotIndex });
                    table.ForeignKey(
                        name: "FK_MailItems_Mail_MailId",
                        column: x => x.MailId,
                        principalTable: "Mail",
                        principalColumn: "MailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailItems_UserItems_UserItemId",
                        column: x => x.UserItemId,
                        principalTable: "UserItems",
                        principalColumn: "UserItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountId",
                table: "Accounts",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountStorageItems_UserItemId",
                table: "AccountStorageItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_CurrentBuyerIndex",
                table: "Auctions",
                column: "CurrentBuyerIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_SellerIndex",
                table: "Auctions",
                column: "SellerIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_UserItemId",
                table: "Auctions",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterCurrentRefineItems_UserItemId",
                table: "CharacterCurrentRefineItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterEquipmentItems_UserItemId",
                table: "CharacterEquipmentItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterHeroSlots_CharacterIndex_HeroIndex",
                table: "CharacterHeroSlots",
                columns: new[] { "CharacterIndex", "HeroIndex" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterHeroSlots_HeroIndex",
                table: "CharacterHeroSlots",
                column: "HeroIndex");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterInventoryItems_UserItemId",
                table: "CharacterInventoryItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterQuestInventoryItems_UserItemId",
                table: "CharacterQuestInventoryItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_AccountIndex",
                table: "Characters",
                column: "AccountIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_Name",
                table: "Characters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Conquests_Owner",
                table: "Conquests",
                column: "Owner");

            migrationBuilder.CreateIndex(
                name: "IX_GuildMembers_GuildIndex_Id",
                table: "GuildMembers",
                columns: new[] { "GuildIndex", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_Name",
                table: "Guilds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuildStorageItems_UserItemId",
                table: "GuildStorageItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeroEquipmentItems_UserItemId",
                table: "HeroEquipmentItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Heroes_Name",
                table: "Heroes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_HeroInventoryItems_UserItemId",
                table: "HeroInventoryItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mail_RecipientIndex",
                table: "Mail",
                column: "RecipientIndex");

            migrationBuilder.CreateIndex(
                name: "IX_MailItems_UserItemId",
                table: "MailItems",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NpcUsedGoods_NpcIndex",
                table: "NpcUsedGoods",
                column: "NpcIndex");

            migrationBuilder.CreateIndex(
                name: "IX_NpcUsedGoods_UserItemId",
                table: "NpcUsedGoods",
                column: "UserItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RespawnSaves_NextSpawnTick",
                table: "RespawnSaves",
                column: "NextSpawnTick");

            migrationBuilder.CreateIndex(
                name: "IX_UserItems_ItemIndex",
                table: "UserItems",
                column: "ItemIndex");

            migrationBuilder.CreateIndex(
                name: "IX_UserItemSlots_ChildUserItemId",
                table: "UserItemSlots",
                column: "ChildUserItemId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountStorageItems");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "CharacterBuffs");

            migrationBuilder.DropTable(
                name: "CharacterCompletedQuests");

            migrationBuilder.DropTable(
                name: "CharacterCurrentRefineItems");

            migrationBuilder.DropTable(
                name: "CharacterEquipmentItems");

            migrationBuilder.DropTable(
                name: "CharacterFriends");

            migrationBuilder.DropTable(
                name: "CharacterGspurchases");

            migrationBuilder.DropTable(
                name: "CharacterHeroSlots");

            migrationBuilder.DropTable(
                name: "CharacterIntelligentCreatures");

            migrationBuilder.DropTable(
                name: "CharacterInventoryItems");

            migrationBuilder.DropTable(
                name: "CharacterMagics");

            migrationBuilder.DropTable(
                name: "CharacterPets");

            migrationBuilder.DropTable(
                name: "CharacterQuestInventoryItems");

            migrationBuilder.DropTable(
                name: "CharacterQuestProgress");

            migrationBuilder.DropTable(
                name: "CharacterRentedItems");

            migrationBuilder.DropTable(
                name: "ConquestArchers");

            migrationBuilder.DropTable(
                name: "ConquestGates");

            migrationBuilder.DropTable(
                name: "ConquestInfos");

            migrationBuilder.DropTable(
                name: "ConquestSieges");

            migrationBuilder.DropTable(
                name: "ConquestWalls");

            migrationBuilder.DropTable(
                name: "DbMeta");

            migrationBuilder.DropTable(
                name: "DragonInfoState");

            migrationBuilder.DropTable(
                name: "GameShopItems");

            migrationBuilder.DropTable(
                name: "GameshopLog");

            migrationBuilder.DropTable(
                name: "GtMaps");

            migrationBuilder.DropTable(
                name: "GuildBuffs");

            migrationBuilder.DropTable(
                name: "GuildMembers");

            migrationBuilder.DropTable(
                name: "GuildNotices");

            migrationBuilder.DropTable(
                name: "GuildRanks");

            migrationBuilder.DropTable(
                name: "GuildStorageItems");

            migrationBuilder.DropTable(
                name: "HeroEquipmentItems");

            migrationBuilder.DropTable(
                name: "HeroInventoryItems");

            migrationBuilder.DropTable(
                name: "HeroMagics");

            migrationBuilder.DropTable(
                name: "ItemInfos");

            migrationBuilder.DropTable(
                name: "MagicInfos");

            migrationBuilder.DropTable(
                name: "MailItems");

            migrationBuilder.DropTable(
                name: "MapInfos");

            migrationBuilder.DropTable(
                name: "MonsterInfos");

            migrationBuilder.DropTable(
                name: "NpcInfos");

            migrationBuilder.DropTable(
                name: "NpcUsedGoods");

            migrationBuilder.DropTable(
                name: "QuestInfos");

            migrationBuilder.DropTable(
                name: "RespawnSaves");

            migrationBuilder.DropTable(
                name: "RespawnTimerState");

            migrationBuilder.DropTable(
                name: "UserItemSlots");

            migrationBuilder.DropTable(
                name: "Conquests");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Heroes");

            migrationBuilder.DropTable(
                name: "Mail");

            migrationBuilder.DropTable(
                name: "UserItems");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
