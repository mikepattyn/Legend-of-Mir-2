using Microsoft.EntityFrameworkCore;
using Server.Database.PersistenceModels;

namespace Server.Database;

public sealed class Mir2DbContext : DbContext
{
    public Mir2DbContext(DbContextOptions<Mir2DbContext> options) : base(options)
    {
    }

    public DbSet<DbMetaEntity> DbMeta => Set<DbMetaEntity>();

    // Static/server DB payload tables (binary payload per row)
    public DbSet<MapInfoEntity> MapInfos => Set<MapInfoEntity>();
    public DbSet<ItemInfoEntity> ItemInfos => Set<ItemInfoEntity>();
    public DbSet<MonsterInfoEntity> MonsterInfos => Set<MonsterInfoEntity>();
    public DbSet<NpcInfoEntity> NpcInfos => Set<NpcInfoEntity>();
    public DbSet<QuestInfoEntity> QuestInfos => Set<QuestInfoEntity>();
    public DbSet<MagicInfoEntity> MagicInfos => Set<MagicInfoEntity>();
    public DbSet<GameShopItemEntity> GameShopItems => Set<GameShopItemEntity>();
    public DbSet<ConquestInfoEntity> ConquestInfos => Set<ConquestInfoEntity>();
    public DbSet<GtMapEntity> GtMaps => Set<GtMapEntity>();
    public DbSet<DragonInfoStateEntity> DragonInfoState => Set<DragonInfoStateEntity>();
    public DbSet<RespawnTimerStateEntity> RespawnTimerState => Set<RespawnTimerStateEntity>();

    // Core dynamic state
    public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
    public DbSet<CharacterEntity> Characters => Set<CharacterEntity>();
    public DbSet<HeroEntity> Heroes => Set<HeroEntity>();

    public DbSet<UserItemEntity> UserItems => Set<UserItemEntity>();
    public DbSet<UserItemSlotEntity> UserItemSlots => Set<UserItemSlotEntity>();

    public DbSet<AccountStorageItemEntity> AccountStorageItems => Set<AccountStorageItemEntity>();
    public DbSet<CharacterInventoryItemEntity> CharacterInventoryItems => Set<CharacterInventoryItemEntity>();
    public DbSet<CharacterEquipmentItemEntity> CharacterEquipmentItems => Set<CharacterEquipmentItemEntity>();
    public DbSet<CharacterQuestInventoryItemEntity> CharacterQuestInventoryItems => Set<CharacterQuestInventoryItemEntity>();
    public DbSet<CharacterCurrentRefineItemEntity> CharacterCurrentRefineItems => Set<CharacterCurrentRefineItemEntity>();
    public DbSet<CharacterMagicEntity> CharacterMagics => Set<CharacterMagicEntity>();
    public DbSet<CharacterPetEntity> CharacterPets => Set<CharacterPetEntity>();
    public DbSet<CharacterQuestProgressEntity> CharacterQuestProgress => Set<CharacterQuestProgressEntity>();
    public DbSet<CharacterBuffEntity> CharacterBuffs => Set<CharacterBuffEntity>();
    public DbSet<CharacterIntelligentCreatureEntity> CharacterIntelligentCreatures => Set<CharacterIntelligentCreatureEntity>();
    public DbSet<CharacterCompletedQuestEntity> CharacterCompletedQuests => Set<CharacterCompletedQuestEntity>();
    public DbSet<CharacterFriendEntity> CharacterFriends => Set<CharacterFriendEntity>();
    public DbSet<CharacterRentedItemEntity> CharacterRentedItems => Set<CharacterRentedItemEntity>();
    public DbSet<CharacterGsPurchaseEntity> CharacterGspurchases => Set<CharacterGsPurchaseEntity>();
    public DbSet<CharacterHeroSlotEntity> CharacterHeroSlots => Set<CharacterHeroSlotEntity>();

    public DbSet<HeroInventoryItemEntity> HeroInventoryItems => Set<HeroInventoryItemEntity>();
    public DbSet<HeroEquipmentItemEntity> HeroEquipmentItems => Set<HeroEquipmentItemEntity>();
    public DbSet<HeroMagicEntity> HeroMagics => Set<HeroMagicEntity>();

    public DbSet<AuctionEntity> Auctions => Set<AuctionEntity>();

    public DbSet<GameshopLogEntryEntity> GameshopLog => Set<GameshopLogEntryEntity>();
    public DbSet<RespawnSaveEntity> RespawnSaves => Set<RespawnSaveEntity>();

    public DbSet<MailEntity> Mail => Set<MailEntity>();
    public DbSet<MailItemEntity> MailItems => Set<MailItemEntity>();

    public DbSet<NpcUsedGoodEntity> NpcUsedGoods => Set<NpcUsedGoodEntity>();

    public DbSet<GuildEntity> Guilds => Set<GuildEntity>();
    public DbSet<GuildRankEntity> GuildRanks => Set<GuildRankEntity>();
    public DbSet<GuildMemberEntity> GuildMembers => Set<GuildMemberEntity>();
    public DbSet<GuildNoticeEntity> GuildNotices => Set<GuildNoticeEntity>();
    public DbSet<GuildBuffEntity> GuildBuffs => Set<GuildBuffEntity>();
    public DbSet<GuildStorageItemEntity> GuildStorageItems => Set<GuildStorageItemEntity>();

    public DbSet<ConquestStateEntity> Conquests => Set<ConquestStateEntity>();
    public DbSet<ConquestArcherStateEntity> ConquestArchers => Set<ConquestArcherStateEntity>();
    public DbSet<ConquestGateStateEntity> ConquestGates => Set<ConquestGateStateEntity>();
    public DbSet<ConquestWallStateEntity> ConquestWalls => Set<ConquestWallStateEntity>();
    public DbSet<ConquestSiegeStateEntity> ConquestSieges => Set<ConquestSiegeStateEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Mir2DbContext).Assembly);
    }
}

