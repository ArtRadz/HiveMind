using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;

public static class DronePathFinding
{
    public static MetaTile ChooseNextTile(TileData currentTileData, UQM currentTarget, int? currentCounter)
    {
        MetaTile[] neighborTiles = currentTileData.neighborTiles;

        List<MetaTile> validNeighbors = neighborTiles
            .Where(tile => tile != null && tile.tileData.tileSpecialType != UQM.Blocker)
            .ToList();

        foreach (var entry in validNeighbors)
        {
            if (EvaluationStrategyManager.DirectTargetCheck(entry.GetTileData(), currentTarget))
            {
                return entry;
            }
        }

        List<MetaTile> pheromoneCandidates = new();
        int? bestValue = currentCounter;

        foreach (var entry in validNeighbors)
        {
            int? phValue = EvaluationStrategyManager.EvaluatePheromoneDistance(entry.GetTileData(), currentTarget);

            if (phValue != null && (bestValue == null || phValue < bestValue))
            {
                bestValue = phValue;
                pheromoneCandidates.Add(entry);
            }
        }

        pheromoneCandidates = pheromoneCandidates
            .Where(entry => EvaluationStrategyManager.EvaluatePheromoneDistance(entry.GetTileData(), currentTarget) == bestValue)
            .ToList();

        if (pheromoneCandidates.Count > 0)
        {
            return ChooseRandomNeighbor(pheromoneCandidates);
        }

        return ChooseRandomWeightedNeighbor(validNeighbors,currentTarget);
    }

    private static MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        return neighbors[Random.Range(0, neighbors.Count)];
    }
    private static MetaTile ChooseRandomWeightedNeighbor(List<MetaTile> neighbors,UQM currentTarget)
    {
        float totalWeight = 0f;
        foreach (MetaTile neighbor in neighbors)
        {
            float phStrength;
            TileData tileData = neighbor.GetTileData();
            if (tileData.tileSpecialType == UQM.Queen)
            {
                phStrength = 999;
            }
            else
            {
                phStrength = EvaluationStrategyManager.EvaluatePheromoneStrength(tileData, currentTarget);
            }
            totalWeight += 1 / (1 + phStrength);
        }
        float r= Random.value * totalWeight;
        foreach (MetaTile neighbor in neighbors)
        {
            float phStrength;
            TileData tileData = neighbor.GetTileData();
            if (tileData.tileSpecialType == UQM.Queen)
            {
                phStrength = 999;
            }
            else
            {
                phStrength = EvaluationStrategyManager.EvaluatePheromoneStrength(tileData, currentTarget);
            }
            r -= 1 / (1 + phStrength);
            if (r <= 0)
            {
                return neighbor;
            }
        }
        return ChooseRandomNeighbor(neighbors);
    }
}