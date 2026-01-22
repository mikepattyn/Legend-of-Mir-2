using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class MailEntityConfiguration : IEntityTypeConfiguration<MailEntity>
{
    public void Configure(EntityTypeBuilder<MailEntity> builder)
    {
        builder.ToTable("Mail");

        builder.HasKey(x => x.MailId);

        builder.Property(x => x.MailId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .ValueGeneratedNever();

        builder.Property(x => x.Sender).HasMaxLength(64).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(4000).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.RecipientIndex)
            .HasPrincipalKey(x => x.Index)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.RecipientIndex);
    }
}

public sealed class MailItemEntityConfiguration : IEntityTypeConfiguration<MailItemEntity>
{
    public void Configure(EntityTypeBuilder<MailItemEntity> builder)
    {
        builder.ToTable("MailItems");

        builder.HasKey(x => new { x.MailId, x.SlotIndex });

        builder.Property(x => x.MailId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20);

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<MailEntity>()
            .WithMany()
            .HasForeignKey(x => x.MailId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

