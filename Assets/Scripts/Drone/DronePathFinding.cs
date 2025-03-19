using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;


static class DronePathFinding 
{
   public static MetaTile ChooseNextTile(TileData currentTileData, UQM currentTarget, int currentCounter)
    {
        MetaTile[] neighborTiles = currentTileData.neighborTiles;
        List<MetaTile> validNeighbors = neighborTiles.Where(tile => tile != null).ToList();
        foreach (var entry in validNeighbors)
        {
            if (EvaluationStrategyManager.DirectTargetCheck(entry.GetTileData(), currentTarget))
            {
                return entry;
            }
        }

        List<MetaTile> pheromoneCandidates = new List<MetaTile>();

// Initialize bestValue to currentCounter - 1.
        int bestValue = currentCounter - 1;

        foreach (var entry in validNeighbors)
        {
            // Now EvaluatePheromone returns a non-nullable int.
            int phValue = EvaluationStrategyManager.EvaluatePheromone(entry.GetTileData(), currentTarget);
    
            // Compare directly since bestValue is non-null.
            if (phValue <= bestValue)
            {
                bestValue = phValue;
                pheromoneCandidates.Clear();
                pheromoneCandidates.Add(entry);
            }
        }

        if (pheromoneCandidates.Count == 1)
        {
            return pheromoneCandidates[0];
        }
        else if (pheromoneCandidates.Count > 1)
        {
            return ChooseRandomNeighbor(pheromoneCandidates);
        }

        return ChooseRandomNeighbor(validNeighbors);
    }

    private static MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}