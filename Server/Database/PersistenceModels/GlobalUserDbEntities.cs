namespace Server.Database.PersistenceModels;

public sealed class GameshopLogEntryEntity
{
    public int ItemKey { get; set; }
    public int Count { get; set; }
}

public sealed class RespawnSaveEntity
{
    public int RespawnIndex { get; set; }
    public long NextSpawnTick { get; set; }
    public bool Spawned { get; set; }
}

