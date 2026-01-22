using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class UserItemEntityConfiguration : IEntityTypeConfiguration<UserItemEntity>
{
    public void Configure(EntityTypeBuilder<UserItemEntity> builder)
    {
        builder.ToTable("UserItems");

        builder.HasKey(x => x.UserItemId);

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .ValueGeneratedNever();

        builder.Property(x => x.ItemIndex).IsRequired();

        builder.Property(x => x.AddedStatsData).IsRequired();
        builder.Property(x => x.AwakeData).IsRequired();

        builder.Property(x => x.ExpireInfoData).IsRequired();
        builder.Property(x => x.RentalInformationData).IsRequired();
        builder.Property(x => x.SealedInfoData).IsRequired();

        builder.HasIndex(x => x.ItemIndex);
    }
}

public sealed class UserItemSlotEntityConfiguration : IEntityTypeConfiguration<UserItemSlotEntity>
{
    public void Configure(EntityTypeBuilder<UserItemSlotEntity> builder)
    {
        builder.ToTable("UserItemSlots");

        builder.HasKey(x => new { x.ParentUserItemId, x.SlotIndex });

        builder.Property(x => x.ParentUserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20);

        builder.Property(x => x.ChildUserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.ParentUserItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.ChildUserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.ChildUserItemId).IsUnique();
    }
}

