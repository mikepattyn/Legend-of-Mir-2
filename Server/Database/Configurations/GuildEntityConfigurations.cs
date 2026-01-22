using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class GuildEntityConfiguration : IEntityTypeConfiguration<GuildEntity>
{
    public void Configure(EntityTypeBuilder<GuildEntity> builder)
    {
        builder.ToTable("Guilds");
        builder.HasKey(x => x.GuildIndex);
        builder.Property(x => x.GuildIndex).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();
    }
}

public sealed class GuildRankEntityConfiguration : IEntityTypeConfiguration<GuildRankEntity>
{
    public void Configure(EntityTypeBuilder<GuildRankEntity> builder)
    {
        builder.ToTable("GuildRanks");
        builder.HasKey(x => new { x.GuildIndex, x.RankIndex });

        builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

        builder.HasOne<GuildEntity>()
            .WithMany()
            .HasForeignKey(x => x.GuildIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class GuildMemberEntityConfiguration : IEntityTypeConfiguration<GuildMemberEntity>
{
    public void Configure(EntityTypeBuilder<GuildMemberEntity> builder)
    {
        builder.ToTable("GuildMembers");
        builder.HasKey(x => new { x.GuildIndex, x.RankIndex, x.MemberIndex });

        builder.Property(x => x.Name).HasMaxLength(32).IsRequired();

        builder.HasOne<GuildEntity>()
            .WithMany()
            .HasForeignKey(x => x.GuildIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.GuildIndex, x.Id });
    }
}

public sealed class GuildNoticeEntityConfiguration : IEntityTypeConfiguration<GuildNoticeEntity>
{
    public void Configure(EntityTypeBuilder<GuildNoticeEntity> builder)
    {
        builder.ToTable("GuildNotices");
        builder.HasKey(x => new { x.GuildIndex, x.LineIndex });

        builder.Property(x => x.Text).HasMaxLength(512).IsRequired();

        builder.HasOne<GuildEntity>()
            .WithMany()
            .HasForeignKey(x => x.GuildIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class GuildBuffEntityConfiguration : IEntityTypeConfiguration<GuildBuffEntity>
{
    public void Configure(EntityTypeBuilder<GuildBuffEntity> builder)
    {
        builder.ToTable("GuildBuffs");
        builder.HasKey(x => new { x.GuildIndex, x.BuffIndex });

        builder.HasOne<GuildEntity>()
            .WithMany()
            .HasForeignKey(x => x.GuildIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class GuildStorageItemEntityConfiguration : IEntityTypeConfiguration<GuildStorageItemEntity>
{
    public void Configure(EntityTypeBuilder<GuildStorageItemEntity> builder)
    {
        builder.ToTable("GuildStorageItems");
        builder.HasKey(x => new { x.GuildIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<GuildEntity>()
            .WithMany()
            .HasForeignKey(x => x.GuildIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

