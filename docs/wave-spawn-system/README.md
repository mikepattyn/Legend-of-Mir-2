# Wave Spawn System

## Overview

The Wave Spawn System is an advanced monster spawning and round-based gameplay system designed for Legend of Mir. It provides comprehensive control over monster spawning patterns, round progression, player tracking, and reward distribution. Unlike the traditional NPC-based spawning system, this system offers a dedicated management interface and more sophisticated control mechanisms.

## System Architecture

The Wave Spawn System consists of several key components:

### Core Components

1. **WaveSpawnSystem** (`Server.MirObjects.WaveSpawnSystem`)
   - Main manager class that handles all active wave instances
   - Processes all active waves in the main server loop
   - Manages starting, stopping, and tracking wave instances

2. **WaveSpawnInstance** (`Server.MirObjects.WaveSpawnInstance`)
   - Represents an active wave session
   - Tracks players, monsters, round progression, and state
   - Handles spawning logic, completion checks, and reward distribution

3. **WaveSpawnInfo** (`Server.MirDatabase.WaveSpawnInfo`)
   - Configuration data structure for wave definitions
   - Contains rounds, rewards, timer settings, and completion conditions
   - Persisted to database via binary serialization

4. **WaveSpawnSystemForm** (`Server.MirForms.Systems.WaveSpawnSystemForm`)
   - Windows Forms management interface
   - Provides UI for creating, editing, and monitoring waves
   - Accessible via the server admin menu: **Systems → Wave Spawn System**

## How It Works

### Wave Lifecycle

1. **Initialization**
   - A player triggers a wave start (typically via NPC interaction or admin command)
   - System creates a `WaveSpawnInstance` with the selected `WaveSpawnInfo`
   - Instance is added to the active instances list

2. **Round Progression**
   - Each wave consists of multiple rounds
   - Rounds are processed sequentially
   - Each round can have multiple spawn patterns
   - Rounds can have different maps and instance IDs

3. **Spawning Process**
   - Spawn patterns define which monsters spawn and how
   - Two spawn types:
     - **AllAtOnce**: All monsters spawn simultaneously
     - **Staggered**: Monsters spawn with delays between each spawn
   - Spawn locations can be:
     - Exact coordinates (from `SpawnLocations` list)
     - Area-based (using `SpawnCenter` and `SpawnRadius`)

4. **Completion Checking**
   - System periodically checks if round completion conditions are met
   - Default: All monsters must be killed (`RequireAllMonstersKilled = true`)
   - Can be configured to auto-advance rounds or wait for manual advancement

5. **Reward Distribution**
   - Rewards can be given per round or upon wave completion
   - Rewards include: Gold, Experience, and Items
   - Distributed to all participating players

6. **Termination**
   - Wave completes when all rounds are finished
   - All spawned monsters are cleaned up
   - Instance is removed from active instances

### State Management

The system uses a state machine with the following states:

- **Waiting**: Initial state, waiting to start
- **Starting**: Wave is being initialized
- **Spawning**: Currently spawning monsters for the round
- **InProgress**: Round is active, monsters are present
- **WaitingForAdvance**: Round complete, waiting for next round (if not auto-advance)
- **Completed**: All rounds finished successfully
- **Stopped**: Manually stopped by admin
- **Error**: An error occurred during processing

## Configuration

### Wave Configuration

Each wave (`WaveSpawnInfo`) contains:

- **Basic Information**
  - `Name`: Display name for the wave
  - `Description`: Optional description
  - `MapIndex`: Default map for the wave (must be selected)
  - `InstanceId`: Instance ID (automatically generated when `UseInstances` is enabled)
  - `UseInstances`: Whether to use dynamic map instances (creates unique instances automatically)

- **Timer Settings**
  - `RoundDuration`: Duration of each round in seconds (0 = no time limit)
  - `SpawnDelay`: Delay between spawn patterns in milliseconds
  - `RoundStartDelay`: Delay before starting next round in milliseconds
  - `CompletionCheckInterval`: How often to check for completion in milliseconds

- **Completion Conditions**
  - `RequireAllMonstersKilled`: Must all monsters be killed to complete round
  - `AutoAdvanceRounds`: Automatically advance to next round when complete
  - `AllowMultiplePlayers`: Allow multiple players to join the wave

### Round Configuration

Each round (`WaveRound`) contains:

- **Basic Information**
  - `RoundNumber`: Sequential round number
  - `Name`: Round name
  - `MapIndex`: Map for this round (can differ from wave default)
  - `InstanceId`: Instance ID for this round

- **Spawn Configuration**
  - `SpawnPatterns`: List of spawn patterns for this round
  - `SpawnLocations`: List of exact spawn coordinates
  - `SpawnCenter`: Center point for area-based spawning
  - `SpawnRadius`: Radius for area-based spawning (0 = use exact locations)

### Spawn Location Modes

The location box (Spawn Center X/Y and Spawn Radius) in the round editor configures area-based spawning. The system supports two spawn modes:

**Area-based spawning (Spawn Center + Radius):**
- Set `SpawnRadius > 0` and a valid `SpawnCenter`
- Monsters spawn randomly within the radius around the center point
- Useful for flexible, varied spawns in a region

**Exact coordinate spawning (Spawn Locations list):**
- Set `SpawnRadius = 0` and add coordinates to `SpawnLocations`
- Monsters spawn at specific coordinates
- Useful for precise, controlled spawns

**Spawning logic prioritization:**
1. If `SpawnRadius > 0` → uses area-based spawning
2. Else if `SpawnLocations` has entries → uses exact coordinates
3. Else → falls back to random walkable cells

The location box is needed for area-based spawning. If you only use exact coordinates, you can leave it empty (or set Radius to 0).

### Spawn Pattern Configuration

Each spawn pattern (`SpawnPattern`) contains:

- `MonsterIndex`: Index of the monster to spawn
- `Count`: Number of monsters to spawn
- `SpawnDelay`: Delay before this pattern spawns (milliseconds)
- `Type`: Spawn type (`AllAtOnce` or `Staggered`)
- `StaggerDelay`: Delay between individual spawns when using `Staggered` type

### Reward Configuration

Each reward (`WaveReward`) contains:

- `RoundNumber`: Round number (0 = completion reward)
- `Gold`: Gold amount to give
- `Experience`: Experience amount to give
- `Items`: List of items to give (`RewardItem`)

Each reward item contains:
- `ItemIndex`: Index of the item
- `Count`: Number of items
- `Durability`: Item durability (0 = max durability)

## Features

### Advanced Spawning Control

- **Multiple Spawn Patterns**: Each round can have multiple spawn patterns with different delays
- **Spawn Types**: Choose between instant spawning or staggered spawning
- **Flexible Locations**: Use exact coordinates or area-based spawning with radius
- **Per-Round Maps**: Each round can use a different map

### Round Management

- **Sequential Rounds**: Rounds progress in order
- **Time Limits**: Optional time limits per round
- **Auto-Advance**: Automatically advance rounds when complete
- **Manual Control**: Admin can manually start/stop waves

### Player Management

- **Player Tracking**: System tracks all players in a wave
- **Multi-Player Support**: Optional support for multiple players
- **Player Messages**: System broadcasts messages to players

### Reward System

- **Per-Round Rewards**: Give rewards after each round
- **Completion Rewards**: Special rewards when wave completes
- **Multiple Reward Types**: Gold, experience, and items
- **Automatic Distribution**: Rewards automatically given to all players

### Monitoring & Management

- **Active Waves View**: See all currently active waves
- **Real-Time Status**: View wave state, current round, player count, monster count
- **Admin Controls**: Start and stop waves from the management interface

## Usage Instructions

### Creating a Wave

1. Open the Wave Spawn System form: **Systems → Wave Spawn System**
2. Navigate to the **Wave Configurations** tab
3. Click **Add Wave**
4. Configure the wave:
   - Set name and description
   - Select the default map
   - Configure timer settings
   - Set completion conditions
5. Click **Save Wave**

### Creating Rounds

1. Select a wave from the list
2. Navigate to the **Round Editor** tab
3. Click **Add Round**
4. Configure the round:
   - Set round number and name
   - Select map (if different from wave default)
   - Set spawn center and radius (for area-based spawning)
   - Add spawn locations (for exact coordinate spawning)
5. Add spawn patterns:
   - Click **Add Pattern**
   - Select monster type
   - Set count and spawn delay
   - Choose spawn type (AllAtOnce or Staggered)
   - Set stagger delay if using Staggered type
6. Click **Save Round**

### Creating Rewards

1. Navigate to the **Reward Manager** tab
2. Click **Add Reward**
3. Configure the reward:
   - Set round number (0 for completion reward)
   - Set gold and experience amounts
   - Add items if needed
4. Click **Save Reward**

### Starting a Wave

1. Navigate to the **Active Waves** tab
2. Select a wave configuration from the dropdown next to the **Start Wave** button
3. Click **Start Wave**
4. The wave will begin spawning monsters according to its configuration

### Monitoring Active Waves

The **Active Waves** tab displays:
- Wave name
- Current state
- Current round number
- Number of players
- Number of active monsters

### Stopping a Wave

1. Navigate to the **Active Waves** tab
2. Select an active wave
3. Click **Stop Wave**
4. All spawned monsters will be cleaned up

### Saving Configuration

After making changes, click **Save DB** to persist all configurations to the database. The form will also automatically save all changes when you close it, ensuring your wave configurations and rounds are persisted across server restarts.

**Note:** Always click **Save DB** after making significant changes, or simply close the form to automatically save. Your configurations will be loaded automatically when the server starts.

## Technical Details

### Dynamic Map Instancing

When `UseInstances` is enabled, the system automatically creates unique map instances in memory for each wave. This allows:

- **Multiple simultaneous waves**: Multiple players can run the same wave configuration at the same time without conflicts
- **No file copying required**: Map instances are created dynamically from the base map file - no need to copy map files multiple times
- **Automatic cleanup**: Map instances are automatically removed when waves complete or are stopped
- **Unique instance IDs**: Each wave instance gets a unique identifier generated from: `StartTime + PlayerID + Random` (Option B strategy)

**How it works:**
1. When a wave starts with `UseInstances = true`, the system creates a new `Map` object from the base `MapInfo`
2. The map instance is added to `Envir.MapList` for tracking
3. Players are teleported to this unique instance
4. When the wave completes or is stopped, the instance is cleaned up and removed from memory

**Round-specific maps:**
- If a round specifies a different `MapIndex`, a new instance is created for that round
- The previous round's instance is cleaned up automatically
- If a round uses the same map, the existing instance is reused

### Integration Points

- **Envir Integration**: The system is integrated into `Envir.Process()` loop
- **Database Persistence**: Wave configurations are saved/loaded via binary serialization
- **Map System**: Uses dynamic map instance creation via `MapInfo.CreateMapInstance()`
- **Monster System**: Spawns monsters using `MonsterObject.GetMonster()`

### Processing Loop

The system processes in the main server loop:

```csharp
// In Envir.Process()
if (Envir.Time >= waveSpawnTime)
{
    WaveSpawnSystem.Process();
    waveSpawnTime = Envir.Time + 1000; // Process every second
}
```

### Spawn Location Selection

When spawning monsters:
1. If `SpawnRadius > 0`: Uses area-based spawning around `SpawnCenter`
2. If `SpawnRadius == 0`: Uses exact coordinates from `SpawnLocations` list
3. If multiple monsters spawn, locations are distributed evenly

### Staggered Spawning

When using `Staggered` spawn type:
- Monsters are added to a `PendingSpawns` queue
- Each spawn has a calculated `SpawnTime`
- The system processes pending spawns each tick
- Monsters spawn individually with the specified delay

### Completion Checking

The system checks for round completion:
- Every `CompletionCheckInterval` milliseconds
- Checks if `RequireAllMonstersKilled` condition is met
- If complete and `AutoAdvanceRounds` is true, advances to next round
- Distributes round rewards if configured

## Best Practices

1. **Round Design**
   - Start with easier rounds and increase difficulty
   - Use staggered spawning for dramatic effect
   - Balance monster count with player capabilities

2. **Reward Balancing**
   - Give incremental rewards per round
   - Make completion rewards substantial
   - Consider item rewards for special achievements

3. **Timer Settings**
   - Set reasonable round durations
   - Use spawn delays to create pacing
   - Adjust completion check interval based on monster count

4. **Testing**
   - Test waves with different player counts
   - Verify spawn locations are accessible
   - Check reward distribution works correctly

## Troubleshooting

### Monsters Not Spawning
- Check spawn locations are valid coordinates
- Verify monster index exists in database
- Check map and instance settings

### Round Not Completing
- Verify `RequireAllMonstersKilled` setting
- Check if monsters are being killed
- Ensure completion check interval is reasonable

### Rewards Not Given
- Verify reward configuration
- Check round number matches
- Ensure players are still in the wave

### Wave Not Starting
- Check wave configuration is saved
- Verify map exists and is selected in the wave configuration
- Check player permissions
- If using instances, ensure `UseInstances` is checked (instances are created automatically)

## Future Enhancements

Potential improvements for the system:
- Leaderboard tracking
- Wave statistics and analytics
- Conditional spawn patterns based on player count
- Dynamic difficulty adjustment
- Wave templates and presets
- Integration with quest system

### Instance Management Enhancements

#### 1. Instance Pooling for Performance (Reuse Instances)

**Goal:** Improve performance by reusing map instances instead of creating new ones for each wave.

**Benefits:**
- Reduced memory allocation overhead
- Faster wave startup times
- Lower garbage collection pressure
- Better scalability for high-frequency wave usage

**Implementation Plan:**
- Create an `InstancePool` class to manage available map instances
- Maintain a pool of pre-initialized map instances per map type
- When a wave starts, check the pool for an available instance
- If available, reuse it; otherwise create a new one
- After wave completion, return the instance to the pool instead of destroying it
- Implement pool size limits (min/max instances per map)
- Add configuration for pool size per map type
- Ensure proper cleanup/reset of instances before reuse (clear monsters, reset state)

**Technical Considerations:**
- Need to reset map state (monsters, objects) before reuse
- Handle edge cases where instance is in use when pool is checked
- Consider thread safety if multiple waves start simultaneously
- Monitor pool hit/miss rates for optimization

#### 2. Instance Expiration Timer (Clean Up Abandoned Instances)

**Goal:** Automatically clean up map instances that are no longer in use or have been abandoned.

**Benefits:**
- Prevent memory leaks from abandoned instances
- Automatic resource cleanup
- Better resource management
- Handle edge cases where players disconnect mid-wave

**Implementation Plan:**
- Add `LastActivityTime` tracking to each instance
- Implement a background cleanup process that runs periodically
- Define configurable expiration timeout (e.g., 30 minutes of inactivity)
- When expiration timer triggers:
  - Check if instance has active players
  - If no players and timeout exceeded, mark for cleanup
  - Gracefully stop any active waves in the instance
  - Remove instance from active list and pool
- Add configuration for expiration timeout
- Log cleanup events for monitoring
- Consider different timeouts for different wave types

**Technical Considerations:**
- Ensure cleanup doesn't interrupt active gameplay
- Handle race conditions between cleanup and wave start
- Integrate with instance pooling (return to pool vs. destroy)
- Add admin override to prevent cleanup if needed

#### 3. Instance Monitoring/Statistics in Admin UI

**Goal:** Provide administrators with visibility into instance usage, performance, and health metrics.

**Benefits:**
- Better system observability
- Performance optimization insights
- Resource usage monitoring
- Troubleshooting capabilities

**Implementation Plan:**
- Add new tab or section in `WaveSpawnSystemForm` for instance monitoring
- Display real-time statistics:
  - Total active instances
  - Instances per map type
  - Average instance lifetime
  - Pool utilization (if pooling implemented)
  - Instance creation/destruction rates
  - Active players per instance
  - Memory usage per instance (if measurable)
- Add instance list view showing:
  - Instance ID
  - Associated map
  - Creation time
  - Last activity time
  - Current player count
  - Active wave (if any)
  - Status (active, idle, expired)
- Implement refresh mechanism for real-time updates
- Add filtering and sorting capabilities
- Export statistics to log file or database
- Add visual indicators for health status (e.g., color coding)

**Technical Considerations:**
- Minimize performance impact of statistics collection
- Use efficient data structures for tracking
- Consider caching statistics to reduce UI update frequency
- Ensure thread-safe access to statistics data
- Add configuration to enable/disable detailed tracking
