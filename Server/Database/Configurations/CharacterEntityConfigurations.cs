using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Converters;
using Server.Database.PersistenceModels;

namespace Server.Database.Configurations;

public sealed class CharacterEntityConfiguration : IEntityTypeConfiguration<CharacterEntity>
{
    public void Configure(EntityTypeBuilder<CharacterEntity> builder)
    {
        builder.ToTable("Characters");

        builder.HasKey(x => x.Index);
        builder.Property(x => x.Index).ValueGeneratedNever();

        builder.HasOne<AccountEntity>()
            .WithMany()
            .HasForeignKey(x => x.AccountIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.Name);

        builder.Property(x => x.CreationIp).HasMaxLength(64).IsRequired();
        builder.Property(x => x.LastIp).HasMaxLength(64).IsRequired();
        builder.Property(x => x.BanReason).HasMaxLength(512).IsRequired();

        builder.Property(x => x.FlagsData).IsRequired();

        builder.Property(x => x.InventorySize).IsRequired();
        builder.Property(x => x.EquipmentSize).IsRequired();
        builder.Property(x => x.QuestInventorySize).IsRequired();
    }
}

public sealed class CharacterInventoryItemEntityConfiguration : IEntityTypeConfiguration<CharacterInventoryItemEntity>
{
    public void Configure(EntityTypeBuilder<CharacterInventoryItemEntity> builder)
    {
        builder.ToTable("CharacterInventoryItems");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class CharacterEquipmentItemEntityConfiguration : IEntityTypeConfiguration<CharacterEquipmentItemEntity>
{
    public void Configure(EntityTypeBuilder<CharacterEquipmentItemEntity> builder)
    {
        builder.ToTable("CharacterEquipmentItems");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class CharacterQuestInventoryItemEntityConfiguration : IEntityTypeConfiguration<CharacterQuestInventoryItemEntity>
{
    public void Configure(EntityTypeBuilder<CharacterQuestInventoryItemEntity> builder)
    {
        builder.ToTable("CharacterQuestInventoryItems");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class CharacterCurrentRefineItemEntityConfiguration : IEntityTypeConfiguration<CharacterCurrentRefineItemEntity>
{
    public void Configure(EntityTypeBuilder<CharacterCurrentRefineItemEntity> builder)
    {
        builder.ToTable("CharacterCurrentRefineItems");
        builder.HasKey(x => x.CharacterIndex);

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class CharacterMagicEntityConfiguration : IEntityTypeConfiguration<CharacterMagicEntity>
{
    public void Configure(EntityTypeBuilder<CharacterMagicEntity> builder)
    {
        builder.ToTable("CharacterMagics");
        builder.HasKey(x => new { x.CharacterIndex, x.Spell });

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterPetEntityConfiguration : IEntityTypeConfiguration<CharacterPetEntity>
{
    public void Configure(EntityTypeBuilder<CharacterPetEntity> builder)
    {
        builder.ToTable("CharacterPets");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterQuestProgressEntityConfiguration : IEntityTypeConfiguration<CharacterQuestProgressEntity>
{
    public void Configure(EntityTypeBuilder<CharacterQuestProgressEntity> builder)
    {
        builder.ToTable("CharacterQuestProgress");
        builder.HasKey(x => new { x.CharacterIndex, x.QuestIndex });
        builder.Property(x => x.Data).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterBuffEntityConfiguration : IEntityTypeConfiguration<CharacterBuffEntity>
{
    public void Configure(EntityTypeBuilder<CharacterBuffEntity> builder)
    {
        builder.ToTable("CharacterBuffs");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });
        builder.Property(x => x.Data).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterIntelligentCreatureEntityConfiguration : IEntityTypeConfiguration<CharacterIntelligentCreatureEntity>
{
    public void Configure(EntityTypeBuilder<CharacterIntelligentCreatureEntity> builder)
    {
        builder.ToTable("CharacterIntelligentCreatures");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });
        builder.Property(x => x.Data).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterCompletedQuestEntityConfiguration : IEntityTypeConfiguration<CharacterCompletedQuestEntity>
{
    public void Configure(EntityTypeBuilder<CharacterCompletedQuestEntity> builder)
    {
        builder.ToTable("CharacterCompletedQuests");
        builder.HasKey(x => new { x.CharacterIndex, x.QuestIndex });

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterFriendEntityConfiguration : IEntityTypeConfiguration<CharacterFriendEntity>
{
    public void Configure(EntityTypeBuilder<CharacterFriendEntity> builder)
    {
        builder.ToTable("CharacterFriends");
        builder.HasKey(x => new { x.CharacterIndex, x.FriendIndex });

        builder.Property(x => x.Memo).HasMaxLength(256).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterRentedItemEntityConfiguration : IEntityTypeConfiguration<CharacterRentedItemEntity>
{
    public void Configure(EntityTypeBuilder<CharacterRentedItemEntity> builder)
    {
        builder.ToTable("CharacterRentedItems");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });
        builder.Property(x => x.Data).IsRequired();

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterGsPurchaseEntityConfiguration : IEntityTypeConfiguration<CharacterGsPurchaseEntity>
{
    public void Configure(EntityTypeBuilder<CharacterGsPurchaseEntity> builder)
    {
        builder.ToTable("CharacterGspurchases");
        builder.HasKey(x => new { x.CharacterIndex, x.ItemKey });

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class HeroEntityConfiguration : IEntityTypeConfiguration<HeroEntity>
{
    public void Configure(EntityTypeBuilder<HeroEntity> builder)
    {
        builder.ToTable("Heroes");

        builder.HasKey(x => x.Index);
        builder.Property(x => x.Index).ValueGeneratedNever();

        builder.Property(x => x.Name).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => x.Name);
    }
}

public sealed class HeroInventoryItemEntityConfiguration : IEntityTypeConfiguration<HeroInventoryItemEntity>
{
    public void Configure(EntityTypeBuilder<HeroInventoryItemEntity> builder)
    {
        builder.ToTable("HeroInventoryItems");
        builder.HasKey(x => new { x.HeroIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<HeroEntity>()
            .WithMany()
            .HasForeignKey(x => x.HeroIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class HeroEquipmentItemEntityConfiguration : IEntityTypeConfiguration<HeroEquipmentItemEntity>
{
    public void Configure(EntityTypeBuilder<HeroEquipmentItemEntity> builder)
    {
        builder.ToTable("HeroEquipmentItems");
        builder.HasKey(x => new { x.HeroIndex, x.SlotIndex });

        builder.Property(x => x.UserItemId)
            .HasConversion(UlongToStringConverter.Instance)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne<HeroEntity>()
            .WithMany()
            .HasForeignKey(x => x.HeroIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<UserItemEntity>()
            .WithMany()
            .HasForeignKey(x => x.UserItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.UserItemId).IsUnique();
    }
}

public sealed class HeroMagicEntityConfiguration : IEntityTypeConfiguration<HeroMagicEntity>
{
    public void Configure(EntityTypeBuilder<HeroMagicEntity> builder)
    {
        builder.ToTable("HeroMagics");
        builder.HasKey(x => new { x.HeroIndex, x.Spell });

        builder.HasOne<HeroEntity>()
            .WithMany()
            .HasForeignKey(x => x.HeroIndex)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class CharacterHeroSlotEntityConfiguration : IEntityTypeConfiguration<CharacterHeroSlotEntity>
{
    public void Configure(EntityTypeBuilder<CharacterHeroSlotEntity> builder)
    {
        builder.ToTable("CharacterHeroSlots");
        builder.HasKey(x => new { x.CharacterIndex, x.SlotIndex });

        builder.HasOne<CharacterEntity>()
            .WithMany()
            .HasForeignKey(x => x.CharacterIndex)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<HeroEntity>()
            .WithMany()
            .HasForeignKey(x => x.HeroIndex)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.CharacterIndex, x.HeroIndex }).IsUnique();
    }
}

