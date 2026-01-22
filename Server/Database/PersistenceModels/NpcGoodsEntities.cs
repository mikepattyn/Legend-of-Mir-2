namespace Server.Database.PersistenceModels;

public sealed class NpcUsedGoodEntity
{
    public int NpcIndex { get; set; }
    public int SlotIndex { get; set; }
    public ulong UserItemId { get; set; }
}

