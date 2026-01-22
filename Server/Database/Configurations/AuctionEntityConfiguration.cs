using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class AuctionEntityConfiguration : IEntityTypeConfiguration<AuctionEntity>
{
    public void Configure(EntityTypeBuilder<AuctionEntity> builder)
    {
        builder.ToTable("Auctions");

        builder.HasKey(x => x.AuctionId);

        builder.Property(x => x.AuctionId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .ValueGeneratedNever();

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.SellerIndex)
            .HasPrincipalKey(x => x.Index)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CurrentBuyerIndex)
            .HasPrincipalKey(x => x.Index)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

