# Memory Management and GC Optimization Plan
## Issue #1046: Memory Doubling on Server Restart

### Problem Summary
Memory doubles every time the server is restarted (via Control -> Reboot Server or Stop/Start), starting at 2.6GB, then 5.2GB, and continuing to grow to 8-9GB after multiple restarts. Objects are not being released/garbage collected because they're still referenced somewhere in the server application.

### Root Cause Analysis

Based on Visual Studio Diagnostic Tools analysis and codebase review, the primary memory leak sources are:

1. **`SavedSpawns` List Not Cleared** (CRITICAL)
   - `Envir.SavedSpawns` is a `List<MapRespawn>` that is **never cleared** in `StopEnvir()`
   - This list holds references to `MapRespawn` objects that prevent garbage collection
   - Location: `Server\MirEnvir\Envir.cs` line 175

2. **Circular Reference Chain**
   - `SavedSpawns` → `MapRespawn` → `Map` → `Respawns` (which includes the same `MapRespawn`)
   - This prevents the entire object graph from being GC'd when `MapList.Clear()` is called

3. **Large Memory Consumers (from diagnostic snapshot)**
   - `List<Point>` (WalkableCells): ~1.87 GB - stored in both `Map` and `MapRespawn`
   - `Server.MirEnvir.Cell`: 42M+ instances (~1.36 GB) - stored in `Map.Cells[,]`
   - `Server.MirEnvir.Cell[,]` arrays: ~340 MB exclusive, ~1.70 GB inclusive
   - `Server.MirObjects.MonsterObject`: Large inclusive size (~1.13 GB)
   - `LinkedListNode<Server.MirObjects.MapObject>`: ~1.23 GB inclusive
   - `List<Server.MirObjects.MapObject>`: ~1.38 GB inclusive

4. **Missing Cleanup in StopEnvir()**
   - `SavedSpawns` is not cleared
   - Map objects may not be properly disposing of their resources
   - Cell arrays and WalkableCells are not explicitly nullified

---

## Solution Plan

### Phase 1: Critical Fixes (Immediate Priority)

#### 1.1 Clear SavedSpawns in StopEnvir()
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` method (around line 3435)

**Action**: Add `SavedSpawns.Clear()` before or after `MapList.Clear()`

**Rationale**: This breaks the circular reference chain and allows Maps to be GC'd.

**Code Change**:
```csharp
private void StopEnvir()
{
    SaveGoods(true);

    // Clear SavedSpawns FIRST to break circular references
    SavedSpawns.Clear();
    
    MapList.Clear();
    StartPoints.Clear();
    StartItems.Clear();
    Objects.Clear();
    Players.Clear();
    Heroes.Clear();
    GTMapList.Clear();

    CleanUp();

    GC.Collect();
    
    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.EnvirStopped));
}
```

#### 1.2 Nullify MapRespawn References
**File**: `Server\MirEnvir\Map.cs`
**Location**: `MapRespawn` class

**Action**: Add a cleanup method to `MapRespawn` that nullifies references, and call it when clearing SavedSpawns.

**Rationale**: Explicitly breaking references helps GC and makes intent clear.

**Code Change**:
```csharp
public class MapRespawn
{
    // ... existing code ...
    
    public void ClearReferences()
    {
        Map = null;
        WalkableCells?.Clear();
        WalkableCells = null;
        Route?.Clear();
        Route = null;
    }
}
```

Then in `StopEnvir()`:
```csharp
// Clear SavedSpawns and break references
foreach (var spawn in SavedSpawns)
{
    spawn?.ClearReferences();
}
SavedSpawns.Clear();
```

#### 1.3 Clear Map Collections Before Clearing MapList
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` method

**Action**: Iterate through MapList and clear internal collections before clearing MapList.

**Rationale**: Ensures all nested collections are cleared, breaking all reference chains.

**Code Change**:
```csharp
private void StopEnvir()
{
    SaveGoods(true);

    // Clear SavedSpawns FIRST to break circular references
    foreach (var spawn in SavedSpawns)
    {
        spawn?.ClearReferences();
    }
    SavedSpawns.Clear();
    
    // Clear all map internal collections before clearing MapList
    foreach (var map in MapList)
    {
        if (map != null)
        {
            map.Respawns?.Clear();
            map.WalkableCells?.Clear();
            map.NPCs?.Clear();
            map.Players?.Clear();
            map.Spells?.Clear();
            map.Heroes?.Clear();
            map.ActionList?.Clear();
            map.Conquest?.Clear();
            // Nullify large arrays
            map.Cells = null;
            map.DoorIndex = null;
            map.Mine = null;
        }
    }
    
    MapList.Clear();
    StartPoints.Clear();
    StartItems.Clear();
    Objects.Clear();
    Players.Clear();
    Heroes.Clear();
    GTMapList.Clear();

    CleanUp();

    GC.Collect();
    
    MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.EnvirStopped));
}
```

---

### Phase 2: Enhanced Cleanup (High Priority)

#### 2.1 Ensure All MapObjects are Despawned
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` method

**Action**: Before clearing Objects list, ensure all MapObjects are properly despawned.

**Rationale**: MapObjects may have references to Maps, Cells, etc. Proper despawn ensures cleanup.

**Code Change**:
```csharp
// Despawn all objects before clearing
var objectsToDespawn = Objects.ToList(); // Create copy to avoid modification during iteration
foreach (var obj in objectsToDespawn)
{
    try
    {
        obj?.Despawn();
    }
    catch (Exception ex)
    {
        MessageQueue.Enqueue($"Error despawning object {obj?.ObjectID}: {ex.Message}");
    }
}
Objects.Clear();
```

#### 2.2 Clear MobThread Collections
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `Stop()` and `StopEnvir()` methods

**Action**: Clear MobThread ObjectsList collections.

**Rationale**: MobThreads hold references to MapObjects that need to be cleared.

**Code Change**:
```csharp
// In Stop() method, after interrupting threads:
for (var i = 1; i < MobThreading.Length; i++)
{
    if (MobThreads[i] != null)
    {
        MobThreads[i].EndTime = Time + 9999;
        MobThreads[i].ObjectsList?.Clear(); // Add this
    }
    // ... rest of existing code
}
```

#### 2.3 Clear WaveSpawnSystem References
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` method

**Action**: Clear WaveSpawnSystem ActiveInstances and ensure all instances are stopped.

**Rationale**: WaveSpawnSystem holds references to Maps, Players, and Monsters through ActiveInstances that prevent GC.

**Code Change**:
```csharp
// Stop all active wave spawn instances before clearing maps
foreach (var instance in WaveSpawnSystem.ActiveInstances.ToList())
{
    instance?.Stop(); // This will cleanup monsters and maps
}
WaveSpawnSystem.ActiveInstances.Clear();
```

**Note**: `WaveSpawnInstance.Stop()` already handles cleanup of monsters and dynamic map instances, but we need to ensure all instances are stopped and the list is cleared.

---

### Phase 3: Memory Optimization (Medium Priority)

#### 3.1 Implement IDisposable for Map
**File**: `Server\MirEnvir\Map.cs`

**Action**: Implement IDisposable pattern for Map class to ensure proper cleanup.

**Rationale**: Makes cleanup explicit and ensures resources are released.

**Code Change**:
```csharp
public class Map : IDisposable
{
    // ... existing code ...
    
    private bool _disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Clear managed resources
                Respawns?.Clear();
                WalkableCells?.Clear();
                NPCs?.Clear();
                Players?.Clear();
                Spells?.Clear();
                Heroes?.Clear();
                ActionList?.Clear();
                Conquest?.Clear();
                Doors?.Clear();
                
                // Nullify large arrays
                Cells = null;
                DoorIndex = null;
                Mine = null;
            }
            
            _disposed = true;
        }
    }
}
```

Then in `StopEnvir()`:
```csharp
foreach (var map in MapList)
{
    (map as IDisposable)?.Dispose();
}
MapList.Clear();
```

#### 3.2 Optimize Cell Object Management
**File**: `Server\MirEnvir\Map.cs`
**Location**: `Cell` class

**Action**: Ensure Cell.Objects list is cleared when Map is disposed.

**Rationale**: Cells hold references to MapObjects that need to be cleared.

**Note**: This is already handled if we clear MapObjects before clearing Maps, but explicit cleanup is better.

#### 3.3 Consider Object Pooling for Frequently Created Objects
**Files**: Various

**Action**: Evaluate if object pooling would help for:
- `Cell` objects (42M+ instances)
- `Point` objects in WalkableCells
- Frequently spawned/despawned MapObjects

**Rationale**: Reduces GC pressure and memory allocations.

**Status**: **Future Enhancement** - Requires performance profiling to determine if beneficial.

---

### Phase 4: GC Optimization (Low Priority)

#### 4.1 Force Multiple GC Collections
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` method

**Action**: After cleanup, force multiple GC collections with compaction.

**Rationale**: Ensures all unreferenced objects are collected and memory is compacted.

**Code Change**:
```csharp
CleanUp();

// Force multiple GC collections to ensure cleanup
GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
GC.WaitForPendingFinalizers();
GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);

MessageQueue.Enqueue(GameLanguage.ServerTextMap.GetLocalization(ServerTextKeys.EnvirStopped));
```

#### 4.2 Add Memory Usage Logging
**File**: `Server\MirEnvir\Envir.cs`
**Location**: `StopEnvir()` and `StartEnvir()` methods

**Action**: Log memory usage before and after cleanup to monitor improvements.

**Rationale**: Helps verify that fixes are working and identify any remaining issues.

**Code Change**:
```csharp
private void StopEnvir()
{
    var memoryBefore = GC.GetTotalMemory(false);
    MessageQueue.Enqueue($"Memory before cleanup: {memoryBefore / 1024 / 1024} MB");
    
    // ... cleanup code ...
    
    var memoryAfter = GC.GetTotalMemory(true);
    MessageQueue.Enqueue($"Memory after cleanup: {memoryAfter / 1024 / 1024} MB");
    MessageQueue.Enqueue($"Memory freed: {(memoryBefore - memoryAfter) / 1024 / 1024} MB");
}
```

---

## Implementation Checklist

### Immediate (Phase 1)
- [ ] **1.1** Add `SavedSpawns.Clear()` to `StopEnvir()`
- [ ] **1.2** Add `ClearReferences()` method to `MapRespawn` class
- [ ] **1.3** Clear Map internal collections before clearing MapList

### High Priority (Phase 2)
- [ ] **2.1** Ensure all MapObjects are despawned before clearing Objects
- [ ] **2.2** Clear MobThread ObjectsList collections
- [ ] **2.3** Stop all WaveSpawnSystem ActiveInstances and clear the list

### Medium Priority (Phase 3)
- [ ] **3.1** Implement IDisposable for Map class
- [ ] **3.2** Verify Cell.Objects cleanup
- [ ] **3.3** Evaluate object pooling (future enhancement)

### Low Priority (Phase 4)
- [ ] **4.1** Force multiple GC collections after cleanup
- [ ] **4.2** Add memory usage logging

---

## Testing Plan

1. **Baseline Test**
   - Start server, note initial memory usage
   - Restart server (Stop/Start), note memory after restart
   - Repeat 3-5 times, verify memory doesn't double

2. **Memory Snapshot Comparison**
   - Take VS Diagnostic Tools snapshot before restart
   - Take snapshot after restart
   - Compare object counts and heap sizes
   - Verify SavedSpawns, MapRespawn, Map, Cell objects are released

3. **Stress Test**
   - Run server for extended period
   - Perform multiple restarts
   - Monitor memory growth over time
   - Verify no memory leaks during normal operation

4. **Functional Test**
   - Verify server functionality after restart
   - Ensure maps load correctly
   - Verify respawns work correctly
   - Test SavedSpawns functionality (if applicable)

---

## Expected Outcomes

After implementing Phase 1 fixes:
- Memory should **not** double on restart
- Memory usage should remain stable across multiple restarts
- VS Diagnostic Tools should show:
  - `SavedSpawns` count = 0 after restart
  - `MapRespawn` objects released
  - `Map` objects released
  - `Cell` arrays released
  - `WalkableCells` (List<Point>) released

After implementing all phases:
- Memory usage should be consistent across restarts
- GC should efficiently collect unreferenced objects
- Server should maintain stable memory footprint

---

## Risk Assessment

**Low Risk Changes:**
- Clearing collections (Phase 1.1, 1.3)
- Adding ClearReferences() method (Phase 1.2)
- Memory logging (Phase 4.2)

**Medium Risk Changes:**
- Despawning all objects (Phase 2.1) - Need to ensure no side effects
- Clearing MobThread collections (Phase 2.2) - Need to verify thread safety
- IDisposable implementation (Phase 3.1) - Need to ensure proper usage

**Mitigation:**
- Test each phase incrementally
- Use memory profiling tools to verify fixes
- Monitor server functionality after each change
- Keep backups before major changes

---

## Notes

- The `Reboot()` method calls `Stop()` then `Start()`, which calls `StopEnvir()` and `StartEnvir()`
- `StopEnvir()` is where cleanup should happen
- `SavedSpawns` is populated in `Map.cs` line 487 when `SaveRespawnTime` is true
- `SavedSpawns` is used during map loading to restore respawn timers (see `Envir.cs` around line 3063)

---

## References

- GitHub Issue: #1046
- VS Diagnostic Tools snapshot analysis
- Code locations:
  - `Server\MirEnvir\Envir.cs` - Main environment and lifecycle management
  - `Server\MirEnvir\Map.cs` - Map and MapRespawn classes
  - `Server\MirObjects\MapObject.cs` - Base object class with Spawned/Despawn methods
