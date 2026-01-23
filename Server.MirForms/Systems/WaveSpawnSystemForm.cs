using Server.MirDatabase;
using Server.MirEnvir;
using Server.MirObjects;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Server.MirForms.Systems
{
    public partial class WaveSpawnSystemForm : Form
    {
        public Envir Envir => SMain.Envir;
        public Envir EditEnvir => SMain.EditEnvir;

        private WaveSpawnInfo selectedWaveSpawn;
        private WaveRound selectedRound;
        private SpawnPattern selectedPattern;
        private WaveReward selectedReward;
        private Timer refreshTimer;

        public WaveSpawnSystemForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Populate map combo boxes
            foreach (var map in EditEnvir.MapInfoList)
            {
                WaveMap_combo.Items.Add(map);
                RoundMap_combo.Items.Add(map);
            }

            // Populate monster combo boxes
            foreach (var monster in EditEnvir.MonsterInfoList)
            {
                PatternMonster_combo.Items.Add(monster);
            }

            // Populate spawn type combo
            SpawnType_combo.Items.AddRange(Enum.GetValues(typeof(SpawnType)).Cast<object>().ToArray());

            // Setup UseInstances checkbox event handler
            UseInstances_checkbox.CheckedChanged += UseInstances_checkbox_CheckedChanged;

            // Setup refresh timer for active waves
            refreshTimer = new Timer();
            refreshTimer.Interval = 1000; // 1 second
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();

            UpdateInterface();
        }

        private void UseInstances_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            // When UseInstances is checked, disable InstanceId textbox
            // Instances are now created automatically with unique IDs (Option B: StartTime + PlayerID + Random)
            InstanceId_textbox.Enabled = !UseInstances_checkbox.Checked;
            if (UseInstances_checkbox.Checked)
            {
                InstanceId_textbox.Text = "0"; // Set to 0, will be auto-generated
                InstanceId_textbox.BackColor = System.Drawing.SystemColors.Control; // Visual indication it's disabled
            }
            else
            {
                InstanceId_textbox.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateActiveWaves();
        }

        private void UpdateInterface()
        {
            UpdateWaveSpawnList();
            UpdateActiveWaves();
        }

        private void UpdateWaveSpawnList()
        {
            WaveSpawnListBox.Items.Clear();
            WaveConfigComboBox.Items.Clear();
            foreach (var wave in EditEnvir.WaveSpawnInfoList)
            {
                WaveSpawnListBox.Items.Add(wave);
                WaveConfigComboBox.Items.Add(wave);
            }
        }

        private void UpdateActiveWaves()
        {
            ActiveWavesListView.Items.Clear();
            foreach (var instance in Envir.WaveSpawnSystem.ActiveInstances)
            {
                if (instance == null) continue;

                var item = new ListViewItem(instance.Info.Name);
                item.SubItems.Add(instance.State.ToString());
                item.SubItems.Add(instance.CurrentRound.ToString());
                item.SubItems.Add(instance.Players.Count.ToString());
                item.SubItems.Add(instance.SpawnedMonsters.Count(x => !x.Dead).ToString());
                item.Tag = instance;
                ActiveWavesListView.Items.Add(item);
            }
        }

        private void WaveSpawnListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WaveSpawnListBox.SelectedItem == null)
            {
                selectedWaveSpawn = null;
                UpdateWaveSpawnDetails();
                return;
            }

            selectedWaveSpawn = (WaveSpawnInfo)WaveSpawnListBox.SelectedItem;
            UpdateWaveSpawnDetails();
        }

        private void UpdateWaveSpawnDetails()
        {
            if (selectedWaveSpawn == null)
            {
                WaveName_textbox.Text = string.Empty;
                WaveDescription_textbox.Text = string.Empty;
                WaveMap_combo.SelectedItem = null;
                InstanceId_textbox.Text = string.Empty;
                UseInstances_checkbox.Checked = false;
                RoundDuration_textbox.Text = string.Empty;
                SpawnDelay_textbox.Text = string.Empty;
                RoundStartDelay_textbox.Text = string.Empty;
                CompletionCheckInterval_textbox.Text = string.Empty;
                RequireAllMonstersKilled_checkbox.Checked = true;
                AutoAdvanceRounds_checkbox.Checked = true;
                AllowMultiplePlayers_checkbox.Checked = true;
                UpdateRoundsList();
                UpdateRewardsList();
                return;
            }

            WaveName_textbox.Text = selectedWaveSpawn.Name;
            WaveDescription_textbox.Text = selectedWaveSpawn.Description;
            WaveMap_combo.SelectedItem = EditEnvir.MapInfoList.FirstOrDefault(x => x.Index == selectedWaveSpawn.MapIndex);
            InstanceId_textbox.Text = selectedWaveSpawn.InstanceId.ToString();
            UseInstances_checkbox.Checked = selectedWaveSpawn.UseInstances;
            RoundDuration_textbox.Text = selectedWaveSpawn.RoundDuration.ToString();
            SpawnDelay_textbox.Text = selectedWaveSpawn.SpawnDelay.ToString();
            RoundStartDelay_textbox.Text = selectedWaveSpawn.RoundStartDelay.ToString();
            CompletionCheckInterval_textbox.Text = selectedWaveSpawn.CompletionCheckInterval.ToString();
            RequireAllMonstersKilled_checkbox.Checked = selectedWaveSpawn.RequireAllMonstersKilled;
            AutoAdvanceRounds_checkbox.Checked = selectedWaveSpawn.AutoAdvanceRounds;
            AllowMultiplePlayers_checkbox.Checked = selectedWaveSpawn.AllowMultiplePlayers;

            UpdateRoundsList();
            UpdateRewardsList();
        }

        private void UpdateRoundsList()
        {
            RoundsListBox.Items.Clear();
            if (selectedWaveSpawn == null) return;

            foreach (var round in selectedWaveSpawn.Rounds.OrderBy(x => x.RoundNumber))
            {
                RoundsListBox.Items.Add(round);
            }
        }

        private void UpdateRewardsList()
        {
            RewardsListBox.Items.Clear();
            if (selectedWaveSpawn == null) return;

            foreach (var reward in selectedWaveSpawn.Rewards)
            {
                RewardsListBox.Items.Add(reward);
            }
        }

        private void AddWaveButton_Click(object sender, EventArgs e)
        {
            var wave = new WaveSpawnInfo
            {
                Index = ++EditEnvir.WaveSpawnIndex,
                Name = "New Wave Spawn",
                Description = string.Empty,
                MapIndex = 0,
                InstanceId = 1,
                UseInstances = false,
                RoundDuration = 300,
                SpawnDelay = 0,
                RoundStartDelay = 2000,
                CompletionCheckInterval = 5000,
                RequireAllMonstersKilled = true,
                AutoAdvanceRounds = true,
                AllowMultiplePlayers = true
            };

            EditEnvir.WaveSpawnInfoList.Add(wave);
            UpdateWaveSpawnList();
            WaveSpawnListBox.SelectedItem = wave;
        }

        private void DeleteWaveButton_Click(object sender, EventArgs e)
        {
            if (selectedWaveSpawn == null) return;

            if (MessageBox.Show($"Delete {selectedWaveSpawn.Name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                EditEnvir.WaveSpawnInfoList.Remove(selectedWaveSpawn);
                selectedWaveSpawn = null;
                UpdateInterface();
            }
        }

        private void SaveWaveButton_Click(object sender, EventArgs e)
        {
            if (selectedWaveSpawn == null) return;

            try
            {
                selectedWaveSpawn.Name = WaveName_textbox.Text;
                selectedWaveSpawn.Description = WaveDescription_textbox.Text;
                if (WaveMap_combo.SelectedItem is MapInfo mapInfo)
                    selectedWaveSpawn.MapIndex = mapInfo.Index;
                if (int.TryParse(InstanceId_textbox.Text, out int instanceId))
                    selectedWaveSpawn.InstanceId = instanceId;
                selectedWaveSpawn.UseInstances = UseInstances_checkbox.Checked;
                if (int.TryParse(RoundDuration_textbox.Text, out int duration))
                    selectedWaveSpawn.RoundDuration = duration;
                if (int.TryParse(SpawnDelay_textbox.Text, out int spawnDelay))
                    selectedWaveSpawn.SpawnDelay = spawnDelay;
                if (int.TryParse(RoundStartDelay_textbox.Text, out int roundStartDelay))
                    selectedWaveSpawn.RoundStartDelay = roundStartDelay;
                if (int.TryParse(CompletionCheckInterval_textbox.Text, out int checkInterval))
                    selectedWaveSpawn.CompletionCheckInterval = checkInterval;
                selectedWaveSpawn.RequireAllMonstersKilled = RequireAllMonstersKilled_checkbox.Checked;
                selectedWaveSpawn.AutoAdvanceRounds = AutoAdvanceRounds_checkbox.Checked;
                selectedWaveSpawn.AllowMultiplePlayers = AllowMultiplePlayers_checkbox.Checked;

                UpdateWaveSpawnList();
                MessageBox.Show("Wave spawn saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving wave spawn: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoundsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoundsListBox.SelectedItem == null)
            {
                selectedRound = null;
                UpdateRoundDetails();
                return;
            }

            selectedRound = (WaveRound)RoundsListBox.SelectedItem;
            UpdateRoundDetails();
        }

        private void UpdateRoundDetails()
        {
            if (selectedRound == null)
            {
                RoundNumber_textbox.Text = string.Empty;
                RoundName_textbox.Text = string.Empty;
                RoundMap_combo.SelectedItem = null;
                RoundInstanceId_textbox.Text = string.Empty;
                SpawnCenterX_textbox.Text = string.Empty;
                SpawnCenterY_textbox.Text = string.Empty;
                SpawnRadius_textbox.Text = string.Empty;
                UpdatePatternsList();
                UpdateSpawnLocationsList();
                return;
            }

            RoundNumber_textbox.Text = selectedRound.RoundNumber.ToString();
            RoundName_textbox.Text = selectedRound.Name;
            RoundMap_combo.SelectedItem = EditEnvir.MapInfoList.FirstOrDefault(x => x.Index == selectedRound.MapIndex);
            RoundInstanceId_textbox.Text = selectedRound.InstanceId.ToString();
            SpawnCenterX_textbox.Text = selectedRound.SpawnCenter.X.ToString();
            SpawnCenterY_textbox.Text = selectedRound.SpawnCenter.Y.ToString();
            SpawnRadius_textbox.Text = selectedRound.SpawnRadius.ToString();

            UpdatePatternsList();
            UpdateSpawnLocationsList();
        }

        private void UpdatePatternsList()
        {
            PatternsListBox.Items.Clear();
            if (selectedRound == null) return;

            foreach (var pattern in selectedRound.SpawnPatterns)
            {
                PatternsListBox.Items.Add(pattern);
            }
        }

        private void UpdateSpawnLocationsList()
        {
            SpawnLocationsListBox.Items.Clear();
            if (selectedRound == null) return;

            foreach (var location in selectedRound.SpawnLocations)
            {
                SpawnLocationsListBox.Items.Add($"{location.X}, {location.Y}");
            }
        }

        private void AddRoundButton_Click(object sender, EventArgs e)
        {
            if (selectedWaveSpawn == null) return;

            var round = new WaveRound
            {
                Index = selectedWaveSpawn.Rounds.Count > 0 ? selectedWaveSpawn.Rounds.Max(x => x.Index) + 1 : 1,
                RoundNumber = selectedWaveSpawn.Rounds.Count > 0 ? selectedWaveSpawn.Rounds.Max(x => x.RoundNumber) + 1 : 1,
                Name = $"Round {selectedWaveSpawn.Rounds.Count + 1}",
                MapIndex = selectedWaveSpawn.MapIndex,
                InstanceId = selectedWaveSpawn.InstanceId,
                SpawnCenter = Point.Empty,
                SpawnRadius = 0
            };

            selectedWaveSpawn.Rounds.Add(round);
            UpdateRoundsList();
            RoundsListBox.SelectedItem = round;
        }

        private void DeleteRoundButton_Click(object sender, EventArgs e)
        {
            if (selectedRound == null || selectedWaveSpawn == null) return;

            if (MessageBox.Show($"Delete {selectedRound.Name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                selectedWaveSpawn.Rounds.Remove(selectedRound);
                selectedRound = null;
                UpdateRoundsList();
                UpdateRoundDetails();
            }
        }

        private void SaveRoundButton_Click(object sender, EventArgs e)
        {
            if (selectedRound == null) return;

            try
            {
                if (int.TryParse(RoundNumber_textbox.Text, out int roundNumber))
                    selectedRound.RoundNumber = roundNumber;
                selectedRound.Name = RoundName_textbox.Text;
                if (RoundMap_combo.SelectedItem is MapInfo mapInfo)
                    selectedRound.MapIndex = mapInfo.Index;
                if (int.TryParse(RoundInstanceId_textbox.Text, out int instanceId))
                    selectedRound.InstanceId = instanceId;
                if (int.TryParse(SpawnCenterX_textbox.Text, out int x) && int.TryParse(SpawnCenterY_textbox.Text, out int y))
                    selectedRound.SpawnCenter = new Point(x, y);
                if (int.TryParse(SpawnRadius_textbox.Text, out int radius))
                    selectedRound.SpawnRadius = radius;

                UpdateRoundsList();
                MessageBox.Show("Round saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving round: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PatternsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PatternsListBox.SelectedItem == null)
            {
                selectedPattern = null;
                UpdatePatternDetails();
                return;
            }

            selectedPattern = (SpawnPattern)PatternsListBox.SelectedItem;
            UpdatePatternDetails();
        }

        private void UpdatePatternDetails()
        {
            if (selectedPattern == null)
            {
                PatternMonster_combo.SelectedItem = null;
                PatternCount_textbox.Text = string.Empty;
                PatternSpawnDelay_textbox.Text = string.Empty;
                SpawnType_combo.SelectedItem = null;
                PatternStaggerDelay_textbox.Text = string.Empty;
                return;
            }

            PatternMonster_combo.SelectedItem = EditEnvir.MonsterInfoList.FirstOrDefault(x => x.Index == selectedPattern.MonsterIndex);
            PatternCount_textbox.Text = selectedPattern.Count.ToString();
            PatternSpawnDelay_textbox.Text = selectedPattern.SpawnDelay.ToString();
            SpawnType_combo.SelectedItem = selectedPattern.Type;
            PatternStaggerDelay_textbox.Text = selectedPattern.StaggerDelay.ToString();
        }

        private void AddPatternButton_Click(object sender, EventArgs e)
        {
            if (selectedRound == null) return;

            var pattern = new SpawnPattern
            {
                Index = selectedRound.SpawnPatterns.Count > 0 ? selectedRound.SpawnPatterns.Max(x => x.Index) + 1 : 1,
                MonsterIndex = 0,
                Count = 1,
                SpawnDelay = 0,
                Type = SpawnType.AllAtOnce,
                StaggerDelay = 0
            };

            selectedRound.SpawnPatterns.Add(pattern);
            UpdatePatternsList();
            PatternsListBox.SelectedItem = pattern;
        }

        private void DeletePatternButton_Click(object sender, EventArgs e)
        {
            if (selectedPattern == null || selectedRound == null) return;

            if (MessageBox.Show("Delete this spawn pattern?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                selectedRound.SpawnPatterns.Remove(selectedPattern);
                selectedPattern = null;
                UpdatePatternsList();
                UpdatePatternDetails();
            }
        }

        private void SavePatternButton_Click(object sender, EventArgs e)
        {
            if (selectedPattern == null) return;

            try
            {
                if (PatternMonster_combo.SelectedItem is MonsterInfo monsterInfo)
                    selectedPattern.MonsterIndex = monsterInfo.Index;
                if (int.TryParse(PatternCount_textbox.Text, out int count))
                    selectedPattern.Count = count;
                if (int.TryParse(PatternSpawnDelay_textbox.Text, out int spawnDelay))
                    selectedPattern.SpawnDelay = spawnDelay;
                if (SpawnType_combo.SelectedItem is SpawnType spawnType)
                    selectedPattern.Type = spawnType;
                if (int.TryParse(PatternStaggerDelay_textbox.Text, out int staggerDelay))
                    selectedPattern.StaggerDelay = staggerDelay;

                UpdatePatternsList();
                MessageBox.Show("Pattern saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving pattern: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSpawnLocationButton_Click(object sender, EventArgs e)
        {
            if (selectedRound == null) return;

            if (int.TryParse(SpawnLocationX_textbox.Text, out int x) && int.TryParse(SpawnLocationY_textbox.Text, out int y))
            {
                selectedRound.SpawnLocations.Add(new Point(x, y));
                UpdateSpawnLocationsList();
                SpawnLocationX_textbox.Text = string.Empty;
                SpawnLocationY_textbox.Text = string.Empty;
            }
        }

        private void DeleteSpawnLocationButton_Click(object sender, EventArgs e)
        {
            if (selectedRound == null || SpawnLocationsListBox.SelectedIndex < 0) return;

            selectedRound.SpawnLocations.RemoveAt(SpawnLocationsListBox.SelectedIndex);
            UpdateSpawnLocationsList();
        }

        private void RewardsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RewardsListBox.SelectedItem == null)
            {
                selectedReward = null;
                UpdateRewardDetails();
                return;
            }

            selectedReward = (WaveReward)RewardsListBox.SelectedItem;
            UpdateRewardDetails();
        }

        private void UpdateRewardDetails()
        {
            if (selectedReward == null)
            {
                RewardRoundNumber_textbox.Text = string.Empty;
                RewardGold_textbox.Text = string.Empty;
                RewardExp_textbox.Text = string.Empty;
                return;
            }

            RewardRoundNumber_textbox.Text = selectedReward.RoundNumber.ToString();
            RewardGold_textbox.Text = selectedReward.Gold.ToString();
            RewardExp_textbox.Text = selectedReward.Experience.ToString();
        }

        private void AddRewardButton_Click(object sender, EventArgs e)
        {
            if (selectedWaveSpawn == null) return;

            var reward = new WaveReward
            {
                Index = selectedWaveSpawn.Rewards.Count > 0 ? selectedWaveSpawn.Rewards.Max(x => x.Index) + 1 : 1,
                RoundNumber = 0,
                Gold = 0,
                Experience = 0
            };

            selectedWaveSpawn.Rewards.Add(reward);
            UpdateRewardsList();
            RewardsListBox.SelectedItem = reward;
        }

        private void DeleteRewardButton_Click(object sender, EventArgs e)
        {
            if (selectedReward == null || selectedWaveSpawn == null) return;

            if (MessageBox.Show("Delete this reward?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                selectedWaveSpawn.Rewards.Remove(selectedReward);
                selectedReward = null;
                UpdateRewardsList();
                UpdateRewardDetails();
            }
        }

        private void SaveRewardButton_Click(object sender, EventArgs e)
        {
            if (selectedReward == null) return;

            try
            {
                if (int.TryParse(RewardRoundNumber_textbox.Text, out int roundNumber))
                    selectedReward.RoundNumber = roundNumber;
                if (long.TryParse(RewardGold_textbox.Text, out long gold))
                    selectedReward.Gold = gold;
                if (long.TryParse(RewardExp_textbox.Text, out long exp))
                    selectedReward.Experience = exp;

                UpdateRewardsList();
                MessageBox.Show("Reward saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving reward: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartWaveButton_Click(object sender, EventArgs e)
        {
            if (WaveConfigComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a wave configuration from the dropdown.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the selected wave from EditEnvir to get its Index/Name
            var selectedWave = (WaveSpawnInfo)WaveConfigComboBox.SelectedItem;
            
            // Sync data from EditEnvir to Envir before starting to ensure latest changes are used
            if (EditEnvir.WaveSpawnInfoList != null)
            {
                Envir.WaveSpawnInfoList = new List<WaveSpawnInfo>(EditEnvir.WaveSpawnInfoList);
                Envir.WaveSpawnIndex = EditEnvir.WaveSpawnIndex;
            }
            
            // Look up the wave from the running server's Envir (not EditEnvir)
            // This ensures we use the data that's actually loaded in the running server
            var waveInfo = Envir.WaveSpawnInfoList.FirstOrDefault(x => x.Index == selectedWave.Index);
            if (waveInfo == null)
            {
                MessageBox.Show($"Wave '{selectedWave.Name}' not found in server database. Please ensure the database is saved and the wave exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate that the wave has a valid map configured
            if (waveInfo.MapIndex == 0)
            {
                MessageBox.Show($"Wave '{waveInfo.Name}' does not have a valid map configured. Please set a map in the wave configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate that the map exists
            var mapInfo = Envir.MapInfoList.FirstOrDefault(x => x.Index == waveInfo.MapIndex);
            if (mapInfo == null)
            {
                MessageBox.Show($"Wave '{waveInfo.Name}' references a map that doesn't exist (MapIndex: {waveInfo.MapIndex}). Please select a valid map.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var player = Envir.Players.FirstOrDefault();
            if (player == null)
            {
                MessageBox.Show("No players online to start wave.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Envir.WaveSpawnSystem.StartWave(waveInfo, player);
            UpdateActiveWaves();
        }

        private void StopWaveButton_Click(object sender, EventArgs e)
        {
            if (ActiveWavesListView.SelectedItems.Count == 0) return;

            var instance = (WaveSpawnInstance)ActiveWavesListView.SelectedItems[0].Tag;
            if (instance != null)
            {
                Envir.WaveSpawnSystem.StopWave(instance);
                UpdateActiveWaves();
            }
        }

        private void SaveDBButton_Click(object sender, EventArgs e)
        {
            // Sync WaveSpawnInfoList from EditEnvir to Main before saving
            // This ensures Main has the latest changes when explicitly saving
            if (EditEnvir.WaveSpawnInfoList != null)
            {
                Envir.WaveSpawnInfoList = new List<WaveSpawnInfo>(EditEnvir.WaveSpawnInfoList);
                Envir.WaveSpawnIndex = EditEnvir.WaveSpawnIndex;
            }

            // Save the Main Envir database (not EditEnvir) - this is what the running server uses
            Envir.SaveDB();
            MessageBox.Show("Database saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Sync and save data when form closes to ensure persistence
            if (EditEnvir.WaveSpawnInfoList != null)
            {
                Envir.WaveSpawnInfoList = new List<WaveSpawnInfo>(EditEnvir.WaveSpawnInfoList);
                Envir.WaveSpawnIndex = EditEnvir.WaveSpawnIndex;
                Envir.SaveDB();
            }

            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}
