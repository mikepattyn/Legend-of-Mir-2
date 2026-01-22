using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class GameshopLogEntryEntityConfiguration : IEntityTypeConfiguration<GameshopLogEntryEntity>
{
    public void Configure(EntityTypeBuilder<GameshopLogEntryEntity> builder)
    {
        builder.ToTable("GameshopLog");
        builder.HasKey(x => x.ItemKey);
        builder.Property(x => x.ItemKey).ValueGeneratedNever();
    }
}

public sealed class RespawnSaveEntityConfiguration : IEntityTypeConfiguration<RespawnSaveEntity>
{
    public void Configure(EntityTypeBuilder<RespawnSaveEntity> builder)
    {
        builder.ToTable("RespawnSaves");
        builder.HasKey(x => x.RespawnIndex);
        builder.Property(x => x.RespawnIndex).ValueGeneratedNever();
        builder.HasIndex(x => x.NextSpawnTick);
    }
}

