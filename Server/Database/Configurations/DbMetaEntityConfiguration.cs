using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class DbMetaEntityConfiguration : IEntityTypeConfiguration<DbMetaEntity>
{
    public void Configure(EntityTypeBuilder<DbMetaEntity> builder)
    {
        builder.ToTable("DbMeta");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.ConcurrencyToken)
            .IsRowVersion()
            .IsRequired();

        // Store ulongs losslessly as TEXT.
        builder.Property(x => x.NextUserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.NextAuctionId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.NextMailId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

    }
}

