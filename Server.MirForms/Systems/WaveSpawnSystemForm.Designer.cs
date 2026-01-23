namespace Server.MirForms.Systems
{
    partial class WaveSpawnSystemForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.ActiveWavesTab = new System.Windows.Forms.TabPage();
            this.ActiveWavesListView = new System.Windows.Forms.ListView();
            this.StopWaveButton = new System.Windows.Forms.Button();
            this.StartWaveButton = new System.Windows.Forms.Button();
            this.WaveConfigComboBox = new System.Windows.Forms.ComboBox();
            this.WaveConfigTab = new System.Windows.Forms.TabPage();
            this.WaveSpawnListBox = new System.Windows.Forms.ListBox();
            this.WaveName_textbox = new System.Windows.Forms.TextBox();
            this.WaveDescription_textbox = new System.Windows.Forms.TextBox();
            this.WaveMap_combo = new System.Windows.Forms.ComboBox();
            this.InstanceId_textbox = new System.Windows.Forms.TextBox();
            this.UseInstances_checkbox = new System.Windows.Forms.CheckBox();
            this.RoundDuration_textbox = new System.Windows.Forms.TextBox();
            this.SpawnDelay_textbox = new System.Windows.Forms.TextBox();
            this.RoundStartDelay_textbox = new System.Windows.Forms.TextBox();
            this.CompletionCheckInterval_textbox = new System.Windows.Forms.TextBox();
            this.RequireAllMonstersKilled_checkbox = new System.Windows.Forms.CheckBox();
            this.AutoAdvanceRounds_checkbox = new System.Windows.Forms.CheckBox();
            this.AllowMultiplePlayers_checkbox = new System.Windows.Forms.CheckBox();
            this.AddWaveButton = new System.Windows.Forms.Button();
            this.DeleteWaveButton = new System.Windows.Forms.Button();
            this.SaveWaveButton = new System.Windows.Forms.Button();
            this.RoundsTab = new System.Windows.Forms.TabPage();
            this.RoundsListBox = new System.Windows.Forms.ListBox();
            this.RoundNumber_textbox = new System.Windows.Forms.TextBox();
            this.RoundName_textbox = new System.Windows.Forms.TextBox();
            this.RoundMap_combo = new System.Windows.Forms.ComboBox();
            this.RoundInstanceId_textbox = new System.Windows.Forms.TextBox();
            this.SpawnCenterX_textbox = new System.Windows.Forms.TextBox();
            this.SpawnCenterY_textbox = new System.Windows.Forms.TextBox();
            this.SpawnRadius_textbox = new System.Windows.Forms.TextBox();
            this.AddRoundButton = new System.Windows.Forms.Button();
            this.DeleteRoundButton = new System.Windows.Forms.Button();
            this.SaveRoundButton = new System.Windows.Forms.Button();
            this.PatternsListBox = new System.Windows.Forms.ListBox();
            this.PatternMonster_combo = new System.Windows.Forms.ComboBox();
            this.PatternCount_textbox = new System.Windows.Forms.TextBox();
            this.PatternSpawnDelay_textbox = new System.Windows.Forms.TextBox();
            this.SpawnType_combo = new System.Windows.Forms.ComboBox();
            this.PatternStaggerDelay_textbox = new System.Windows.Forms.TextBox();
            this.AddPatternButton = new System.Windows.Forms.Button();
            this.DeletePatternButton = new System.Windows.Forms.Button();
            this.SavePatternButton = new System.Windows.Forms.Button();
            this.SpawnLocationsListBox = new System.Windows.Forms.ListBox();
            this.SpawnLocationX_textbox = new System.Windows.Forms.TextBox();
            this.SpawnLocationY_textbox = new System.Windows.Forms.TextBox();
            this.AddSpawnLocationButton = new System.Windows.Forms.Button();
            this.DeleteSpawnLocationButton = new System.Windows.Forms.Button();
            this.RoundName_label = new System.Windows.Forms.Label();
            this.RoundMap_label = new System.Windows.Forms.Label();
            this.RoundInstanceId_label = new System.Windows.Forms.Label();
            this.SpawnCenterX_label = new System.Windows.Forms.Label();
            this.SpawnCenterY_label = new System.Windows.Forms.Label();
            this.SpawnRadius_label = new System.Windows.Forms.Label();
            this.PatternMonster_label = new System.Windows.Forms.Label();
            this.PatternCount_label = new System.Windows.Forms.Label();
            this.PatternSpawnDelay_label = new System.Windows.Forms.Label();
            this.SpawnType_label = new System.Windows.Forms.Label();
            this.PatternStaggerDelay_label = new System.Windows.Forms.Label();
            this.SpawnLocationX_label = new System.Windows.Forms.Label();
            this.SpawnLocationY_label = new System.Windows.Forms.Label();
            this.RoundNumber_label = new System.Windows.Forms.Label();
            this.SpawnModeInfo_label = new System.Windows.Forms.Label();
            this.WaveName_label = new System.Windows.Forms.Label();
            this.WaveDescription_label = new System.Windows.Forms.Label();
            this.WaveMap_label = new System.Windows.Forms.Label();
            this.InstanceId_label = new System.Windows.Forms.Label();
            this.RoundDuration_label = new System.Windows.Forms.Label();
            this.SpawnDelay_label = new System.Windows.Forms.Label();
            this.RoundStartDelay_label = new System.Windows.Forms.Label();
            this.CompletionCheckInterval_label = new System.Windows.Forms.Label();
            this.RewardsTab = new System.Windows.Forms.TabPage();
            this.RewardsListBox = new System.Windows.Forms.ListBox();
            this.RewardRoundNumber_textbox = new System.Windows.Forms.TextBox();
            this.RewardGold_textbox = new System.Windows.Forms.TextBox();
            this.RewardExp_textbox = new System.Windows.Forms.TextBox();
            this.AddRewardButton = new System.Windows.Forms.Button();
            this.DeleteRewardButton = new System.Windows.Forms.Button();
            this.SaveRewardButton = new System.Windows.Forms.Button();
            this.SaveDBButton = new System.Windows.Forms.Button();
            this.MainTabControl.SuspendLayout();
            this.ActiveWavesTab.SuspendLayout();
            this.WaveConfigTab.SuspendLayout();
            this.RoundsTab.SuspendLayout();
            this.RewardsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.ActiveWavesTab);
            this.MainTabControl.Controls.Add(this.WaveConfigTab);
            this.MainTabControl.Controls.Add(this.RoundsTab);
            this.MainTabControl.Controls.Add(this.RewardsTab);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(1000, 600);
            this.MainTabControl.TabIndex = 0;
            // 
            // ActiveWavesTab
            // 
            this.ActiveWavesTab.Controls.Add(this.ActiveWavesListView);
            this.ActiveWavesTab.Controls.Add(this.StopWaveButton);
            this.ActiveWavesTab.Controls.Add(this.StartWaveButton);
            this.ActiveWavesTab.Controls.Add(this.WaveConfigComboBox);
            this.ActiveWavesTab.Location = new System.Drawing.Point(4, 22);
            this.ActiveWavesTab.Name = "ActiveWavesTab";
            this.ActiveWavesTab.Padding = new System.Windows.Forms.Padding(3);
            this.ActiveWavesTab.Size = new System.Drawing.Size(992, 574);
            this.ActiveWavesTab.TabIndex = 0;
            this.ActiveWavesTab.Text = "Active Waves";
            this.ActiveWavesTab.UseVisualStyleBackColor = true;
            // 
            // ActiveWavesListView
            // 
            this.ActiveWavesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActiveWavesListView.FullRowSelect = true;
            this.ActiveWavesListView.GridLines = true;
            this.ActiveWavesListView.Location = new System.Drawing.Point(3, 3);
            this.ActiveWavesListView.Name = "ActiveWavesListView";
            this.ActiveWavesListView.Size = new System.Drawing.Size(986, 500);
            this.ActiveWavesListView.TabIndex = 0;
            this.ActiveWavesListView.UseCompatibleStateImageBehavior = false;
            this.ActiveWavesListView.View = System.Windows.Forms.View.Details;
            this.ActiveWavesListView.Columns.Add("Wave Name", 200);
            this.ActiveWavesListView.Columns.Add("State", 150);
            this.ActiveWavesListView.Columns.Add("Current Round", 100);
            this.ActiveWavesListView.Columns.Add("Players", 100);
            this.ActiveWavesListView.Columns.Add("Monsters", 100);
            // 
            // StopWaveButton
            // 
            this.StopWaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StopWaveButton.Location = new System.Drawing.Point(914, 509);
            this.StopWaveButton.Name = "StopWaveButton";
            this.StopWaveButton.Size = new System.Drawing.Size(75, 23);
            this.StopWaveButton.TabIndex = 2;
            this.StopWaveButton.Text = "Stop Wave";
            this.StopWaveButton.UseVisualStyleBackColor = true;
            this.StopWaveButton.Click += new System.EventHandler(this.StopWaveButton_Click);
            // 
            // WaveConfigComboBox
            // 
            this.WaveConfigComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.WaveConfigComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WaveConfigComboBox.FormattingEnabled = true;
            this.WaveConfigComboBox.Location = new System.Drawing.Point(3, 509);
            this.WaveConfigComboBox.Name = "WaveConfigComboBox";
            this.WaveConfigComboBox.Size = new System.Drawing.Size(300, 21);
            this.WaveConfigComboBox.TabIndex = 3;
            // 
            // StartWaveButton
            // 
            this.StartWaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartWaveButton.Location = new System.Drawing.Point(833, 509);
            this.StartWaveButton.Name = "StartWaveButton";
            this.StartWaveButton.Size = new System.Drawing.Size(75, 23);
            this.StartWaveButton.TabIndex = 1;
            this.StartWaveButton.Text = "Start Wave";
            this.StartWaveButton.UseVisualStyleBackColor = true;
            this.StartWaveButton.Click += new System.EventHandler(this.StartWaveButton_Click);
            // 
            // WaveConfigTab
            // 
            this.WaveConfigTab.Controls.Add(this.WaveSpawnListBox);
            this.WaveConfigTab.Controls.Add(this.WaveName_label);
            this.WaveConfigTab.Controls.Add(this.WaveName_textbox);
            this.WaveConfigTab.Controls.Add(this.WaveDescription_label);
            this.WaveConfigTab.Controls.Add(this.WaveDescription_textbox);
            this.WaveConfigTab.Controls.Add(this.WaveMap_label);
            this.WaveConfigTab.Controls.Add(this.WaveMap_combo);
            this.WaveConfigTab.Controls.Add(this.InstanceId_label);
            this.WaveConfigTab.Controls.Add(this.InstanceId_textbox);
            this.WaveConfigTab.Controls.Add(this.UseInstances_checkbox);
            this.WaveConfigTab.Controls.Add(this.RoundDuration_label);
            this.WaveConfigTab.Controls.Add(this.RoundDuration_textbox);
            this.WaveConfigTab.Controls.Add(this.SpawnDelay_label);
            this.WaveConfigTab.Controls.Add(this.SpawnDelay_textbox);
            this.WaveConfigTab.Controls.Add(this.RoundStartDelay_label);
            this.WaveConfigTab.Controls.Add(this.RoundStartDelay_textbox);
            this.WaveConfigTab.Controls.Add(this.CompletionCheckInterval_label);
            this.WaveConfigTab.Controls.Add(this.CompletionCheckInterval_textbox);
            this.WaveConfigTab.Controls.Add(this.RequireAllMonstersKilled_checkbox);
            this.WaveConfigTab.Controls.Add(this.AutoAdvanceRounds_checkbox);
            this.WaveConfigTab.Controls.Add(this.AllowMultiplePlayers_checkbox);
            this.WaveConfigTab.Controls.Add(this.AddWaveButton);
            this.WaveConfigTab.Controls.Add(this.DeleteWaveButton);
            this.WaveConfigTab.Controls.Add(this.SaveWaveButton);
            this.WaveConfigTab.Location = new System.Drawing.Point(4, 22);
            this.WaveConfigTab.Name = "WaveConfigTab";
            this.WaveConfigTab.Padding = new System.Windows.Forms.Padding(3);
            this.WaveConfigTab.Size = new System.Drawing.Size(992, 574);
            this.WaveConfigTab.TabIndex = 1;
            this.WaveConfigTab.Text = "Wave Configurations";
            this.WaveConfigTab.UseVisualStyleBackColor = true;
            // 
            // WaveSpawnListBox
            // 
            this.WaveSpawnListBox.FormattingEnabled = true;
            this.WaveSpawnListBox.Location = new System.Drawing.Point(3, 3);
            this.WaveSpawnListBox.Name = "WaveSpawnListBox";
            this.WaveSpawnListBox.Size = new System.Drawing.Size(200, 550);
            this.WaveSpawnListBox.TabIndex = 0;
            this.WaveSpawnListBox.SelectedIndexChanged += new System.EventHandler(this.WaveSpawnListBox_SelectedIndexChanged);
            // 
            // WaveName_label
            // 
            this.WaveName_label.AutoSize = true;
            this.WaveName_label.Location = new System.Drawing.Point(209, 3);
            this.WaveName_label.Name = "WaveName_label";
            this.WaveName_label.Size = new System.Drawing.Size(38, 13);
            this.WaveName_label.TabIndex = 39;
            this.WaveName_label.Text = "Name:";
            // 
            // WaveName_textbox
            // 
            this.WaveName_textbox.Location = new System.Drawing.Point(209, 19);
            this.WaveName_textbox.Name = "WaveName_textbox";
            this.WaveName_textbox.Size = new System.Drawing.Size(200, 20);
            this.WaveName_textbox.TabIndex = 1;
            // 
            // WaveDescription_label
            // 
            this.WaveDescription_label.AutoSize = true;
            this.WaveDescription_label.Location = new System.Drawing.Point(209, 45);
            this.WaveDescription_label.Name = "WaveDescription_label";
            this.WaveDescription_label.Size = new System.Drawing.Size(63, 13);
            this.WaveDescription_label.TabIndex = 40;
            this.WaveDescription_label.Text = "Description:";
            // 
            // WaveDescription_textbox
            // 
            this.WaveDescription_textbox.Location = new System.Drawing.Point(209, 61);
            this.WaveDescription_textbox.Multiline = true;
            this.WaveDescription_textbox.Name = "WaveDescription_textbox";
            this.WaveDescription_textbox.Size = new System.Drawing.Size(200, 100);
            this.WaveDescription_textbox.TabIndex = 2;
            // 
            // WaveMap_label
            // 
            this.WaveMap_label.AutoSize = true;
            this.WaveMap_label.Location = new System.Drawing.Point(209, 167);
            this.WaveMap_label.Name = "WaveMap_label";
            this.WaveMap_label.Size = new System.Drawing.Size(31, 13);
            this.WaveMap_label.TabIndex = 41;
            this.WaveMap_label.Text = "Map:";
            // 
            // WaveMap_combo
            // 
            this.WaveMap_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WaveMap_combo.FormattingEnabled = true;
            this.WaveMap_combo.Location = new System.Drawing.Point(209, 183);
            this.WaveMap_combo.Name = "WaveMap_combo";
            this.WaveMap_combo.Size = new System.Drawing.Size(200, 21);
            this.WaveMap_combo.TabIndex = 3;
            // 
            // InstanceId_label
            // 
            this.InstanceId_label.AutoSize = true;
            this.InstanceId_label.Location = new System.Drawing.Point(209, 210);
            this.InstanceId_label.Name = "InstanceId_label";
            this.InstanceId_label.Size = new System.Drawing.Size(70, 13);
            this.InstanceId_label.TabIndex = 42;
            this.InstanceId_label.Text = "Instance ID:";
            // 
            // InstanceId_textbox
            // 
            this.InstanceId_textbox.Location = new System.Drawing.Point(209, 226);
            this.InstanceId_textbox.Name = "InstanceId_textbox";
            this.InstanceId_textbox.Size = new System.Drawing.Size(100, 20);
            this.InstanceId_textbox.TabIndex = 4;
            // 
            // UseInstances_checkbox
            // 
            this.UseInstances_checkbox.AutoSize = true;
            this.UseInstances_checkbox.Location = new System.Drawing.Point(315, 228);
            this.UseInstances_checkbox.Name = "UseInstances_checkbox";
            this.UseInstances_checkbox.Size = new System.Drawing.Size(94, 17);
            this.UseInstances_checkbox.TabIndex = 5;
            this.UseInstances_checkbox.Text = "Use Instances";
            this.UseInstances_checkbox.UseVisualStyleBackColor = true;
            // 
            // RoundDuration_label
            // 
            this.RoundDuration_label.AutoSize = true;
            this.RoundDuration_label.Location = new System.Drawing.Point(209, 252);
            this.RoundDuration_label.Name = "RoundDuration_label";
            this.RoundDuration_label.Size = new System.Drawing.Size(93, 13);
            this.RoundDuration_label.TabIndex = 43;
            this.RoundDuration_label.Text = "Round Duration:";
            // 
            // RoundDuration_textbox
            // 
            this.RoundDuration_textbox.Location = new System.Drawing.Point(209, 268);
            this.RoundDuration_textbox.Name = "RoundDuration_textbox";
            this.RoundDuration_textbox.Size = new System.Drawing.Size(100, 20);
            this.RoundDuration_textbox.TabIndex = 6;
            // 
            // SpawnDelay_label
            // 
            this.SpawnDelay_label.AutoSize = true;
            this.SpawnDelay_label.Location = new System.Drawing.Point(209, 294);
            this.SpawnDelay_label.Name = "SpawnDelay_label";
            this.SpawnDelay_label.Size = new System.Drawing.Size(75, 13);
            this.SpawnDelay_label.TabIndex = 44;
            this.SpawnDelay_label.Text = "Spawn Delay:";
            // 
            // SpawnDelay_textbox
            // 
            this.SpawnDelay_textbox.Location = new System.Drawing.Point(209, 310);
            this.SpawnDelay_textbox.Name = "SpawnDelay_textbox";
            this.SpawnDelay_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnDelay_textbox.TabIndex = 7;
            // 
            // RoundStartDelay_label
            // 
            this.RoundStartDelay_label.AutoSize = true;
            this.RoundStartDelay_label.Location = new System.Drawing.Point(209, 336);
            this.RoundStartDelay_label.Name = "RoundStartDelay_label";
            this.RoundStartDelay_label.Size = new System.Drawing.Size(100, 13);
            this.RoundStartDelay_label.TabIndex = 45;
            this.RoundStartDelay_label.Text = "Round Start Delay:";
            // 
            // RoundStartDelay_textbox
            // 
            this.RoundStartDelay_textbox.Location = new System.Drawing.Point(209, 352);
            this.RoundStartDelay_textbox.Name = "RoundStartDelay_textbox";
            this.RoundStartDelay_textbox.Size = new System.Drawing.Size(100, 20);
            this.RoundStartDelay_textbox.TabIndex = 8;
            // 
            // CompletionCheckInterval_label
            // 
            this.CompletionCheckInterval_label.AutoSize = true;
            this.CompletionCheckInterval_label.Location = new System.Drawing.Point(209, 378);
            this.CompletionCheckInterval_label.Name = "CompletionCheckInterval_label";
            this.CompletionCheckInterval_label.Size = new System.Drawing.Size(140, 13);
            this.CompletionCheckInterval_label.TabIndex = 46;
            this.CompletionCheckInterval_label.Text = "Completion Check Interval:";
            // 
            // CompletionCheckInterval_textbox
            // 
            this.CompletionCheckInterval_textbox.Location = new System.Drawing.Point(209, 394);
            this.CompletionCheckInterval_textbox.Name = "CompletionCheckInterval_textbox";
            this.CompletionCheckInterval_textbox.Size = new System.Drawing.Size(100, 20);
            this.CompletionCheckInterval_textbox.TabIndex = 9;
            // 
            // RequireAllMonstersKilled_checkbox
            // 
            this.RequireAllMonstersKilled_checkbox.AutoSize = true;
            this.RequireAllMonstersKilled_checkbox.Location = new System.Drawing.Point(209, 420);
            this.RequireAllMonstersKilled_checkbox.Name = "RequireAllMonstersKilled_checkbox";
            this.RequireAllMonstersKilled_checkbox.Size = new System.Drawing.Size(158, 17);
            this.RequireAllMonstersKilled_checkbox.TabIndex = 10;
            this.RequireAllMonstersKilled_checkbox.Text = "Require All Monsters Killed";
            this.RequireAllMonstersKilled_checkbox.UseVisualStyleBackColor = true;
            // 
            // AutoAdvanceRounds_checkbox
            // 
            this.AutoAdvanceRounds_checkbox.AutoSize = true;
            this.AutoAdvanceRounds_checkbox.Location = new System.Drawing.Point(209, 443);
            this.AutoAdvanceRounds_checkbox.Name = "AutoAdvanceRounds_checkbox";
            this.AutoAdvanceRounds_checkbox.Size = new System.Drawing.Size(130, 17);
            this.AutoAdvanceRounds_checkbox.TabIndex = 11;
            this.AutoAdvanceRounds_checkbox.Text = "Auto Advance Rounds";
            this.AutoAdvanceRounds_checkbox.UseVisualStyleBackColor = true;
            // 
            // AllowMultiplePlayers_checkbox
            // 
            this.AllowMultiplePlayers_checkbox.AutoSize = true;
            this.AllowMultiplePlayers_checkbox.Location = new System.Drawing.Point(209, 466);
            this.AllowMultiplePlayers_checkbox.Name = "AllowMultiplePlayers_checkbox";
            this.AllowMultiplePlayers_checkbox.Size = new System.Drawing.Size(135, 17);
            this.AllowMultiplePlayers_checkbox.TabIndex = 12;
            this.AllowMultiplePlayers_checkbox.Text = "Allow Multiple Players";
            this.AllowMultiplePlayers_checkbox.UseVisualStyleBackColor = true;
            // 
            // AddWaveButton
            // 
            this.AddWaveButton.Location = new System.Drawing.Point(209, 489);
            this.AddWaveButton.Name = "AddWaveButton";
            this.AddWaveButton.Size = new System.Drawing.Size(75, 23);
            this.AddWaveButton.TabIndex = 13;
            this.AddWaveButton.Text = "Add Wave";
            this.AddWaveButton.UseVisualStyleBackColor = true;
            this.AddWaveButton.Click += new System.EventHandler(this.AddWaveButton_Click);
            // 
            // DeleteWaveButton
            // 
            this.DeleteWaveButton.Location = new System.Drawing.Point(290, 489);
            this.DeleteWaveButton.Name = "DeleteWaveButton";
            this.DeleteWaveButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteWaveButton.TabIndex = 14;
            this.DeleteWaveButton.Text = "Delete Wave";
            this.DeleteWaveButton.UseVisualStyleBackColor = true;
            this.DeleteWaveButton.Click += new System.EventHandler(this.DeleteWaveButton_Click);
            // 
            // SaveWaveButton
            // 
            this.SaveWaveButton.Location = new System.Drawing.Point(371, 489);
            this.SaveWaveButton.Name = "SaveWaveButton";
            this.SaveWaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveWaveButton.TabIndex = 15;
            this.SaveWaveButton.Text = "Save Wave";
            this.SaveWaveButton.UseVisualStyleBackColor = true;
            this.SaveWaveButton.Click += new System.EventHandler(this.SaveWaveButton_Click);
            // 
            // RoundsTab
            // 
            this.RoundsTab.Controls.Add(this.RoundsListBox);
            this.RoundsTab.Controls.Add(this.RoundNumber_label);
            this.RoundsTab.Controls.Add(this.RoundNumber_textbox);
            this.RoundsTab.Controls.Add(this.RoundName_label);
            this.RoundsTab.Controls.Add(this.RoundName_textbox);
            this.RoundsTab.Controls.Add(this.RoundMap_label);
            this.RoundsTab.Controls.Add(this.RoundMap_combo);
            this.RoundsTab.Controls.Add(this.RoundInstanceId_label);
            this.RoundsTab.Controls.Add(this.RoundInstanceId_textbox);
            this.RoundsTab.Controls.Add(this.SpawnCenterX_label);
            this.RoundsTab.Controls.Add(this.SpawnCenterX_textbox);
            this.RoundsTab.Controls.Add(this.SpawnCenterY_label);
            this.RoundsTab.Controls.Add(this.SpawnCenterY_textbox);
            this.RoundsTab.Controls.Add(this.SpawnRadius_label);
            this.RoundsTab.Controls.Add(this.SpawnRadius_textbox);
            this.RoundsTab.Controls.Add(this.AddRoundButton);
            this.RoundsTab.Controls.Add(this.DeleteRoundButton);
            this.RoundsTab.Controls.Add(this.SaveRoundButton);
            this.RoundsTab.Controls.Add(this.PatternsListBox);
            this.RoundsTab.Controls.Add(this.PatternMonster_label);
            this.RoundsTab.Controls.Add(this.PatternMonster_combo);
            this.RoundsTab.Controls.Add(this.PatternCount_label);
            this.RoundsTab.Controls.Add(this.PatternCount_textbox);
            this.RoundsTab.Controls.Add(this.PatternSpawnDelay_label);
            this.RoundsTab.Controls.Add(this.PatternSpawnDelay_textbox);
            this.RoundsTab.Controls.Add(this.SpawnType_label);
            this.RoundsTab.Controls.Add(this.SpawnType_combo);
            this.RoundsTab.Controls.Add(this.PatternStaggerDelay_label);
            this.RoundsTab.Controls.Add(this.PatternStaggerDelay_textbox);
            this.RoundsTab.Controls.Add(this.AddPatternButton);
            this.RoundsTab.Controls.Add(this.DeletePatternButton);
            this.RoundsTab.Controls.Add(this.SavePatternButton);
            this.RoundsTab.Controls.Add(this.SpawnLocationsListBox);
            this.RoundsTab.Controls.Add(this.SpawnLocationX_label);
            this.RoundsTab.Controls.Add(this.SpawnLocationX_textbox);
            this.RoundsTab.Controls.Add(this.SpawnLocationY_label);
            this.RoundsTab.Controls.Add(this.SpawnLocationY_textbox);
            this.RoundsTab.Controls.Add(this.AddSpawnLocationButton);
            this.RoundsTab.Controls.Add(this.DeleteSpawnLocationButton);
            this.RoundsTab.Controls.Add(this.SpawnModeInfo_label);
            this.RoundsTab.Location = new System.Drawing.Point(4, 22);
            this.RoundsTab.Name = "RoundsTab";
            this.RoundsTab.Size = new System.Drawing.Size(992, 574);
            this.RoundsTab.TabIndex = 2;
            this.RoundsTab.Text = "Round Editor";
            this.RoundsTab.UseVisualStyleBackColor = true;
            // 
            // RoundsListBox
            // 
            this.RoundsListBox.FormattingEnabled = true;
            this.RoundsListBox.Location = new System.Drawing.Point(3, 3);
            this.RoundsListBox.Name = "RoundsListBox";
            this.RoundsListBox.Size = new System.Drawing.Size(200, 200);
            this.RoundsListBox.TabIndex = 0;
            this.RoundsListBox.SelectedIndexChanged += new System.EventHandler(this.RoundsListBox_SelectedIndexChanged);
            // 
            // RoundNumber_label
            // 
            this.RoundNumber_label.AutoSize = true;
            this.RoundNumber_label.Location = new System.Drawing.Point(209, 3);
            this.RoundNumber_label.Name = "RoundNumber_label";
            this.RoundNumber_label.Size = new System.Drawing.Size(47, 13);
            this.RoundNumber_label.TabIndex = 38;
            this.RoundNumber_label.Text = "Round:";
            // 
            // RoundNumber_textbox
            // 
            this.RoundNumber_textbox.Location = new System.Drawing.Point(209, 19);
            this.RoundNumber_textbox.Name = "RoundNumber_textbox";
            this.RoundNumber_textbox.Size = new System.Drawing.Size(100, 20);
            this.RoundNumber_textbox.TabIndex = 1;
            // 
            // RoundName_label
            // 
            this.RoundName_label.AutoSize = true;
            this.RoundName_label.Location = new System.Drawing.Point(315, 3);
            this.RoundName_label.Name = "RoundName_label";
            this.RoundName_label.Size = new System.Drawing.Size(38, 13);
            this.RoundName_label.TabIndex = 25;
            this.RoundName_label.Text = "Name:";
            // 
            // RoundName_textbox
            // 
            this.RoundName_textbox.Location = new System.Drawing.Point(315, 19);
            this.RoundName_textbox.Name = "RoundName_textbox";
            this.RoundName_textbox.Size = new System.Drawing.Size(200, 20);
            this.RoundName_textbox.TabIndex = 2;
            // 
            // RoundMap_label
            // 
            this.RoundMap_label.AutoSize = true;
            this.RoundMap_label.Location = new System.Drawing.Point(209, 45);
            this.RoundMap_label.Name = "RoundMap_label";
            this.RoundMap_label.Size = new System.Drawing.Size(31, 13);
            this.RoundMap_label.TabIndex = 26;
            this.RoundMap_label.Text = "Map:";
            // 
            // RoundMap_combo
            // 
            this.RoundMap_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RoundMap_combo.FormattingEnabled = true;
            this.RoundMap_combo.Location = new System.Drawing.Point(209, 61);
            this.RoundMap_combo.Name = "RoundMap_combo";
            this.RoundMap_combo.Size = new System.Drawing.Size(200, 21);
            this.RoundMap_combo.TabIndex = 3;
            // 
            // RoundInstanceId_label
            // 
            this.RoundInstanceId_label.AutoSize = true;
            this.RoundInstanceId_label.Location = new System.Drawing.Point(415, 45);
            this.RoundInstanceId_label.Name = "RoundInstanceId_label";
            this.RoundInstanceId_label.Size = new System.Drawing.Size(70, 13);
            this.RoundInstanceId_label.TabIndex = 27;
            this.RoundInstanceId_label.Text = "Instance ID:";
            // 
            // RoundInstanceId_textbox
            // 
            this.RoundInstanceId_textbox.Location = new System.Drawing.Point(415, 61);
            this.RoundInstanceId_textbox.Name = "RoundInstanceId_textbox";
            this.RoundInstanceId_textbox.Size = new System.Drawing.Size(100, 20);
            this.RoundInstanceId_textbox.TabIndex = 4;
            // 
            // SpawnCenterX_label
            // 
            this.SpawnCenterX_label.AutoSize = true;
            this.SpawnCenterX_label.Location = new System.Drawing.Point(209, 88);
            this.SpawnCenterX_label.Name = "SpawnCenterX_label";
            this.SpawnCenterX_label.Size = new System.Drawing.Size(95, 13);
            this.SpawnCenterX_label.TabIndex = 28;
            this.SpawnCenterX_label.Text = "Spawn Center X:";
            // 
            // SpawnCenterX_textbox
            // 
            this.SpawnCenterX_textbox.Location = new System.Drawing.Point(209, 104);
            this.SpawnCenterX_textbox.Name = "SpawnCenterX_textbox";
            this.SpawnCenterX_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnCenterX_textbox.TabIndex = 5;
            // 
            // SpawnCenterY_label
            // 
            this.SpawnCenterY_label.AutoSize = true;
            this.SpawnCenterY_label.Location = new System.Drawing.Point(315, 88);
            this.SpawnCenterY_label.Name = "SpawnCenterY_label";
            this.SpawnCenterY_label.Size = new System.Drawing.Size(95, 13);
            this.SpawnCenterY_label.TabIndex = 29;
            this.SpawnCenterY_label.Text = "Spawn Center Y:";
            // 
            // SpawnCenterY_textbox
            // 
            this.SpawnCenterY_textbox.Location = new System.Drawing.Point(315, 104);
            this.SpawnCenterY_textbox.Name = "SpawnCenterY_textbox";
            this.SpawnCenterY_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnCenterY_textbox.TabIndex = 6;
            // 
            // SpawnRadius_label
            // 
            this.SpawnRadius_label.AutoSize = true;
            this.SpawnRadius_label.Location = new System.Drawing.Point(421, 88);
            this.SpawnRadius_label.Name = "SpawnRadius_label";
            this.SpawnRadius_label.Size = new System.Drawing.Size(85, 13);
            this.SpawnRadius_label.TabIndex = 30;
            this.SpawnRadius_label.Text = "Spawn Radius:";
            // 
            // SpawnRadius_textbox
            // 
            this.SpawnRadius_textbox.Location = new System.Drawing.Point(421, 104);
            this.SpawnRadius_textbox.Name = "SpawnRadius_textbox";
            this.SpawnRadius_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnRadius_textbox.TabIndex = 7;
            // 
            // SpawnModeInfo_label
            // 
            this.SpawnModeInfo_label.AutoSize = false;
            this.SpawnModeInfo_label.Location = new System.Drawing.Point(209, 130);
            this.SpawnModeInfo_label.Name = "SpawnModeInfo_label";
            this.SpawnModeInfo_label.Size = new System.Drawing.Size(500, 50);
            this.SpawnModeInfo_label.TabIndex = 47;
            this.SpawnModeInfo_label.Text = "Spawn Modes:\r\n    • Area-based (Radius > 0): Random spawns within radius around center point\r\n    • Exact coordinates (Radius = 0 + Spawn Locations): Spawns at specific points\r\n    Priority: Radius > Locations > Random walkable cells";
            // 
            // AddRoundButton
            // 
            this.AddRoundButton.Location = new System.Drawing.Point(209, 186);
            this.AddRoundButton.Name = "AddRoundButton";
            this.AddRoundButton.Size = new System.Drawing.Size(75, 23);
            this.AddRoundButton.TabIndex = 8;
            this.AddRoundButton.Text = "Add Round";
            this.AddRoundButton.UseVisualStyleBackColor = true;
            this.AddRoundButton.Click += new System.EventHandler(this.AddRoundButton_Click);
            // 
            // DeleteRoundButton
            // 
            this.DeleteRoundButton.Location = new System.Drawing.Point(290, 186);
            this.DeleteRoundButton.Name = "DeleteRoundButton";
            this.DeleteRoundButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteRoundButton.TabIndex = 9;
            this.DeleteRoundButton.Text = "Delete Round";
            this.DeleteRoundButton.UseVisualStyleBackColor = true;
            this.DeleteRoundButton.Click += new System.EventHandler(this.DeleteRoundButton_Click);
            // 
            // SaveRoundButton
            // 
            this.SaveRoundButton.Location = new System.Drawing.Point(371, 186);
            this.SaveRoundButton.Name = "SaveRoundButton";
            this.SaveRoundButton.Size = new System.Drawing.Size(75, 23);
            this.SaveRoundButton.TabIndex = 10;
            this.SaveRoundButton.Text = "Save Round";
            this.SaveRoundButton.UseVisualStyleBackColor = true;
            this.SaveRoundButton.Click += new System.EventHandler(this.SaveRoundButton_Click);
            // 
            // PatternsListBox
            // 
            this.PatternsListBox.FormattingEnabled = true;
            this.PatternsListBox.Location = new System.Drawing.Point(209, 215);
            this.PatternsListBox.Name = "PatternsListBox";
            this.PatternsListBox.Size = new System.Drawing.Size(200, 200);
            this.PatternsListBox.TabIndex = 11;
            this.PatternsListBox.SelectedIndexChanged += new System.EventHandler(this.PatternsListBox_SelectedIndexChanged);
            // 
            // PatternMonster_label
            // 
            this.PatternMonster_label.AutoSize = true;
            this.PatternMonster_label.Location = new System.Drawing.Point(415, 215);
            this.PatternMonster_label.Name = "PatternMonster_label";
            this.PatternMonster_label.Size = new System.Drawing.Size(50, 13);
            this.PatternMonster_label.TabIndex = 31;
            this.PatternMonster_label.Text = "Monster:";
            // 
            // PatternMonster_combo
            // 
            this.PatternMonster_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PatternMonster_combo.FormattingEnabled = true;
            this.PatternMonster_combo.Location = new System.Drawing.Point(415, 231);
            this.PatternMonster_combo.Name = "PatternMonster_combo";
            this.PatternMonster_combo.Size = new System.Drawing.Size(200, 21);
            this.PatternMonster_combo.TabIndex = 12;
            // 
            // PatternCount_label
            // 
            this.PatternCount_label.AutoSize = true;
            this.PatternCount_label.Location = new System.Drawing.Point(415, 258);
            this.PatternCount_label.Name = "PatternCount_label";
            this.PatternCount_label.Size = new System.Drawing.Size(38, 13);
            this.PatternCount_label.TabIndex = 32;
            this.PatternCount_label.Text = "Count:";
            // 
            // PatternCount_textbox
            // 
            this.PatternCount_textbox.Location = new System.Drawing.Point(415, 274);
            this.PatternCount_textbox.Name = "PatternCount_textbox";
            this.PatternCount_textbox.Size = new System.Drawing.Size(100, 20);
            this.PatternCount_textbox.TabIndex = 13;
            // 
            // PatternSpawnDelay_label
            // 
            this.PatternSpawnDelay_label.AutoSize = true;
            this.PatternSpawnDelay_label.Location = new System.Drawing.Point(415, 300);
            this.PatternSpawnDelay_label.Name = "PatternSpawnDelay_label";
            this.PatternSpawnDelay_label.Size = new System.Drawing.Size(75, 13);
            this.PatternSpawnDelay_label.TabIndex = 33;
            this.PatternSpawnDelay_label.Text = "Spawn Delay:";
            // 
            // PatternSpawnDelay_textbox
            // 
            this.PatternSpawnDelay_textbox.Location = new System.Drawing.Point(415, 316);
            this.PatternSpawnDelay_textbox.Name = "PatternSpawnDelay_textbox";
            this.PatternSpawnDelay_textbox.Size = new System.Drawing.Size(100, 20);
            this.PatternSpawnDelay_textbox.TabIndex = 14;
            // 
            // SpawnType_label
            // 
            this.SpawnType_label.AutoSize = true;
            this.SpawnType_label.Location = new System.Drawing.Point(415, 342);
            this.SpawnType_label.Name = "SpawnType_label";
            this.SpawnType_label.Size = new System.Drawing.Size(70, 13);
            this.SpawnType_label.TabIndex = 34;
            this.SpawnType_label.Text = "Spawn Type:";
            // 
            // SpawnType_combo
            // 
            this.SpawnType_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SpawnType_combo.FormattingEnabled = true;
            this.SpawnType_combo.Location = new System.Drawing.Point(415, 358);
            this.SpawnType_combo.Name = "SpawnType_combo";
            this.SpawnType_combo.Size = new System.Drawing.Size(200, 21);
            this.SpawnType_combo.TabIndex = 15;
            // 
            // PatternStaggerDelay_label
            // 
            this.PatternStaggerDelay_label.AutoSize = true;
            this.PatternStaggerDelay_label.Location = new System.Drawing.Point(415, 384);
            this.PatternStaggerDelay_label.Name = "PatternStaggerDelay_label";
            this.PatternStaggerDelay_label.Size = new System.Drawing.Size(80, 13);
            this.PatternStaggerDelay_label.TabIndex = 35;
            this.PatternStaggerDelay_label.Text = "Stagger Delay:";
            // 
            // PatternStaggerDelay_textbox
            // 
            this.PatternStaggerDelay_textbox.Location = new System.Drawing.Point(415, 400);
            this.PatternStaggerDelay_textbox.Name = "PatternStaggerDelay_textbox";
            this.PatternStaggerDelay_textbox.Size = new System.Drawing.Size(100, 20);
            this.PatternStaggerDelay_textbox.TabIndex = 16;
            // 
            // AddPatternButton
            // 
            this.AddPatternButton.Location = new System.Drawing.Point(415, 426);
            this.AddPatternButton.Name = "AddPatternButton";
            this.AddPatternButton.Size = new System.Drawing.Size(75, 23);
            this.AddPatternButton.TabIndex = 17;
            this.AddPatternButton.Text = "Add Pattern";
            this.AddPatternButton.UseVisualStyleBackColor = true;
            this.AddPatternButton.Click += new System.EventHandler(this.AddPatternButton_Click);
            // 
            // DeletePatternButton
            // 
            this.DeletePatternButton.Location = new System.Drawing.Point(496, 426);
            this.DeletePatternButton.Name = "DeletePatternButton";
            this.DeletePatternButton.Size = new System.Drawing.Size(75, 23);
            this.DeletePatternButton.TabIndex = 18;
            this.DeletePatternButton.Text = "Delete Pattern";
            this.DeletePatternButton.UseVisualStyleBackColor = true;
            this.DeletePatternButton.Click += new System.EventHandler(this.DeletePatternButton_Click);
            // 
            // SavePatternButton
            // 
            this.SavePatternButton.Location = new System.Drawing.Point(577, 426);
            this.SavePatternButton.Name = "SavePatternButton";
            this.SavePatternButton.Size = new System.Drawing.Size(75, 23);
            this.SavePatternButton.TabIndex = 19;
            this.SavePatternButton.Text = "Save Pattern";
            this.SavePatternButton.UseVisualStyleBackColor = true;
            this.SavePatternButton.Click += new System.EventHandler(this.SavePatternButton_Click);
            // 
            // SpawnLocationsListBox
            // 
            this.SpawnLocationsListBox.FormattingEnabled = true;
            this.SpawnLocationsListBox.Location = new System.Drawing.Point(700, 205);
            this.SpawnLocationsListBox.Name = "SpawnLocationsListBox";
            this.SpawnLocationsListBox.Size = new System.Drawing.Size(200, 200);
            this.SpawnLocationsListBox.TabIndex = 20;
            // 
            // SpawnLocationX_label
            // 
            this.SpawnLocationX_label.AutoSize = true;
            this.SpawnLocationX_label.Location = new System.Drawing.Point(700, 421);
            this.SpawnLocationX_label.Name = "SpawnLocationX_label";
            this.SpawnLocationX_label.Size = new System.Drawing.Size(17, 13);
            this.SpawnLocationX_label.TabIndex = 36;
            this.SpawnLocationX_label.Text = "X:";
            // 
            // SpawnLocationX_textbox
            // 
            this.SpawnLocationX_textbox.Location = new System.Drawing.Point(700, 437);
            this.SpawnLocationX_textbox.Name = "SpawnLocationX_textbox";
            this.SpawnLocationX_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnLocationX_textbox.TabIndex = 21;
            // 
            // SpawnLocationY_label
            // 
            this.SpawnLocationY_label.AutoSize = true;
            this.SpawnLocationY_label.Location = new System.Drawing.Point(806, 421);
            this.SpawnLocationY_label.Name = "SpawnLocationY_label";
            this.SpawnLocationY_label.Size = new System.Drawing.Size(17, 13);
            this.SpawnLocationY_label.TabIndex = 37;
            this.SpawnLocationY_label.Text = "Y:";
            // 
            // SpawnLocationY_textbox
            // 
            this.SpawnLocationY_textbox.Location = new System.Drawing.Point(806, 437);
            this.SpawnLocationY_textbox.Name = "SpawnLocationY_textbox";
            this.SpawnLocationY_textbox.Size = new System.Drawing.Size(100, 20);
            this.SpawnLocationY_textbox.TabIndex = 22;
            // 
            // AddSpawnLocationButton
            // 
            this.AddSpawnLocationButton.Location = new System.Drawing.Point(700, 463);
            this.AddSpawnLocationButton.Name = "AddSpawnLocationButton";
            this.AddSpawnLocationButton.Size = new System.Drawing.Size(100, 23);
            this.AddSpawnLocationButton.TabIndex = 23;
            this.AddSpawnLocationButton.Text = "Add Location";
            this.AddSpawnLocationButton.UseVisualStyleBackColor = true;
            this.AddSpawnLocationButton.Click += new System.EventHandler(this.AddSpawnLocationButton_Click);
            // 
            // DeleteSpawnLocationButton
            // 
            this.DeleteSpawnLocationButton.Location = new System.Drawing.Point(806, 463);
            this.DeleteSpawnLocationButton.Name = "DeleteSpawnLocationButton";
            this.DeleteSpawnLocationButton.Size = new System.Drawing.Size(100, 23);
            this.DeleteSpawnLocationButton.TabIndex = 24;
            this.DeleteSpawnLocationButton.Text = "Delete Location";
            this.DeleteSpawnLocationButton.UseVisualStyleBackColor = true;
            this.DeleteSpawnLocationButton.Click += new System.EventHandler(this.DeleteSpawnLocationButton_Click);
            // 
            // RewardsTab
            // 
            this.RewardsTab.Controls.Add(this.RewardsListBox);
            this.RewardsTab.Controls.Add(this.RewardRoundNumber_textbox);
            this.RewardsTab.Controls.Add(this.RewardGold_textbox);
            this.RewardsTab.Controls.Add(this.RewardExp_textbox);
            this.RewardsTab.Controls.Add(this.AddRewardButton);
            this.RewardsTab.Controls.Add(this.DeleteRewardButton);
            this.RewardsTab.Controls.Add(this.SaveRewardButton);
            this.RewardsTab.Location = new System.Drawing.Point(4, 22);
            this.RewardsTab.Name = "RewardsTab";
            this.RewardsTab.Size = new System.Drawing.Size(992, 574);
            this.RewardsTab.TabIndex = 3;
            this.RewardsTab.Text = "Reward Manager";
            this.RewardsTab.UseVisualStyleBackColor = true;
            // 
            // RewardsListBox
            // 
            this.RewardsListBox.FormattingEnabled = true;
            this.RewardsListBox.Location = new System.Drawing.Point(3, 3);
            this.RewardsListBox.Name = "RewardsListBox";
            this.RewardsListBox.Size = new System.Drawing.Size(200, 550);
            this.RewardsListBox.TabIndex = 0;
            this.RewardsListBox.SelectedIndexChanged += new System.EventHandler(this.RewardsListBox_SelectedIndexChanged);
            // 
            // RewardRoundNumber_textbox
            // 
            this.RewardRoundNumber_textbox.Location = new System.Drawing.Point(209, 3);
            this.RewardRoundNumber_textbox.Name = "RewardRoundNumber_textbox";
            this.RewardRoundNumber_textbox.Size = new System.Drawing.Size(100, 20);
            this.RewardRoundNumber_textbox.TabIndex = 1;
            // 
            // RewardGold_textbox
            // 
            this.RewardGold_textbox.Location = new System.Drawing.Point(209, 29);
            this.RewardGold_textbox.Name = "RewardGold_textbox";
            this.RewardGold_textbox.Size = new System.Drawing.Size(100, 20);
            this.RewardGold_textbox.TabIndex = 2;
            // 
            // RewardExp_textbox
            // 
            this.RewardExp_textbox.Location = new System.Drawing.Point(209, 55);
            this.RewardExp_textbox.Name = "RewardExp_textbox";
            this.RewardExp_textbox.Size = new System.Drawing.Size(100, 20);
            this.RewardExp_textbox.TabIndex = 3;
            // 
            // AddRewardButton
            // 
            this.AddRewardButton.Location = new System.Drawing.Point(209, 81);
            this.AddRewardButton.Name = "AddRewardButton";
            this.AddRewardButton.Size = new System.Drawing.Size(75, 23);
            this.AddRewardButton.TabIndex = 4;
            this.AddRewardButton.Text = "Add Reward";
            this.AddRewardButton.UseVisualStyleBackColor = true;
            this.AddRewardButton.Click += new System.EventHandler(this.AddRewardButton_Click);
            // 
            // DeleteRewardButton
            // 
            this.DeleteRewardButton.Location = new System.Drawing.Point(290, 81);
            this.DeleteRewardButton.Name = "DeleteRewardButton";
            this.DeleteRewardButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteRewardButton.TabIndex = 5;
            this.DeleteRewardButton.Text = "Delete Reward";
            this.DeleteRewardButton.UseVisualStyleBackColor = true;
            this.DeleteRewardButton.Click += new System.EventHandler(this.DeleteRewardButton_Click);
            // 
            // SaveRewardButton
            // 
            this.SaveRewardButton.Location = new System.Drawing.Point(371, 81);
            this.SaveRewardButton.Name = "SaveRewardButton";
            this.SaveRewardButton.Size = new System.Drawing.Size(75, 23);
            this.SaveRewardButton.TabIndex = 6;
            this.SaveRewardButton.Text = "Save Reward";
            this.SaveRewardButton.UseVisualStyleBackColor = true;
            this.SaveRewardButton.Click += new System.EventHandler(this.SaveRewardButton_Click);
            // 
            // SaveDBButton
            // 
            this.SaveDBButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveDBButton.Location = new System.Drawing.Point(925, 606);
            this.SaveDBButton.Name = "SaveDBButton";
            this.SaveDBButton.Size = new System.Drawing.Size(75, 23);
            this.SaveDBButton.TabIndex = 1;
            this.SaveDBButton.Text = "Save DB";
            this.SaveDBButton.UseVisualStyleBackColor = true;
            this.SaveDBButton.Click += new System.EventHandler(this.SaveDBButton_Click);
            // 
            // WaveSpawnSystemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 635);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.SaveDBButton);
            this.Name = "WaveSpawnSystemForm";
            this.Text = "Wave Spawn System";
            this.MainTabControl.ResumeLayout(false);
            this.ActiveWavesTab.ResumeLayout(false);
            this.WaveConfigTab.ResumeLayout(false);
            this.WaveConfigTab.PerformLayout();
            this.RoundsTab.ResumeLayout(false);
            this.RoundsTab.PerformLayout();
            this.RewardsTab.ResumeLayout(false);
            this.RewardsTab.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage ActiveWavesTab;
        private System.Windows.Forms.TabPage WaveConfigTab;
        private System.Windows.Forms.TabPage RoundsTab;
        private System.Windows.Forms.TabPage RewardsTab;
        private System.Windows.Forms.ListView ActiveWavesListView;
        private System.Windows.Forms.Button StopWaveButton;
        private System.Windows.Forms.Button StartWaveButton;
        private System.Windows.Forms.ListBox WaveSpawnListBox;
        private System.Windows.Forms.TextBox WaveName_textbox;
        private System.Windows.Forms.TextBox WaveDescription_textbox;
        private System.Windows.Forms.ComboBox WaveMap_combo;
        private System.Windows.Forms.TextBox InstanceId_textbox;
        private System.Windows.Forms.CheckBox UseInstances_checkbox;
        private System.Windows.Forms.TextBox RoundDuration_textbox;
        private System.Windows.Forms.TextBox SpawnDelay_textbox;
        private System.Windows.Forms.TextBox RoundStartDelay_textbox;
        private System.Windows.Forms.TextBox CompletionCheckInterval_textbox;
        private System.Windows.Forms.CheckBox RequireAllMonstersKilled_checkbox;
        private System.Windows.Forms.CheckBox AutoAdvanceRounds_checkbox;
        private System.Windows.Forms.CheckBox AllowMultiplePlayers_checkbox;
        private System.Windows.Forms.Button AddWaveButton;
        private System.Windows.Forms.Button DeleteWaveButton;
        private System.Windows.Forms.Button SaveWaveButton;
        private System.Windows.Forms.ListBox RoundsListBox;
        private System.Windows.Forms.TextBox RoundNumber_textbox;
        private System.Windows.Forms.TextBox RoundName_textbox;
        private System.Windows.Forms.ComboBox RoundMap_combo;
        private System.Windows.Forms.TextBox RoundInstanceId_textbox;
        private System.Windows.Forms.TextBox SpawnCenterX_textbox;
        private System.Windows.Forms.TextBox SpawnCenterY_textbox;
        private System.Windows.Forms.TextBox SpawnRadius_textbox;
        private System.Windows.Forms.Button AddRoundButton;
        private System.Windows.Forms.Button DeleteRoundButton;
        private System.Windows.Forms.Button SaveRoundButton;
        private System.Windows.Forms.ListBox PatternsListBox;
        private System.Windows.Forms.ComboBox PatternMonster_combo;
        private System.Windows.Forms.TextBox PatternCount_textbox;
        private System.Windows.Forms.TextBox PatternSpawnDelay_textbox;
        private System.Windows.Forms.ComboBox SpawnType_combo;
        private System.Windows.Forms.TextBox PatternStaggerDelay_textbox;
        private System.Windows.Forms.Button AddPatternButton;
        private System.Windows.Forms.Button DeletePatternButton;
        private System.Windows.Forms.Button SavePatternButton;
        private System.Windows.Forms.ListBox SpawnLocationsListBox;
        private System.Windows.Forms.TextBox SpawnLocationX_textbox;
        private System.Windows.Forms.TextBox SpawnLocationY_textbox;
        private System.Windows.Forms.Button AddSpawnLocationButton;
        private System.Windows.Forms.Button DeleteSpawnLocationButton;
        private System.Windows.Forms.ListBox RewardsListBox;
        private System.Windows.Forms.TextBox RewardRoundNumber_textbox;
        private System.Windows.Forms.TextBox RewardGold_textbox;
        private System.Windows.Forms.TextBox RewardExp_textbox;
        private System.Windows.Forms.Button AddRewardButton;
        private System.Windows.Forms.Button DeleteRewardButton;
        private System.Windows.Forms.Button SaveRewardButton;
        private System.Windows.Forms.Button SaveDBButton;
        private System.Windows.Forms.Label RoundName_label;
        private System.Windows.Forms.Label RoundMap_label;
        private System.Windows.Forms.Label RoundInstanceId_label;
        private System.Windows.Forms.Label SpawnCenterX_label;
        private System.Windows.Forms.Label SpawnCenterY_label;
        private System.Windows.Forms.Label SpawnRadius_label;
        private System.Windows.Forms.Label PatternMonster_label;
        private System.Windows.Forms.Label PatternCount_label;
        private System.Windows.Forms.Label PatternSpawnDelay_label;
        private System.Windows.Forms.Label SpawnType_label;
        private System.Windows.Forms.Label PatternStaggerDelay_label;
        private System.Windows.Forms.Label SpawnLocationX_label;
        private System.Windows.Forms.Label SpawnLocationY_label;
        private System.Windows.Forms.Label RoundNumber_label;
        private System.Windows.Forms.Label WaveName_label;
        private System.Windows.Forms.Label SpawnModeInfo_label;
        private System.Windows.Forms.Label WaveDescription_label;
        private System.Windows.Forms.Label WaveMap_label;
        private System.Windows.Forms.Label InstanceId_label;
        private System.Windows.Forms.Label RoundDuration_label;
        private System.Windows.Forms.Label SpawnDelay_label;
        private System.Windows.Forms.Label RoundStartDelay_label;
        private System.Windows.Forms.Label CompletionCheckInterval_label;
        private System.Windows.Forms.ComboBox WaveConfigComboBox;
    }
}
