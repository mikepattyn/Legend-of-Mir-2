using System.Drawing;
using Server.MirEnvir;

namespace Server.MirDatabase
{
    public class WaveSpawnInfo
    {
        public int Index;
        public string Name;
        public string Description = string.Empty;
        public int MapIndex;
        public int InstanceId = 1;
        public bool UseInstances = false;

        public List<WaveRound> Rounds = new List<WaveRound>();
        public List<WaveReward> Rewards = new List<WaveReward>();

        // Timer settings
        public int RoundDuration = 300; // seconds, 0 = no time limit
        public int SpawnDelay = 0; // delay between spawn patterns in milliseconds
        public int RoundStartDelay = 2000; // delay before starting next round in milliseconds
        public int CompletionCheckInterval = 5000; // how often to check for completion in milliseconds

        // Completion conditions
        public bool RequireAllMonstersKilled = true;
        public bool AutoAdvanceRounds = true;
        public bool AllowMultiplePlayers = true;

        public WaveSpawnInfo() { }

        public WaveSpawnInfo(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            Name = reader.ReadString();
            Description = reader.ReadString();
            MapIndex = reader.ReadInt32();
            InstanceId = reader.ReadInt32();
            UseInstances = reader.ReadBoolean();

            var counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                Rounds.Add(new WaveRound(reader));
            }

            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                Rewards.Add(new WaveReward(reader));
            }

            RoundDuration = reader.ReadInt32();
            SpawnDelay = reader.ReadInt32();
            RoundStartDelay = reader.ReadInt32();
            CompletionCheckInterval = reader.ReadInt32();
            RequireAllMonstersKilled = reader.ReadBoolean();
            AutoAdvanceRounds = reader.ReadBoolean();
            AllowMultiplePlayers = reader.ReadBoolean();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(Name);
            writer.Write(Description);
            writer.Write(MapIndex);
            writer.Write(InstanceId);
            writer.Write(UseInstances);

            writer.Write(Rounds.Count);
            for (int i = 0; i < Rounds.Count; i++)
            {
                Rounds[i].Save(writer);
            }

            writer.Write(Rewards.Count);
            for (int i = 0; i < Rewards.Count; i++)
            {
                Rewards[i].Save(writer);
            }

            writer.Write(RoundDuration);
            writer.Write(SpawnDelay);
            writer.Write(RoundStartDelay);
            writer.Write(CompletionCheckInterval);
            writer.Write(RequireAllMonstersKilled);
            writer.Write(AutoAdvanceRounds);
            writer.Write(AllowMultiplePlayers);
        }

        public override string ToString()
        {
            return string.Format("{0}- {1}", Index, Name);
        }
    }

    public class WaveRound
    {
        public int Index;
        public int RoundNumber;
        public string Name;
        public int MapIndex;
        public int InstanceId = 1;
        public List<SpawnPattern> SpawnPatterns = new List<SpawnPattern>();
        public List<Point> SpawnLocations = new List<Point>();
        public Point SpawnCenter = Point.Empty;
        public int SpawnRadius = 0; // 0 = use exact locations, >0 = area-based spawning

        public WaveRound() { }

        public WaveRound(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            RoundNumber = reader.ReadInt32();
            Name = reader.ReadString();
            MapIndex = reader.ReadInt32();
            InstanceId = reader.ReadInt32();

            var counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                SpawnPatterns.Add(new SpawnPattern(reader));
            }

            counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                SpawnLocations.Add(new Point(reader.ReadInt32(), reader.ReadInt32()));
            }

            SpawnCenter = new Point(reader.ReadInt32(), reader.ReadInt32());
            SpawnRadius = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(RoundNumber);
            writer.Write(Name);
            writer.Write(MapIndex);
            writer.Write(InstanceId);

            writer.Write(SpawnPatterns.Count);
            for (int i = 0; i < SpawnPatterns.Count; i++)
            {
                SpawnPatterns[i].Save(writer);
            }

            writer.Write(SpawnLocations.Count);
            for (int i = 0; i < SpawnLocations.Count; i++)
            {
                writer.Write(SpawnLocations[i].X);
                writer.Write(SpawnLocations[i].Y);
            }

            writer.Write(SpawnCenter.X);
            writer.Write(SpawnCenter.Y);
            writer.Write(SpawnRadius);
        }

        public override string ToString()
        {
            return string.Format("Round {0} - {1}", RoundNumber, Name);
        }
    }

    public class SpawnPattern
    {
        public int Index;
        public int MonsterIndex;
        public int Count = 1;
        public int SpawnDelay = 0; // delay before this pattern spawns in milliseconds
        public SpawnType Type = SpawnType.AllAtOnce;
        public int StaggerDelay = 0; // delay between individual spawns when using Staggered type

        public SpawnPattern() { }

        public SpawnPattern(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            MonsterIndex = reader.ReadInt32();
            Count = reader.ReadInt32();
            SpawnDelay = reader.ReadInt32();
            Type = (SpawnType)reader.ReadByte();
            StaggerDelay = reader.ReadInt32();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(MonsterIndex);
            writer.Write(Count);
            writer.Write(SpawnDelay);
            writer.Write((byte)Type);
            writer.Write(StaggerDelay);
        }

        public override string ToString()
        {
            return string.Format("Pattern {0}: {1}x Monster {2}", Index, Count, MonsterIndex);
        }
    }

    public enum SpawnType
    {
        AllAtOnce = 0,
        Staggered = 1
    }

    public class WaveReward
    {
        public int Index;
        public int RoundNumber; // 0 = completion reward
        public long Gold = 0;
        public long Experience = 0;
        public List<RewardItem> Items = new List<RewardItem>();

        public WaveReward() { }

        public WaveReward(BinaryReader reader)
        {
            Index = reader.ReadInt32();
            RoundNumber = reader.ReadInt32();
            Gold = reader.ReadInt64();
            Experience = reader.ReadInt64();

            var counter = reader.ReadInt32();
            for (int i = 0; i < counter; i++)
            {
                Items.Add(new RewardItem(reader));
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Index);
            writer.Write(RoundNumber);
            writer.Write(Gold);
            writer.Write(Experience);

            writer.Write(Items.Count);
            for (int i = 0; i < Items.Count; i++)
            {
                Items[i].Save(writer);
            }
        }

        public override string ToString()
        {
            if (RoundNumber == 0)
                return string.Format("Completion Reward: {0} Gold, {1} Exp", Gold, Experience);
            return string.Format("Round {0} Reward: {1} Gold, {2} Exp", RoundNumber, Gold, Experience);
        }
    }

    public class RewardItem
    {
        public int ItemIndex;
        public int Count = 1;
        public byte Durability = 0; // 0 = max durability

        public RewardItem() { }

        public RewardItem(BinaryReader reader)
        {
            ItemIndex = reader.ReadInt32();
            Count = reader.ReadInt32();
            Durability = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(ItemIndex);
            writer.Write(Count);
            writer.Write(Durability);
        }
    }
}
