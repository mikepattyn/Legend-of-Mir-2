namespace Server.Database.PersistenceModels;

public sealed class GuildEntity
{
    public int GuildIndex { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Level { get; set; }       // byte in legacy
    public int SparePoints { get; set; } // byte in legacy

    public long Experience { get; set; }
    public long Gold { get; set; } // uint32 in legacy

    public int Votes { get; set; }
    public DateTime LastVoteAttempt { get; set; }
    public bool Voting { get; set; }

    public int FlagImage { get; set; }  // ushort in legacy
    public int FlagColourArgb { get; set; }

    public DateTime GtRent { get; set; }
    public DateTime GtBegin { get; set; }
    public int GtIndex { get; set; }
    public int GtKey { get; set; }
    public int GtPrice { get; set; }
}

public sealed class GuildRankEntity
{
    public int GuildIndex { get; set; }
    public int RankIndex { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Options { get; set; } // byte in legacy
}

public sealed class GuildMemberEntity
{
    public int GuildIndex { get; set; }
    public int RankIndex { get; set; }
    public int MemberIndex { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Id { get; set; }
    public DateTime LastLogin { get; set; }
    public bool HasVoted { get; set; }
    public bool Online { get; set; }
}

public sealed class GuildNoticeEntity
{
    public int GuildIndex { get; set; }
    public int LineIndex { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class GuildBuffEntity
{
    public int GuildIndex { get; set; }
    public int BuffIndex { get; set; }

    public int Id { get; set; }
    public bool Active { get; set; }
    public int ActiveTimeRemaining { get; set; }
}

public sealed class GuildStorageItemEntity
{
    public int GuildIndex { get; set; }
    public int SlotIndex { get; set; }

    public ulong UserItemId { get; set; }
    public long UserId { get; set; }
}

