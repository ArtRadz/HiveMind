using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] private int? resourceCounter; // Lower means closer to resource.
    [SerializeField] private int? queenCounter; // Lower means closer to hive.
    [SerializeField] private float PheromonMarkStrength =5f;
    [SerializeField] private enum target{queen,resource}

    private target currentTarget;

    private MetaTile currentTile;
    private MetaTile nextTile;

    private TileInfo currentTileInfo;
    private TileInfo nextTileInfo;

    private DroneState currentState;

    [SerializeField] private SpriteRenderer SR;

    #region Initialization Methods

    public void InitDrone(MetaTile _currentTile)
    {
        currentTarget = target.resource;
        currentTile = _currentTile;
        currentTileInfo = currentTile.GetTileInfo();
        SR.transform.localScale = currentTileInfo.tileSize;

        GameManager gm = FindObjectOfType<GameManager>();
        gm.onTick.AddListener(OnTick);
    }

    #endregion

    private void OnTick()
    {
        Debug.Log($"[OnTick START] Current Tile: {currentTile.name}, Target: {currentTarget}, " +
                  $"ResourceCounter: {resourceCounter.GetValueOrDefault(int.MaxValue)}, " +
                  $"QueenCounter: {queenCounter.GetValueOrDefault(int.MaxValue)}");
        GetTileInfo();
        UpdateTimers();
        LeavePheromoneMark();
        ChooseNextTile();
        MoveDrone();
        currentTile = nextTile;
        Debug.Log($"[OnTick END] Moved to Tile: {currentTile.name}, " +
                  $"Tile Position: {currentTile.GetTileTransform().position}");
    }
    
    private void GetTileInfo()
    {
        currentTileInfo = currentTile.GetTileInfo();
    }

    private void UpdateTimers()
{
    // If the current tile directly contains a target:
    if (currentTileInfo.hasResource || currentTileInfo.hasQueen)
    {
        if (currentTileInfo.hasQueen)
        {
            // When on a queen tile:
            queenCounter = 0;        // At queen, distance is 0.
            currentTarget = target.resource;  // Now drone will search for a resource.
            resourceCounter = null;
        }
        if (currentTileInfo.hasResource)
        {
            // When on a resource tile:
            resourceCounter = 0;     // At resource, distance is 0.
            currentTarget = target.queen;     // Now drone will search for the queen.
            queenCounter = null;
        }
    }
    else
    {
        // When not on a target tile, update the appropriate counter.
        if (currentTarget == target.resource)
        {
            // Drone is following a path from queen (target resource means it left the queen)
            // So we update queenCounter.
            if (queenCounter == null)
            {
                queenCounter = 1; // First step away from queen.
            }
            else
            {
                queenCounter = queenCounter.Value + 1;
            }
            // Now check pheromone markings on this tile for queen type.
            foreach (var pher in currentTileInfo.pheromones)
            {
                if (pher.Type == Pheromone.PheromoneType.Queen && pher.Distance.HasValue)
                {
                    // If the tile already indicates a shorter path than our measured steps, adopt it.
                    if (pher.Distance.Value < queenCounter.Value)
                    {
                        queenCounter = pher.Distance;
                    }
                }
            }
        }
        else if (currentTarget == target.queen)
        {
            // Drone is following a path from resource (target queen means it left the resource)
            // So we update resourceCounter.
            if (resourceCounter == null)
            {
                resourceCounter = 1; // First step away from resource.
            }
            else
            {
                resourceCounter = resourceCounter.Value + 1;
            }
            // Check pheromone markings for resource type.
            foreach (var pher in currentTileInfo.pheromones)
            {
                if (pher.Type == Pheromone.PheromoneType.Resource && pher.Distance.HasValue)
                {
                    if (pher.Distance.Value < resourceCounter.Value)
                    {
                        resourceCounter = pher.Distance;
                    }
                }
            }
        }
    }
}




    private void LeavePheromoneMark()
    {
        currentTile.UpdatePheromone(queenCounter, resourceCounter,PheromonMarkStrength);
    }
private void ChooseNextTile()
{
    MetaTile[] neighborTiles = currentTileInfo.neighborTiles;
    MetaTile[] validNeighbors = neighborTiles.Where(tile => tile != null).ToArray();
    Debug.Log($"[ChooseNextTile] Valid Neighbors Count: {validNeighbors.Length}");

    // 1. First, look for any neighbor that directly contains the target object.
    List<MetaTile> directTargets = new List<MetaTile>();
    if (currentTarget == target.resource)
    {
        directTargets.AddRange(validNeighbors.Where(tile => tile.tileInfo.hasResource));
    }
    else if (currentTarget == target.queen)
    {
        directTargets.AddRange(validNeighbors.Where(tile => tile.tileInfo.hasQueen));
    }
    
    if (directTargets.Count > 0)
    {
        Debug.Log($"[ChooseNextTile] Direct target found. Count: {directTargets.Count}");
        nextTile = directTargets[Random.Range(0, directTargets.Count)];
        Debug.Log($"[ChooseNextTile] Selected Direct Target: {nextTile.name}");
        return;
    }

    // 2. If no direct target exists, search for neighbor tiles with a pheromone mark.
    // Determine the drone's internal counter based on the current target.
    int? internalCounter = (currentTarget == target.resource ? resourceCounter : queenCounter);
    int effectiveCounter = internalCounter.GetValueOrDefault(int.MaxValue);
    Debug.Log($"[ChooseNextTile] InternalCounter: {internalCounter} (Effective: {effectiveCounter})");

    // Create a list to hold neighbor tiles that have a valid pheromone signal.
    List<MetaTile> pheromoneCandidates = new List<MetaTile>();
    // Use a dictionary to group tiles by their matching pheromone distance.
    Dictionary<MetaTile, int> candidateDistances = new Dictionary<MetaTile, int>();

    foreach (var tile in validNeighbors)
    {
        var matchingPheromone = tile.tileInfo.pheromones.FirstOrDefault(p =>
            (currentTarget == target.resource && p.Type == Pheromone.PheromoneType.Resource) ||
            (currentTarget == target.queen && p.Type == Pheromone.PheromoneType.Queen)
        );

        if (matchingPheromone != null && matchingPheromone.Distance.HasValue)
        {
            Debug.Log($"[ChooseNextTile] Tile: {tile.name} has pheromone distance: {matchingPheromone.Distance.Value}");
        }

        if (matchingPheromone != null &&
            matchingPheromone.Distance.HasValue &&
            matchingPheromone.Distance.Value < effectiveCounter)
        {
            candidateDistances[tile] = matchingPheromone.Distance.Value;
            pheromoneCandidates.Add(tile);
            Debug.Log($"[ChooseNextTile] Added candidate: {tile.name} with pheromone distance: {matchingPheromone.Distance.Value}");
        }
    }

    if (pheromoneCandidates.Count > 0)
    {
        // 2a. Find the minimum pheromone distance among candidates.
        int minDistance = candidateDistances.Values.Min();
        Debug.Log($"[ChooseNextTile] Minimum pheromone distance among candidates: {minDistance}");
        // 2b. Filter candidates to only those with the minimum distance.
        var bestCandidates = candidateDistances.Where(kvp => kvp.Value == minDistance)
                                               .Select(kvp => kvp.Key)
                                               .ToList();
        Debug.Log($"[ChooseNextTile] Best candidate count: {bestCandidates.Count}");
        // Choose one at random among those best candidates.
        nextTile = bestCandidates[Random.Range(0, bestCandidates.Count)];
        Debug.Log($"[ChooseNextTile] Selected candidate: {nextTile.name}");
        return;
    }

    // 3. Fallback: choose a random valid neighbor.
    nextTile = validNeighbors[Random.Range(0, validNeighbors.Length)];
    Debug.Log($"[ChooseNextTile] Fallback random selection: {nextTile.name}");
}


    
    private void MoveDrone()
    {
        transform.position = nextTile.GetTileTransform().position;
    }
}