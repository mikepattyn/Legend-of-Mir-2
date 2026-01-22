namespace Server.Database.PersistenceModels;

public sealed class DbMetaEntity
{
    public int Id { get; set; } = 1;

    public int Version { get; set; }
    public int CustomVersion { get; set; }

    // Server DB counters
    public int MapIndex { get; set; }
    public int ItemIndex { get; set; }
    public int MonsterIndex { get; set; }
    public int NpcIndex { get; set; }
    public int QuestIndex { get; set; }
    public int GameshopIndex { get; set; }
    public int ConquestIndex { get; set; }
    public int RespawnIndex { get; set; }

    // User DB counters
    public int NextAccountId { get; set; }
    public int NextCharacterId { get; set; }
    public ulong NextUserItemId { get; set; }
    public int NextHeroId { get; set; }
    public int GuildCount { get; set; }
    public int NextGuildId { get; set; }
    public ulong NextAuctionId { get; set; }
    public ulong NextMailId { get; set; }

    // Used for safe ID allocation / optimistic concurrency.
    public byte[] ConcurrencyToken { get; set; } = Array.Empty<byte>();
}

