using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class AccountEntityConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(x => x.Index);
        builder.Property(x => x.Index).ValueGeneratedNever();

        builder.Property(x => x.AccountId).HasMaxLength(64).IsRequired();
        builder.HasIndex(x => x.AccountId).IsUnique();

        builder.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
        builder.Property(x => x.Salt).IsRequired();

        builder.Property(x => x.UserName).HasMaxLength(128).IsRequired();
        builder.Property(x => x.SecretQuestion).HasMaxLength(256).IsRequired();
        builder.Property(x => x.SecretAnswer).HasMaxLength(256).IsRequired();
        builder.Property(x => x.EmailAddress).HasMaxLength(256).IsRequired();

        builder.Property(x => x.CreationIp).HasMaxLength(64).IsRequired();
        builder.Property(x => x.LastIp).HasMaxLength(64).IsRequired();
        builder.Property(x => x.BanReason).HasMaxLength(512).IsRequired();

        builder.Property(x => x.Gold).IsRequired();
        builder.Property(x => x.Credit).IsRequired();
        builder.Property(x => x.StorageSize).IsRequired();
    }
}

public sealed class AccountStorageItemEntityConfiguration : IEntityTypeConfiguration<AccountStorageItemEntity>
{
    public void Configure(EntityTypeBuilder<AccountStorageItemEntity> builder)
    {
        builder.ToTable("AccountStorageItems");

        builder.HasKey(x => new { x.AccountIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(x => x.AccountIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

