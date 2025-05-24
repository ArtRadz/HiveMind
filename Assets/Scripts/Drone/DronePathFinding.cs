using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;


static class DronePathFinding 
{
   public static MetaTile ChooseNextTile(TileData currentTileData, UQM currentTarget, int? currentCounter)
    {
        MetaTile[] neighborTiles = currentTileData.neighborTiles;
        List<MetaTile> validNeighbors = neighborTiles.Where(tile => tile != null && tile.tileData.tileSpecialType != UQM.Blocker).ToList();
        foreach (var entry in validNeighbors)
        {
            if (EvaluationStrategyManager.DirectTargetCheck(entry.GetTileData(), currentTarget))
            {
                Debug.Log("DIRECT TARGET FOUND!!!!!!!!!!!!!!!!!!!!");
                return entry;
            }
        }

        List<MetaTile> pheromoneCandidates = new List<MetaTile>();

        int? bestValue = currentCounter;
        foreach (var entry in validNeighbors)
        {
            int? phValue = EvaluationStrategyManager.EvaluatePheromone(entry.GetTileData(), currentTarget);
            if (phValue != null)
            {
                Debug.Log("PHValue is : " + phValue + " and best value is : " + bestValue);
                if (bestValue==null || phValue<bestValue)
                {
                    bestValue = phValue;
                    pheromoneCandidates.Add(entry);
                    
                }
            }
        }

        pheromoneCandidates = pheromoneCandidates
            .Where(entry => EvaluationStrategyManager.EvaluatePheromone(entry.GetTileData(), currentTarget) == bestValue)
            .ToList();

        if (pheromoneCandidates.Count >0)
        {
            return ChooseRandomNeighbor(pheromoneCandidates);
        }

        return ChooseRandomNeighbor(validNeighbors);
    }

    private static MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        // Debug.Log("I got here with those neighbours " + neighbors)//;
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}