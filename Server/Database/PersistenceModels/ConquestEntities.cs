namespace Server.Database.PersistenceModels;

public sealed class ConquestStateEntity
{
    public int ConquestIndex { get; set; }

    public int Owner { get; set; }
    public long GoldStorage { get; set; } // uint32 in legacy
    public int AttackerId { get; set; }
    public int NpcRate { get; set; } // byte in legacy
}

public sealed class ConquestSiegeStateEntity
{
    public int ConquestIndex { get; set; }
    public int Index { get; set; }
    public int Health { get; set; }
}

public sealed class ConquestWallStateEntity
{
    public int ConquestIndex { get; set; }
    public int Index { get; set; }
    public int Health { get; set; }
}

public sealed class ConquestGateStateEntity
{
    public int ConquestIndex { get; set; }
    public int Index { get; set; }
    public int Health { get; set; }
}

public sealed class ConquestArcherStateEntity
{
    public int ConquestIndex { get; set; }
    public int Index { get; set; }
    public bool Alive { get; set; }
}

