using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class NpcUsedGoodEntityConfiguration : IEntityTypeConfiguration<NpcUsedGoodEntity>
{
    public void Configure(EntityTypeBuilder<NpcUsedGoodEntity> builder)
    {
        builder.ToTable("NpcUsedGoods");

        builder.HasKey(x => new { x.NpcIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.NpcIndex);
        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

