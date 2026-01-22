namespace Server.Database.PersistenceModels;

public sealed class UserItemEntity
{
    public ulong UserItemId { get; set; }

    public int ItemIndex { get; set; }

    public ushort CurrentDura { get; set; }
    public ushort MaxDura { get; set; }

    public ushort Count { get; set; }
    public ushort GemCount { get; set; }

    public int SoulBoundId { get; set; }
    public bool Identified { get; set; }
    public bool Cursed { get; set; }

    public int WeddingRing { get; set; }

    public byte RefinedValue { get; set; }
    public byte RefineAdded { get; set; }
    public int RefineSuccessChance { get; set; }

    public int SlotsLength { get; set; }

    public byte[] AddedStatsData { get; set; } = Array.Empty<byte>();
    public byte[] AwakeData { get; set; } = Array.Empty<byte>();

    // Optional legacy sub-objects; empty array means "absent".
    public byte[] ExpireInfoData { get; set; } = Array.Empty<byte>();
    public byte[] RentalInformationData { get; set; } = Array.Empty<byte>();
    public byte[] SealedInfoData { get; set; } = Array.Empty<byte>();

    public bool IsShopItem { get; set; }
    public bool GmMade { get; set; }
}

public sealed class UserItemSlotEntity
{
    public ulong ParentUserItemId { get; set; }
    public int SlotIndex { get; set; }

    public ulong ChildUserItemId { get; set; }
}

