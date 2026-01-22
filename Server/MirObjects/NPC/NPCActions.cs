namespace Server.MirObjects
{
    public class NPCActions
    {
        public ActionType Type;
        public List<string> Params = new List<string>();

        public NPCActions(ActionType action, params string[] p)
        {
            Type = action;

            Params.AddRange(p);
        }
    }

    /// <summary>
    /// Enumeration of all available NPC action types that can be used in NPC scripts.
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Teleports the player to a specified map and coordinates.
        /// <para>Parameters: mapName, x, y (x and y default to 0 if not provided, which triggers random teleport)</para>
        /// <para>Example: MOVE DogYoMineLobby1 32 31</para>
        /// </summary>
        Move,
        
        /// <summary>
        /// Teleports the player to a specific instance of a map with coordinates.
        /// <para>Parameters: mapName, instanceId, x, y</para>
        /// <para>Example: INSTANCEMOVE DogYoMineLobby1 1 32 31</para>
        /// </summary>
        InstanceMove,
        
        /// <summary>
        /// Gives gold to the player's account.
        /// <para>Parameters: amount</para>
        /// <para>Example: GIVEGOLD 10000</para>
        /// </summary>
        GiveGold,
        
        /// <summary>
        /// Takes gold from the player's account.
        /// <para>Parameters: amount</para>
        /// <para>Example: TAKEGOLD 5000</para>
        /// </summary>
        TakeGold,
        
        /// <summary>
        /// Adds gold to the player's guild storage.
        /// <para>Parameters: amount</para>
        /// <para>Example: GIVEGUILDGOLD 50000</para>
        /// </summary>
        GiveGuildGold,
        
        /// <summary>
        /// Removes gold from the player's guild storage.
        /// <para>Parameters: amount</para>
        /// <para>Example: TAKEGUILDGOLD 30000</para>
        /// </summary>
        TakeGuildGold,
        
        /// <summary>
        /// Gives credit (premium currency) to the player's account.
        /// <para>Parameters: amount</para>
        /// <para>Example: GIVECREDIT 100</para>
        /// </summary>
        GiveCredit,
        
        /// <summary>
        /// Takes credit from the player's account.
        /// <para>Parameters: amount</para>
        /// <para>Example: TAKECREDIT 50</para>
        /// </summary>
        TakeCredit,
        
        /// <summary>
        /// Gives an item to the player.
        /// <para>Parameters: itemName, count (optional, defaults to 1)</para>
        /// <para>Example: GIVEITEM IronSword 1</para>
        /// <para>Example: GIVEITEM Gold 1000</para>
        /// </summary>
        GiveItem,
        
        /// <summary>
        /// Takes an item from the player's inventory.
        /// <para>Parameters: itemName, count (optional, defaults to 1), minDurability (optional)</para>
        /// <para>Example: TAKEITEM IronSword 1</para>
        /// <para>Example: TAKEITEM Gold 100 0</para>
        /// </summary>
        TakeItem,
        
        /// <summary>
        /// Gives experience points to the player.
        /// <para>Parameters: amount</para>
        /// <para>Example: GIVEEXP 10000</para>
        /// </summary>
        GiveExp,
        
        /// <summary>
        /// Spawns a pet for the player.
        /// <para>Parameters: monsterName, count (optional, defaults to 1), level (optional, defaults to 0, max 7)</para>
        /// <para>Example: GIVEPET Hen 1 0</para>
        /// <para>Example: GIVEPET Wolf 2 3</para>
        /// </summary>
        GivePet,
        
        /// <summary>
        /// Removes all pets from the player.
        /// <para>Parameters: none</para>
        /// <para>Example: CLEARPETS</para>
        /// </summary>
        ClearPets,
        
        /// <summary>
        /// Adds the player's name to a name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes for paths with spaces)</para>
        /// <para>Example: ADDNAMELIST "Quest/CompletedQuest1"</para>
        /// </summary>
        AddNameList,
        
        /// <summary>
        /// Removes the player's name from a name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes)</para>
        /// <para>Example: DELNAMELIST "Quest/CompletedQuest1"</para>
        /// </summary>
        DelNameList,
        
        /// <summary>
        /// Clears all names from a name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes)</para>
        /// <para>Example: CLEARNAMELIST "Quest/CompletedQuest1"</para>
        /// </summary>
        ClearNameList,
        
        /// <summary>
        /// Restores HP to the player.
        /// <para>Parameters: amount (can be negative to damage)</para>
        /// <para>Example: GIVEHP 1000</para>
        /// </summary>
        GiveHP,
        
        /// <summary>
        /// Restores MP to the player.
        /// <para>Parameters: amount (can be negative to drain)</para>
        /// <para>Example: GIVEMP 500</para>
        /// </summary>
        GiveMP,
        
        /// <summary>
        /// Changes the player's level.
        /// <para>Parameters: level</para>
        /// <para>Example: CHANGELEVEL 50</para>
        /// </summary>
        ChangeLevel,
        
        /// <summary>
        /// Sets the player's PK (Player Kill) points to a specific value.
        /// <para>Parameters: pkPoints</para>
        /// <para>Example: SETPKPOINT 100</para>
        /// </summary>
        SetPkPoint,
        
        /// <summary>
        /// Reduces the player's PK points.
        /// <para>Parameters: amount</para>
        /// <para>Example: REDUCEPKPOINT 10</para>
        /// </summary>
        ReducePkPoint,
        
        /// <summary>
        /// Increases the player's PK points.
        /// <para>Parameters: amount</para>
        /// <para>Example: INCREASEPKPOINT 20</para>
        /// </summary>
        IncreasePkPoint,
        
        /// <summary>
        /// Toggles the player's gender (Male to Female or vice versa).
        /// <para>Parameters: none</para>
        /// <para>Example: CHANGEGENDER</para>
        /// </summary>
        ChangeGender,
        
        /// <summary>
        /// Changes the player's class.
        /// <para>Parameters: className (Warrior, Wizard, Taoist, Assassin, Archer)</para>
        /// <para>Example: CHANGECLASS Warrior</para>
        /// </summary>
        ChangeClass,
        
        /// <summary>
        /// Sends a local message to the player.
        /// <para>Parameters: message (in quotes), chatType</para>
        /// <para>Example: LOCALMESSAGE "Welcome to the game!" System</para>
        /// </summary>
        LocalMessage,
        
        /// <summary>
        /// Jumps to a specific label/page in the NPC script.
        /// <para>Parameters: labelName (without @ symbol)</para>
        /// <para>Example: GOTO main</para>
        /// </summary>
        Goto,
        
        /// <summary>
        /// Calls another NPC script file.
        /// <para>Parameters: scriptName (without .txt extension, can use quotes)</para>
        /// <para>Example: CALL "CommonScripts/ShopKeeper"</para>
        /// </summary>
        Call,
        
        /// <summary>
        /// Gives a skill/spell to the player.
        /// <para>Parameters: spellName, level (optional, defaults to 0, max 3)</para>
        /// <para>Example: GIVESKILL FireBall 1</para>
        /// </summary>
        GiveSkill,
        
        /// <summary>
        /// Removes a skill/spell from the player.
        /// <para>Parameters: spellName</para>
        /// <para>Example: REMOVESKILL FireBall</para>
        /// </summary>
        RemoveSkill,
        
        /// <summary>
        /// Sets a quest flag for the player.
        /// <para>Parameters: flagIndex (in brackets), value (0 or 1)</para>
        /// <para>Example: SET [1] 1</para>
        /// </summary>
        Set,
        
        /// <summary>
        /// Sets Param1 (map name) and Param1Instance (instance ID) for use with Mongen.
        /// <para>Parameters: mapName, instanceId</para>
        /// <para>Example: PARAM1 DogYoMineLobby1 1</para>
        /// </summary>
        Param1,
        
        /// <summary>
        /// Sets Param2 (X coordinate) for use with Mongen.
        /// <para>Parameters: xCoordinate</para>
        /// <para>Example: PARAM2 50</para>
        /// </summary>
        Param2,
        
        /// <summary>
        /// Sets Param3 (Y coordinate) for use with Mongen.
        /// <para>Parameters: yCoordinate</para>
        /// <para>Example: PARAM3 50</para>
        /// </summary>
        Param3,
        
        /// <summary>
        /// Spawns monsters at the location specified by Param1, Param2, Param3.
        /// <para>Parameters: monsterName, count</para>
        /// <para>Example: PARAM1 DogYoMineLobby1 1</para>
        /// <para>          PARAM2 50</para>
        /// <para>          PARAM3 50</para>
        /// <para>          MONGEN Zombie1 5</para>
        /// </summary>
        Mongen,
        
        /// <summary>
        /// Schedules a delayed recall to the current location after specified seconds.
        /// <para>Parameters: seconds, labelName (optional, defaults to current page)</para>
        /// <para>Example: TIMERECALL 60 main</para>
        /// </summary>
        TimeRecall,
        
        /// <summary>
        /// Schedules a delayed recall for all group members.
        /// <para>Parameters: seconds, labelName (optional)</para>
        /// <para>Example: TIMERECALLGROUP 30 main</para>
        /// </summary>
        TimeRecallGroup,
        
        /// <summary>
        /// Cancels all pending time recall actions for the player.
        /// <para>Parameters: none</para>
        /// <para>Example: BREAKTIMERECALL</para>
        /// </summary>
        BreakTimeRecall,
        
        /// <summary>
        /// Clears all monsters from a specified map instance.
        /// <para>Parameters: mapName, instanceId, monsterName (optional, if empty clears all)</para>
        /// <para>Example: MONCLEAR BichonProvince 1</para>
        /// <para>Example: MONCLEAR BichonProvince 1 Zombie1</para>
        /// </summary>
        MonClear,
        
        /// <summary>
        /// Teleports all group members to the player's current location.
        /// <para>Parameters: none</para>
        /// <para>Example: GROUPRECALL</para>
        /// </summary>
        GroupRecall,
        
        /// <summary>
        /// Teleports all group members to a specified map and coordinates.
        /// <para>Parameters: mapName, instanceId (optional, defaults to 1), x (optional, defaults to 0), y (optional, defaults to 0)</para>
        /// <para>Example: GROUPTELEPORT BichonProvince 1 100 100</para>
        /// <para>Example: GROUPTELEPORT BichonProvince (uses random location if x/y are 0)</para>
        /// </summary>
        GroupTeleport,
        
        /// <summary>
        /// Delays execution and then jumps to a label after specified seconds.
        /// <para>Parameters: seconds, labelName</para>
        /// <para>Example: DELAYGOTO 5 next_page</para>
        /// </summary>
        DelayGoto,
        
        /// <summary>
        /// Moves a value into a variable (R0-R9, M0-M9, etc.).
        /// <para>Parameters: variableName, value (can use quotes for strings)</para>
        /// <para>Example: MOV R1 1</para>
        /// <para>Example: MOV M1 "Hello"</para>
        /// </summary>
        Mov,
        
        /// <summary>
        /// Performs a calculation and stores the result in a variable.
        /// <para>Parameters: leftValue, operator (+ - * /), rightValue, variableName</para>
        /// <para>Example: CALC %R1 + 1 R1</para>
        /// <para>Example: CALC 10 * 5 R2</para>
        /// </summary>
        Calc,
        
        /// <summary>
        /// Applies a buff to the player.
        /// <para>Parameters: buffType, duration (seconds), visible (true/false, optional), infinite (true/false, optional), stackable (true/false, optional)</para>
        /// <para>Example: GIVEBUFF Poison 60 true false false</para>
        /// </summary>
        GiveBuff,
        
        /// <summary>
        /// Removes a buff from the player.
        /// <para>Parameters: buffType</para>
        /// <para>Example: REMOVEBUFF Poison</para>
        /// </summary>
        RemoveBuff,
        
        /// <summary>
        /// Sends a guild invitation to the player.
        /// <para>Parameters: guildName</para>
        /// <para>Example: ADDTOGUILD "MyGuild"</para>
        /// </summary>
        AddToGuild,
        
        /// <summary>
        /// Removes the player from their current guild.
        /// <para>Parameters: none</para>
        /// <para>Example: REMOVEFROMGUILD</para>
        /// </summary>
        RemoveFromGuild,
        
        /// <summary>
        /// Refreshes the player's level effects display.
        /// <para>Parameters: none</para>
        /// <para>Example: REFRESHEFFECTS</para>
        /// </summary>
        RefreshEffects,
        
        /// <summary>
        /// Changes the player's hair style (0-9). If no parameter, randomizes.
        /// <para>Parameters: hairStyle (optional, 0-9)</para>
        /// <para>Example: CHANGEHAIR 5</para>
        /// <para>Example: CHANGEHAIR (random)</para>
        /// </summary>
        ChangeHair,
        
        /// <summary>
        /// Sets whether the player can gain experience.
        /// <para>Parameters: true/false</para>
        /// <para>Example: CANGAINEXP false</para>
        /// </summary>
        CanGainExp,
        
        /// <summary>
        /// Composes a mail message (must be followed by AddMailItem/AddMailGold and SendMail).
        /// <para>Parameters: message (in quotes), senderName</para>
        /// <para>Example: COMPOSEMAIL "Your reward is ready!" "QuestMaster"</para>
        /// </summary>
        ComposeMail,
        
        /// <summary>
        /// Adds an item to the composed mail.
        /// <para>Parameters: itemName, count (optional, defaults to 1)</para>
        /// <para>Example: ADDMAILITEM IronSword 1</para>
        /// </summary>
        AddMailItem,
        
        /// <summary>
        /// Adds gold to the composed mail.
        /// <para>Parameters: amount</para>
        /// <para>Example: ADDMAILGOLD 10000</para>
        /// </summary>
        AddMailGold,
        
        /// <summary>
        /// Sends the composed mail to the player.
        /// <para>Parameters: none</para>
        /// <para>Example: SENDMAIL</para>
        /// </summary>
        SendMail,
        
        /// <summary>
        /// Makes all group members jump to a specific label in their NPC interaction.
        /// <para>Parameters: labelName</para>
        /// <para>Example: GROUPGOTO reward_page</para>
        /// </summary>
        GroupGoto,
        
        /// <summary>
        /// Teleports the player to the map and coordinates stored from a previous Move action.
        /// <para>Parameters: none</para>
        /// <para>Example: ENTERMAP</para>
        /// </summary>
        EnterMap,
        
        /// <summary>
        /// Gives pearls (intelligent creature currency) to the player.
        /// <para>Parameters: amount</para>
        /// <para>Example: GIVEPEARLS 50</para>
        /// </summary>
        GivePearls,
        
        /// <summary>
        /// Takes pearls from the player.
        /// <para>Parameters: amount</para>
        /// <para>Example: TAKEPEARLS 25</para>
        /// </summary>
        TakePearls,
        
        /// <summary>
        /// Creates a wedding ring for the player (requires marriage partner).
        /// <para>Parameters: none</para>
        /// <para>Example: MAKEWEDDINGRING</para>
        /// </summary>
        MakeWeddingRing,
        
        /// <summary>
        /// Forces a divorce between the player and their partner.
        /// <para>Parameters: none</para>
        /// <para>Example: FORCEDIVORCE</para>
        /// </summary>
        ForceDivorce,
        
        /// <summary>
        /// Broadcasts a message to all players on the server.
        /// <para>Parameters: message (in quotes), chatType</para>
        /// <para>Example: GLOBALMESSAGE "Server maintenance in 10 minutes!" System</para>
        /// </summary>
        GlobalMessage,
        
        /// <summary>
        /// Loads a value from an INI file into a variable.
        /// <para>Parameters: variableName, filePath (in quotes), header, key</para>
        /// <para>Example: LOADVALUE R1 "Settings.ini" General MaxLevel</para>
        /// </summary>
        LoadValue,
        
        /// <summary>
        /// Saves a value to an INI file.
        /// <para>Parameters: filePath (in quotes), header, key, value (in quotes if string)</para>
        /// <para>Example: SAVEVALUE "Settings.ini" General MaxLevel "100"</para>
        /// </summary>
        SaveValue,
        
        /// <summary>
        /// Removes a specific pet by name from the player.
        /// <para>Parameters: monsterName</para>
        /// <para>Example: REMOVEPET Hen</para>
        /// </summary>
        RemovePet,
        
        /// <summary>
        /// Repairs a conquest guard/archer (requires guild leader or GM).
        /// <para>Parameters: conquestIndex, guardIndex</para>
        /// <para>Example: CONQUESTGUARD 1 5</para>
        /// </summary>
        ConquestGuard,
        
        /// <summary>
        /// Repairs a conquest gate (requires guild leader or GM).
        /// <para>Parameters: conquestIndex, gateIndex</para>
        /// <para>Example: CONQUESTGATE 1 2</para>
        /// </summary>
        ConquestGate,
        
        /// <summary>
        /// Repairs a conquest wall (requires guild leader or GM).
        /// <para>Parameters: conquestIndex, wallIndex</para>
        /// <para>Example: CONQUESTWALL 1 3</para>
        /// </summary>
        ConquestWall,
        
        /// <summary>
        /// Repairs a conquest siege weapon (requires guild leader or GM).
        /// <para>Parameters: conquestIndex, siegeIndex</para>
        /// <para>Example: CONQUESTSIEGE 1 1</para>
        /// </summary>
        ConquestSiege,
        
        /// <summary>
        /// Transfers gold from conquest storage to guild storage (requires guild leader).
        /// <para>Parameters: conquestIndex</para>
        /// <para>Example: TAKECONQUESTGOLD 1</para>
        /// </summary>
        TakeConquestGold,
        
        /// <summary>
        /// Sets the NPC rate for a conquest (requires guild leader).
        /// <para>Parameters: conquestIndex, rate (0-255)</para>
        /// <para>Example: SETCONQUSTRATE 1 100</para>
        /// </summary>
        SetConquestRate,
        
        /// <summary>
        /// Starts or stops a conquest war.
        /// <para>Parameters: conquestIndex</para>
        /// <para>Example: STARTCONQUEST 1</para>
        /// </summary>
        StartConquest,
        
        /// <summary>
        /// Schedules a conquest attack (sets attacker guild).
        /// <para>Parameters: conquestIndex</para>
        /// <para>Example: SCHEDULECONQUEST 1</para>
        /// </summary>
        ScheduleConquest,
        
        /// <summary>
        /// Opens a conquest gate.
        /// <para>Parameters: conquestIndex, gateIndex</para>
        /// <para>Example: OPENGATE 1 2</para>
        /// </summary>
        OpenGate,
        
        /// <summary>
        /// Closes a conquest gate.
        /// <para>Parameters: conquestIndex, gateIndex</para>
        /// <para>Example: CLOSEGATE 1 2</para>
        /// </summary>
        CloseGate,
        
        /// <summary>
        /// Breaks out of the current segment execution (stops processing remaining actions).
        /// <para>Parameters: none</para>
        /// <para>Example: BREAK</para>
        /// </summary>
        Break,
        
        /// <summary>
        /// Adds the player's guild name to a guild name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes)</para>
        /// <para>Example: ADDGUILDNAMELIST "Conquest/Winners"</para>
        /// </summary>
        AddGuildNameList,
        
        /// <summary>
        /// Removes the player's guild name from a guild name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes)</para>
        /// <para>Example: DELGUILDNAMELIST "Conquest/Winners"</para>
        /// </summary>
        DelGuildNameList,
        
        /// <summary>
        /// Clears all guild names from a guild name list file.
        /// <para>Parameters: filePath (relative to NameListPath, can use quotes)</para>
        /// <para>Example: CLEARGUILDNAMELIST "Conquest/Winners"</para>
        /// </summary>
        ClearGuildNameList,
        
        /// <summary>
        /// Opens a web browser with the specified URL for the player.
        /// <para>Parameters: url</para>
        /// <para>Example: OPENBROWSER https://example.com</para>
        /// </summary>
        OpenBrowser,
        
        /// <summary>
        /// Loads a random line from a text file into a variable.
        /// <para>Parameters: fileName (relative to NPCPath), variableName</para>
        /// <para>Example: GETRANDOMTEXT "Quotes.txt" M1</para>
        /// </summary>
        GetRandomText,
        
        /// <summary>
        /// Plays a sound effect for the player.
        /// <para>Parameters: soundID</para>
        /// <para>Example: PLAYSOUND 100</para>
        /// </summary>
        PlaySound,
        
        /// <summary>
        /// Sets a timer for the player or globally.
        /// <para>Parameters: timerName, seconds, timerType, global (true/false, optional)</para>
        /// <para>Example: SETTIMER QuestTimer 300 0 false</para>
        /// </summary>
        SetTimer,
        
        /// <summary>
        /// Expires/removes a timer.
        /// <para>Parameters: timerName</para>
        /// <para>Example: EXPIRETIMER QuestTimer</para>
        /// </summary>
        ExpireTimer,
        
        /// <summary>
        /// Unequips an item from the player (or all items if no slot specified).
        /// <para>Parameters: slotName (optional, e.g., "Weapon", "Armour")</para>
        /// <para>Example: UNEQUIPITEM Weapon</para>
        /// <para>Example: UNEQUIPITEM (unequips all)</para>
        /// </summary>
        UnequipItem,
        
        /// <summary>
        /// Rolls a die (1-6) and stores result in NPCRollResult.
        /// <para>Parameters: pageName, autoRoll (true/false)</para>
        /// <para>Example: ROLLDIE roll_result_page false</para>
        /// </summary>
        RollDie,
        
        /// <summary>
        /// Rolls Yut sticks and stores result in NPCRollResult.
        /// <para>Parameters: pageName, autoRoll (true/false)</para>
        /// <para>Example: ROLLYUT yut_result_page true</para>
        /// </summary>
        RollYut,
        
        /// <summary>
        /// Drops items/gold based on a drop configuration file.
        /// <para>Parameters: dropFilePath (relative to DropPath)</para>
        /// <para>Example: DROP "QuestRewards/Quest1"</para>
        /// </summary>
        Drop,
        
        /// <summary>
        /// Revives the player's hero.
        /// <para>Parameters: none</para>
        /// <para>Example: REVIVEHERO</para>
        /// </summary>
        ReviveHero,
        
        /// <summary>
        /// Seals the player's hero (stores hero).
        /// <para>Parameters: none</para>
        /// <para>Example: SEALHERO</para>
        /// </summary>
        SealHero,
        
        /// <summary>
        /// Deletes the player's hero permanently.
        /// <para>Parameters: none</para>
        /// <para>Example: DELETEHERO</para>
        /// </summary>
        DeleteHero,
        
        /// <summary>
        /// Repairs all conquest structures (gates, walls, guards, sieges) - GM only.
        /// <para>Parameters: conquestIndex</para>
        /// <para>Example: CONQUESTREPAIRALL 1</para>
        /// </summary>
        ConquestRepairAll,
        
        /// <summary>
        /// Purchases a Guild Territory (GT) for the guild (requires guild leader).
        /// <para>Parameters: none</para>
        /// <para>Example: BUYGT</para>
        /// </summary>
        BuyGT,
        
        /// <summary>
        /// Teleports the player to their guild's territory (requires guild with GT).
        /// <para>Parameters: none</para>
        /// <para>Example: TELEPORTGT</para>
        /// </summary>
        TeleportGT,
        
        /// <summary>
        /// Extends the guild territory rental period (requires guild leader).
        /// <para>Parameters: none</para>
        /// <para>Example: EXTENDGT</para>
        /// </summary>
        ExtendGT,
        
        /// <summary>
        /// Recalls all online guild members to the player's location (requires guild leader).
        /// <para>Parameters: none</para>
        /// <para>Example: GTALLRECALL</para>
        /// </summary>
        GTAllRecall,
        
        /// <summary>
        /// Recalls a specific guild member by name to the player's location (requires guild leader).
        /// <para>Parameters: playerName</para>
        /// <para>Example: GTRECALL PlayerName</para>
        /// </summary>
        GTRecall,
        
        /// <summary>
        /// Displays the remaining rental days for the guild territory.
        /// <para>Parameters: none</para>
        /// <para>Example: DISPLAYGTRENTALDAYS</para>
        /// </summary>
        DisplayGTRentalDays,
        
        /// <summary>
        /// Lists the guild territory for sale (requires guild leader, minimum 2,000,000 gold).
        /// <para>Parameters: salePrice</para>
        /// <para>Example: GTSALE 5000000</para>
        /// </summary>
        GTSale,
        
        /// <summary>
        /// Cancels the guild territory sale (requires guild leader).
        /// <para>Parameters: none</para>
        /// <para>Example: CANCELGTSALE</para>
        /// </summary>
        GTCancelSale,
        
        /// <summary>
        /// Gives a skill/spell to the player's hero.
        /// <para>Parameters: spellName, level (optional, defaults to 0, max 3)</para>
        /// <para>Example: HEROGIVESKILL FireBall 1</para>
        /// </summary>
        HeroGiveSkill,
        
        /// <summary>
        /// Removes a skill/spell from the player's hero.
        /// <para>Parameters: spellName</para>
        /// <para>Example: HEROREMOVESKILL FireBall</para>
        /// </summary>
        HeroRemoveSkill,
    }
}