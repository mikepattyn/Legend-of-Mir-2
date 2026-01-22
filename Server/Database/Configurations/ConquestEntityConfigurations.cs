using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class ConquestStateEntityConfiguration : IEntityTypeConfiguration<ConquestStateEntity>
{
    public void Configure(EntityTypeBuilder<ConquestStateEntity> builder)
    {
        builder.ToTable("Conquests");
        builder.HasKey(x => x.ConquestIndex);
        builder.Property(x => x.ConquestIndex).ValueGeneratedNever();
        builder.HasIndex(x => x.Owner);
    }
}

public sealed class ConquestSiegeStateEntityConfiguration : IEntityTypeConfiguration<ConquestSiegeStateEntity>
{
    public void Configure(EntityTypeBuilder<ConquestSiegeStateEntity> builder)
    {
        builder.ToTable("ConquestSieges");
        builder.HasKey(x => new { x.ConquestIndex, x.Index });

        builder.HasOne<ConquestStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.ConquestIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ConquestWallStateEntityConfiguration : IEntityTypeConfiguration<ConquestWallStateEntity>
{
    public void Configure(EntityTypeBuilder<ConquestWallStateEntity> builder)
    {
        builder.ToTable("ConquestWalls");
        builder.HasKey(x => new { x.ConquestIndex, x.Index });

        builder.HasOne<ConquestStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.ConquestIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ConquestGateStateEntityConfiguration : IEntityTypeConfiguration<ConquestGateStateEntity>
{
    public void Configure(EntityTypeBuilder<ConquestGateStateEntity> builder)
    {
        builder.ToTable("ConquestGates");
        builder.HasKey(x => new { x.ConquestIndex, x.Index });

        builder.HasOne<ConquestStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.ConquestIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class ConquestArcherStateEntityConfiguration : IEntityTypeConfiguration<ConquestArcherStateEntity>
{
    public void Configure(EntityTypeBuilder<ConquestArcherStateEntity> builder)
    {
        builder.ToTable("ConquestArchers");
        builder.HasKey(x => new { x.ConquestIndex, x.Index });

        builder.HasOne<ConquestStateEntity>()
            .WithMany()
            .HasForeignKey(x => x.ConquestIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

