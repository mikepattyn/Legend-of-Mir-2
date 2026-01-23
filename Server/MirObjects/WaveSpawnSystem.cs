using System.Drawing;
using System.Linq;
using Server.MirDatabase;
using Server.MirEnvir;
using S = ServerPackets;

namespace Server.MirObjects
{
    public class WaveSpawnSystem
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public List<WaveSpawnInstance> ActiveInstances = new List<WaveSpawnInstance>();

        public WaveSpawnSystem() { }

        public WaveSpawnInstance StartWave(WaveSpawnInfo info, PlayerObject player)
        {
            if (info == null) return null;

            // Check if player is already in a wave
            var existingInstance = ActiveInstances.FirstOrDefault(x => x.Players.Contains(player));
            if (existingInstance != null && !info.AllowMultiplePlayers)
            {
                return null; // Player already in a wave
            }

            var instance = new WaveSpawnInstance(info, player);
            ActiveInstances.Add(instance);
            return instance;
        }

        public void StopWave(WaveSpawnInstance instance)
        {
            if (instance == null) return;

            instance.Stop();
            ActiveInstances.Remove(instance);
        }

        public void Process()
        {
            for (int i = ActiveInstances.Count - 1; i >= 0; i--)
            {
                var instance = ActiveInstances[i];
                if (instance == null)
                {
                    ActiveInstances.RemoveAt(i);
                    continue;
                }

                instance.Process();

                if (instance.State == WaveState.Completed || instance.State == WaveState.Stopped)
                {
                    ActiveInstances.RemoveAt(i);
                }
            }
        }

        public WaveSpawnInstance GetInstance(PlayerObject player)
        {
            return ActiveInstances.FirstOrDefault(x => x.Players.Contains(player));
        }
    }

    public class WaveSpawnInstance
    {
        protected static Envir Envir
        {
            get { return Envir.Main; }
        }

        public WaveSpawnInfo Info;
        public WaveState State = WaveState.Waiting;
        public int CurrentRound = 0;
        public List<PlayerObject> Players = new List<PlayerObject>();
        public List<MonsterObject> SpawnedMonsters = new List<MonsterObject>();
        public Map CurrentMap;
        public long StartTime;
        public long RoundStartTime;
        public long NextSpawnTime;
        public long NextCheckTime;
        public int CurrentPatternIndex = 0;
        public int CurrentSpawnCount = 0;
        public WaveRound CurrentRoundInfo;
        public List<StaggeredSpawn> PendingSpawns = new List<StaggeredSpawn>();
        
        // Dynamic instance tracking
        public bool IsDynamicInstance = false;
        public int InstanceId = 0;
        private Map PreviousRoundMap = null; // Track previous round's map for cleanup

        public WaveSpawnInstance(WaveSpawnInfo info, PlayerObject player)
        {
            Info = info;
            Players.Add(player);
            StartTime = Envir.Time;
            State = WaveState.Starting;

            // Generate unique instance ID if using instances (Option B: StartTime + PlayerID + Random)
            if (Info.UseInstances)
            {
                var uniqueIdString = $"{StartTime}_{player.ObjectID}_{Envir.Random.Next(1000, 9999)}";
                InstanceId = uniqueIdString.GetHashCode();
                
                // Create map instance dynamically
                var mapInfo = Envir.MapInfoList.FirstOrDefault(x => x.Index == Info.MapIndex);
                if (mapInfo != null)
                {
                    CurrentMap = Envir.CreateMapInstance(mapInfo);
                    if (CurrentMap != null)
                    {
                        IsDynamicInstance = true;
                    }
                }
            }
            else
            {
                // Use existing map (non-instance mode)
                CurrentMap = Envir.GetMap(Info.MapIndex);
                InstanceId = Info.InstanceId; // Use config value for non-instance mode
            }

            if (CurrentMap == null)
            {
                State = WaveState.Error;
                return;
            }

            // Teleport player to map
            var spawnPoint = GetSpawnPoint();
            if (spawnPoint != Point.Empty)
            {
                player.Teleport(CurrentMap, spawnPoint);
            }

            // Start first round
            StartRound(1);
        }

        public void Process()
        {
            if (State == WaveState.Error || State == WaveState.Stopped || State == WaveState.Completed)
                return;

            // Check if round time expired
            if (Info.RoundDuration > 0 && Envir.Time >= RoundStartTime + (Info.RoundDuration * 1000))
            {
                if (Info.AutoAdvanceRounds)
                {
                    AdvanceRound();
                }
                else
                {
                    State = WaveState.WaitingForAdvance;
                }
                return;
            }

            // Process staggered spawns
            ProcessStaggeredSpawns();

            // Process spawning
            if (State == WaveState.Spawning && Envir.Time >= NextSpawnTime)
            {
                ProcessSpawn();
            }

            // Check for round completion
            if (State == WaveState.InProgress && Envir.Time >= NextCheckTime)
            {
                CheckRoundCompletion();
            }
        }

        private void StartRound(int roundNumber)
        {
            CurrentRound = roundNumber;
            CurrentRoundInfo = Info.Rounds.FirstOrDefault(x => x.RoundNumber == roundNumber);

            if (CurrentRoundInfo == null)
            {
                // No more rounds, wave completed
                CompleteWave();
                return;
            }

            State = WaveState.Spawning;
            RoundStartTime = Envir.Time;
            NextSpawnTime = Envir.Time + Info.RoundStartDelay;
            CurrentPatternIndex = 0;
            CurrentSpawnCount = 0;

            // Check if we need to change map
            bool needsNewMap = false;
            if (Info.UseInstances)
            {
                // For instances, check if round uses different map
                if (CurrentRoundInfo.MapIndex != Info.MapIndex)
                {
                    needsNewMap = true;
                }
                // If same map, reuse existing instance
            }
            else
            {
                // For non-instances, check if map index changed
                if (CurrentRoundInfo.MapIndex != Info.MapIndex)
                {
                    needsNewMap = true;
                }
            }

            if (needsNewMap)
            {
                // Clean up previous round's map if it was dynamically created
                if (PreviousRoundMap != null && PreviousRoundMap != CurrentMap)
                {
                    CleanupMapInstance(PreviousRoundMap);
                }

                Map newMap = null;
                if (Info.UseInstances)
                {
                    // Create new dynamic instance for this round
                    var mapInfo = Envir.MapInfoList.FirstOrDefault(x => x.Index == CurrentRoundInfo.MapIndex);
                    if (mapInfo != null)
                    {
                        newMap = Envir.CreateMapInstance(mapInfo);
                        if (newMap != null)
                        {
                            // Store previous map for cleanup
                            PreviousRoundMap = CurrentMap;
                            IsDynamicInstance = true;
                        }
                    }
                }
                else
                {
                    // Use existing map (non-instance mode)
                    newMap = Envir.GetMap(CurrentRoundInfo.MapIndex);
                }

                if (newMap != null)
                {
                    CurrentMap = newMap;
                    // Teleport players
                    var spawnPoint = GetSpawnPoint();
                    foreach (var player in Players)
                    {
                        if (player != null && !player.Dead)
                        {
                            player.Teleport(CurrentMap, spawnPoint);
                        }
                    }
                }
                else
                {
                    State = WaveState.Error;
                    return;
                }
            }
            else if (Info.UseInstances && CurrentRoundInfo.MapIndex == Info.MapIndex)
            {
                // Same map, reuse existing instance
                // No need to create new instance or teleport
            }

            // Send message to players
            BroadcastMessage($"Round {CurrentRound} started: {CurrentRoundInfo.Name}");
        }

        private void ProcessSpawn()
        {
            if (CurrentRoundInfo == null || CurrentPatternIndex >= CurrentRoundInfo.SpawnPatterns.Count)
            {
                // All patterns spawned, start the round
                State = WaveState.InProgress;
                NextCheckTime = Envir.Time + Info.CompletionCheckInterval;
                BroadcastMessage($"Round {CurrentRound} - All monsters spawned!");
                return;
            }

            var pattern = CurrentRoundInfo.SpawnPatterns[CurrentPatternIndex];

            // Check if it's time to spawn this pattern
            if (Envir.Time >= RoundStartTime + pattern.SpawnDelay)
            {
                SpawnPattern(pattern);
                CurrentPatternIndex++;
                NextSpawnTime = Envir.Time + Info.SpawnDelay;
            }
            else
            {
                NextSpawnTime = RoundStartTime + pattern.SpawnDelay;
            }
        }

        private void SpawnPattern(SpawnPattern pattern)
        {
            var monsterInfo = Envir.GetMonsterInfo(pattern.MonsterIndex);
            if (monsterInfo == null) return;

            var spawnLocations = GetSpawnLocations(pattern.Count);

            if (pattern.Type == SpawnType.AllAtOnce)
            {
                // Spawn all at once
                foreach (var location in spawnLocations)
                {
                    SpawnMonster(monsterInfo, location);
                }
            }
            else if (pattern.Type == SpawnType.Staggered)
            {
                // Spawn with stagger delay - schedule them
                for (int i = 0; i < spawnLocations.Count; i++)
                {
                    var delay = i * pattern.StaggerDelay;
                    var location = spawnLocations[i];
                    PendingSpawns.Add(new StaggeredSpawn
                    {
                        SpawnTime = Envir.Time + delay,
                        MonsterInfo = monsterInfo,
                        Location = location
                    });
                }
            }
        }

        private void SpawnMonster(MonsterInfo monsterInfo, Point location)
        {
            if (CurrentMap == null) return;

            var monster = MonsterObject.GetMonster(monsterInfo);
            if (monster == null) return;

            if (monster.Spawn(CurrentMap, location))
            {
                SpawnedMonsters.Add(monster);
                CurrentSpawnCount++;
            }
        }

        private List<Point> GetSpawnLocations(int count)
        {
            var locations = new List<Point>();

            if (CurrentMap == null || CurrentRoundInfo == null)
                return locations;

            if (CurrentRoundInfo.SpawnRadius > 0 && CurrentRoundInfo.SpawnCenter != Point.Empty)
            {
                // Area-based spawning
                var center = CurrentRoundInfo.SpawnCenter;
                var radius = CurrentRoundInfo.SpawnRadius;
                var walkableCells = CurrentMap.WalkableCells.Where(x =>
                    x.X >= center.X - radius && x.X <= center.X + radius &&
                    x.Y >= center.Y - radius && x.Y <= center.Y + radius).ToList();

                if (walkableCells.Count > 0)
                {
                    for (int i = 0; i < count && i < walkableCells.Count; i++)
                    {
                        var cell = walkableCells[Envir.Random.Next(walkableCells.Count)];
                        locations.Add(cell);
                        walkableCells.Remove(cell);
                    }
                }
            }
            else if (CurrentRoundInfo.SpawnLocations.Count > 0)
            {
                // Use specific spawn locations
                for (int i = 0; i < count; i++)
                {
                    var location = CurrentRoundInfo.SpawnLocations[Envir.Random.Next(CurrentRoundInfo.SpawnLocations.Count)];
                    locations.Add(location);
                }
            }
            else
            {
                // Fallback: use walkable cells near center or random
                var walkableCells = CurrentMap.WalkableCells;
                if (walkableCells.Count > 0)
                {
                    for (int i = 0; i < count && i < walkableCells.Count; i++)
                    {
                        var cell = walkableCells[Envir.Random.Next(walkableCells.Count)];
                        locations.Add(cell);
                    }
                }
            }

            return locations;
        }

        private Point GetSpawnPoint()
        {
            if (CurrentRoundInfo != null && CurrentRoundInfo.SpawnLocations.Count > 0)
            {
                return CurrentRoundInfo.SpawnLocations[0];
            }

            if (CurrentMap != null && CurrentMap.WalkableCells.Count > 0)
            {
                return CurrentMap.WalkableCells[Envir.Random.Next(CurrentMap.WalkableCells.Count)];
            }

            return new Point(50, 50); // Default fallback
        }

        private void CheckRoundCompletion()
        {
            NextCheckTime = Envir.Time + Info.CompletionCheckInterval;

            // Remove dead monsters from list
            SpawnedMonsters.RemoveAll(x => x == null || x.Dead);

            if (Info.RequireAllMonstersKilled)
            {
                if (SpawnedMonsters.Count == 0)
                {
                    // Round completed
                    CompleteRound();
                }
            }
        }

        private void CompleteRound()
        {
            // Give rewards for this round
            var roundReward = Info.Rewards.FirstOrDefault(x => x.RoundNumber == CurrentRound);
            if (roundReward != null)
            {
                DistributeReward(roundReward);
            }

            BroadcastMessage($"Round {CurrentRound} completed!");

            // Advance to next round
            if (Info.AutoAdvanceRounds)
            {
                AdvanceRound();
            }
            else
            {
                State = WaveState.WaitingForAdvance;
            }
        }

        private void AdvanceRound()
        {
            StartRound(CurrentRound + 1);
        }

        private void CompleteWave()
        {
            State = WaveState.Completed;

            // Give completion reward
            var completionReward = Info.Rewards.FirstOrDefault(x => x.RoundNumber == 0);
            if (completionReward != null)
            {
                DistributeReward(completionReward);
            }

            BroadcastMessage("Wave completed! Congratulations!");

            // Clean up
            Cleanup();
        }

        private void DistributeReward(WaveReward reward)
        {
            foreach (var player in Players)
            {
                if (player == null || player.Dead) continue;

                if (reward.Gold > 0)
                {
                    player.GainGold((uint)reward.Gold);
                }

                if (reward.Experience > 0)
                {
                    player.WinExp((uint)reward.Experience, 0);
                }

                foreach (var itemReward in reward.Items)
                {
                    var itemInfo = Envir.GetItemInfo(itemReward.ItemIndex);
                    if (itemInfo != null)
                    {
                        for (int i = 0; i < itemReward.Count; i++)
                        {
                            var item = Envir.CreateDropItem(itemInfo);
                            if (item != null)
                            {
                                if (itemReward.Durability > 0)
                                {
                                    item.CurrentDura = itemReward.Durability;
                                }
                                player.GainItem(item);
                            }
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            State = WaveState.Stopped;
            BroadcastMessage("Wave stopped by administrator.");
            Cleanup();
        }

        private void Cleanup()
        {
            // Remove all spawned monsters
            foreach (var monster in SpawnedMonsters.ToList())
            {
                if (monster != null && !monster.Dead)
                {
                    monster.Die();
                }
            }
            SpawnedMonsters.Clear();

            // Clean up dynamically created map instances
            if (IsDynamicInstance && CurrentMap != null)
            {
                CleanupMapInstance(CurrentMap);
            }

            // Clean up previous round's map if it was dynamically created
            if (PreviousRoundMap != null && PreviousRoundMap != CurrentMap)
            {
                CleanupMapInstance(PreviousRoundMap);
            }
        }

        private void CleanupMapInstance(Map map)
        {
            if (map == null) return;

            // Remove all players from the map (teleport them to safe location)
            foreach (var player in map.Players.ToList())
            {
                if (player != null)
                {
                    // Teleport player to a safe location (bind map or start point)
                    var safeMap = Envir.GetMap(player.BindMapIndex);
                    if (safeMap != null && safeMap.ValidPoint(player.BindLocation))
                    {
                        player.Teleport(safeMap, player.BindLocation);
                    }
                    else
                    {
                        // Fallback to first start point
                        if (Envir.StartPoints.Count > 0)
                        {
                            var startPoint = Envir.StartPoints[0];
                            var startMap = Envir.GetMap(startPoint.Info.Index);
                            if (startMap != null)
                            {
                                player.Teleport(startMap, startPoint.Location);
                            }
                        }
                    }
                }
            }

            // Note: Monsters are already cleaned up in Cleanup() via SpawnedMonsters list
            // Any remaining monsters on the map will be cleaned up when the map is removed
            // We don't need to iterate through all objects as we track our spawned monsters

            // Note: We don't remove NPCs as they're part of the map structure and will be cleaned up with the map

            // Remove map from Envir.MapList
            if (Envir.MapList.Contains(map))
            {
                Envir.MapList.Remove(map);
            }
        }

        private void ProcessStaggeredSpawns()
        {
            for (int i = PendingSpawns.Count - 1; i >= 0; i--)
            {
                var spawn = PendingSpawns[i];
                if (Envir.Time >= spawn.SpawnTime)
                {
                    SpawnMonster(spawn.MonsterInfo, spawn.Location);
                    PendingSpawns.RemoveAt(i);
                }
            }
        }

        private void BroadcastMessage(string message)
        {
            foreach (var player in Players)
            {
                if (player != null && !player.Dead)
                {
                    player.ReceiveChat(message, ChatType.System);
                }
            }
        }

        public void AddPlayer(PlayerObject player)
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        public void RemovePlayer(PlayerObject player)
        {
            Players.Remove(player);
        }
    }

    public enum WaveState
    {
        Waiting = 0,
        Starting = 1,
        Spawning = 2,
        InProgress = 3,
        WaitingForAdvance = 4,
        Completed = 5,
        Stopped = 6,
        Error = 7
    }

    public class StaggeredSpawn
    {
        public long SpawnTime;
        public MonsterInfo MonsterInfo;
        public Point Location;
    }
}
