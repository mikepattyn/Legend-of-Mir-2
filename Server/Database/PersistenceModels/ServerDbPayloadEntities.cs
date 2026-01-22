namespace Server.Database.PersistenceModels;

public sealed class MapInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class ItemInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class MonsterInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class NpcInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class QuestInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class MagicInfoEntity
{
    public byte Spell { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class GameShopItemEntity
{
    public int GIndex { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class ConquestInfoEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class GtMapEntity
{
    public int Index { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class DragonInfoStateEntity
{
    public int Id { get; set; } = 1;
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public sealed class RespawnTimerStateEntity
{
    public int Id { get; set; } = 1;
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

