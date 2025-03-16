using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DronePathFinding : MonoBehaviour
{
    public MetaTile ChooseNextTile(TileData currentTileData, DroneData.DroneTargets currentTarget, int currentCounter)
    {
        MetaTile[] neighborTiles = currentTileData.neighborTiles;
        List<MetaTile> validNeighbors = neighborTiles.Where(tile => tile != null).ToList();
        EvaluationStrategyManager.TargetEvaluationStrategy strategy =
            EvaluationStrategyManager.targetStrategies[currentTarget];
        foreach (var entry in validNeighbors)
        {
            if (strategy.IsDirectTarget(entry.GetTileData()))
            {
                return entry;
            }
        }

        foreach (var entry in validNeighbors)
        {
            MetaTile pheromoneCandidate =
                EvaluatePheromoneCandidate(entry, strategy.GetPheromoneDistance, currentCounter);
            if (pheromoneCandidate != null)
            {
                return pheromoneCandidate;
            }
        }

        MetaTile randomNeighbor = ChooseRandomNeighbor(validNeighbors);
        return randomNeighbor;
    }

    // private MetaTile EvaluateDirectTarget(List<MetaTile> neighbors,
    //     EvaluationStrategyManager.DirectTargetCheck isDirectTarget)
    // {
    //     foreach (MetaTile tile in neighbors)
    //     {
    //         TileData tileData = tile.GetTileData();
    //         if (isDirectTarget(tileData))
    //         {
    //             return tile;
    //         }
    //     }
    //
    //     return null;
    // }
    //
    // private MetaTile EvaluatePheromoneCandidate(List<MetaTile> neighbors,
    //     EvaluationStrategyManager.PheromoneGetter getPheromoneDistance,
    //     int currentCounter)
    // {
    //     MetaTile bestCandidate = null;
    //     int bestDistance = currentCounter;
    //     foreach (MetaTile tile in neighbors)
    //     {
    //         TileData tileData = tile.GetTileData();
    //         int? distance = getPheromoneDistance(tileData);
    //         if (distance.HasValue && distance.Value < bestDistance)
    //         {
    //             bestDistance = distance.Value;
    //             bestCandidate = tile;
    //         }
    //     }
    //
    //     return bestCandidate;
    // }

    private MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}