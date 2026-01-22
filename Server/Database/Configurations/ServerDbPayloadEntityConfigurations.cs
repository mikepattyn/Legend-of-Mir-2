using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

internal static class PayloadEntityConfig
{
    public static void ConfigurePayload<TEntity>(EntityTypeBuilder<TEntity> builder, string tableName)
        where TEntity : class
    {
        builder.ToTable(tableName);
        builder.Property("Data").IsRequired();
    }
}

public sealed class MapInfoEntityConfiguration : IEntityTypeConfiguration<MapInfoEntity>
{
    public void Configure(EntityTypeBuilder<MapInfoEntity> builder)
    {
        builder.ToTable("MapInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class ItemInfoEntityConfiguration : IEntityTypeConfiguration<ItemInfoEntity>
{
    public void Configure(EntityTypeBuilder<ItemInfoEntity> builder)
    {
        builder.ToTable("ItemInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class MonsterInfoEntityConfiguration : IEntityTypeConfiguration<MonsterInfoEntity>
{
    public void Configure(EntityTypeBuilder<MonsterInfoEntity> builder)
    {
        builder.ToTable("MonsterInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class NpcInfoEntityConfiguration : IEntityTypeConfiguration<NpcInfoEntity>
{
    public void Configure(EntityTypeBuilder<NpcInfoEntity> builder)
    {
        builder.ToTable("NpcInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class QuestInfoEntityConfiguration : IEntityTypeConfiguration<QuestInfoEntity>
{
    public void Configure(EntityTypeBuilder<QuestInfoEntity> builder)
    {
        builder.ToTable("QuestInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class MagicInfoEntityConfiguration : IEntityTypeConfiguration<MagicInfoEntity>
{
    public void Configure(EntityTypeBuilder<MagicInfoEntity> builder)
    {
        builder.ToTable("MagicInfos");
        builder.HasKey(x => x.Spell);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class GameShopItemEntityConfiguration : IEntityTypeConfiguration<GameShopItemEntity>
{
    public void Configure(EntityTypeBuilder<GameShopItemEntity> builder)
    {
        builder.ToTable("GameShopItems");
        builder.HasKey(x => x.GIndex);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class ConquestInfoEntityConfiguration : IEntityTypeConfiguration<ConquestInfoEntity>
{
    public void Configure(EntityTypeBuilder<ConquestInfoEntity> builder)
    {
        builder.ToTable("ConquestInfos");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class GtMapEntityConfiguration : IEntityTypeConfiguration<GtMapEntity>
{
    public void Configure(EntityTypeBuilder<GtMapEntity> builder)
    {
        builder.ToTable("GtMaps");
        builder.HasKey(x => x.Index);
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class DragonInfoStateEntityConfiguration : IEntityTypeConfiguration<DragonInfoStateEntity>
{
    public void Configure(EntityTypeBuilder<DragonInfoStateEntity> builder)
    {
        builder.ToTable("DragonInfoState");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Data).IsRequired();
    }
}

public sealed class RespawnTimerStateEntityConfiguration : IEntityTypeConfiguration<RespawnTimerStateEntity>
{
    public void Configure(EntityTypeBuilder<RespawnTimerStateEntity> builder)
    {
        builder.ToTable("RespawnTimerState");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Data).IsRequired();
    }
}

