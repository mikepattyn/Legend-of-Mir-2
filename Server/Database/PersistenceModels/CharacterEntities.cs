namespace Server.Database.PersistenceModels;

public sealed class CharacterEntity
{
    public int Index { get; set; }
    public int AccountIndex { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public byte Class { get; set; }
    public byte Gender { get; set; }
    public byte Hair { get; set; }

    public int GuildIndex { get; set; }

    public string CreationIp { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }

    public bool Banned { get; set; }
    public string BanReason { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }

    public string LastIp { get; set; } = string.Empty;
    public DateTime LastLogoutDate { get; set; }
    public DateTime LastLoginDate { get; set; }

    public bool Deleted { get; set; }
    public DateTime DeleteDate { get; set; }

    public int CurrentMapIndex { get; set; }
    public int CurrentLocationX { get; set; }
    public int CurrentLocationY { get; set; }
    public byte Direction { get; set; }

    public int BindMapIndex { get; set; }
    public int BindLocationX { get; set; }
    public int BindLocationY { get; set; }

    public int Hp { get; set; }
    public int Mp { get; set; }
    public long Experience { get; set; }

    public byte AttackMode { get; set; }
    public byte PetMode { get; set; }

    public bool AllowGroup { get; set; }
    public bool AllowTrade { get; set; }
    public bool AllowObserve { get; set; }

    public int PkPoints { get; set; }

    public bool Thrusting { get; set; }
    public bool HalfMoon { get; set; }
    public bool CrossHalfMoon { get; set; }
    public bool DoubleSlash { get; set; }
    public byte MentalState { get; set; }

    public int InventorySize { get; set; }
    public int EquipmentSize { get; set; }
    public int QuestInventorySize { get; set; }

    public byte[] FlagsData { get; set; } = Array.Empty<byte>();

    public int PearlCount { get; set; }

    public long RefineTimeRemaining { get; set; }

    public int Married { get; set; }
    public DateTime MarriedDate { get; set; }
    public int Mentor { get; set; }
    public DateTime MentorDate { get; set; }
    public bool IsMentor { get; set; }
    public long MentorExp { get; set; }

    public int MaximumHeroCount { get; set; }
    public int CurrentHeroIndex { get; set; }
    public bool HeroSpawned { get; set; }
    public byte HeroBehaviour { get; set; }
}

public sealed class CharacterInventoryItemEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class CharacterEquipmentItemEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class CharacterQuestInventoryItemEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class CharacterCurrentRefineItemEntity
{
    public int CharacterIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class CharacterMagicEntity
{
    public int CharacterIndex { get; set; }
    public byte Spell { get; set; }
    public byte Level { get; set; }
    public byte Key { get; set; }
    public int Experience { get; set; } // uint16 in legacy
    public bool IsTempSpell { get; set; }
    public long CastTime { get; set; }
}

public sealed class CharacterPetEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }

    public int MonsterIndex { get; set; }
    public int Hp { get; set; }
    public long Experience { get; set; } // uint32 in legacy
    public int Level { get; set; }
    public int MaxPetLevel { get; set; }
}

public sealed class CharacterQuestProgressEntity
{
    public int CharacterIndex { get; set; }
    public int QuestIndex { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class CharacterBuffEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class CharacterIntelligentCreatureEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class CharacterCompletedQuestEntity
{
    public int CharacterIndex { get; set; }
    public int QuestIndex { get; set; }
}

public sealed class CharacterFriendEntity
{
    public int CharacterIndex { get; set; }
    public int FriendIndex { get; set; }
    public bool Blocked { get; set; }
    public string Memo { get; set; } = string.Empty;
}

public sealed class CharacterRentedItemEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class CharacterGsPurchaseEntity
{
    public int CharacterIndex { get; set; }
    public int ItemKey { get; set; }
    public int Count { get; set; }
}

public sealed class HeroEntity
{
    public int Index { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public byte Class { get; set; }
    public byte Gender { get; set; }
    public byte Hair { get; set; }

    public DateTime CreationDate { get; set; }

    public bool Deleted { get; set; }
    public DateTime DeleteDate { get; set; }

    public int Hp { get; set; }
    public int Mp { get; set; }
    public long Experience { get; set; }

    public int InventorySize { get; set; }
    public int EquipmentSize { get; set; }

    public bool AutoPot { get; set; }
    public byte Grade { get; set; }
    public int HpItemIndex { get; set; }
    public int MpItemIndex { get; set; }
    public byte AutoHpPercent { get; set; }
    public byte AutoMpPercent { get; set; }
    public int SealCount { get; set; } // ushort in legacy
}

public sealed class HeroInventoryItemEntity
{
    public int HeroIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class HeroEquipmentItemEntity
{
    public int HeroIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

public sealed class HeroMagicEntity
{
    public int HeroIndex { get; set; }
    public byte Spell { get; set; }
    public byte Level { get; set; }
    public byte Key { get; set; }
    public int Experience { get; set; } // uint16 in legacy
    public bool IsTempSpell { get; set; }
    public long CastTime { get; set; }
}

public sealed class CharacterHeroSlotEntity
{
    public int CharacterIndex { get; set; }
    public int SlotIndex { get; set; }
    public int HeroIndex { get; set; }
}

