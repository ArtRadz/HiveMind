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
                Debug.Log("DIRECT TARGET FOUND!!!!!!!!!!!!!!!!!!!!");
                return entry;
            }
        }

        List<MetaTile> pheromoneCandidates = new List<MetaTile>();
        
        int bestValue = (currentCounter > 0) ? currentCounter - 1 : int.MaxValue;
        foreach (var entry in validNeighbors)
        {
            int phValue = EvaluationStrategyManager.EvaluatePheromone(entry.GetTileData(), currentTarget);
            Debug.Log("PHValue is : " + phValue + " and best value is : " + bestValue);
            if (phValue < bestValue)
            {
                bestValue = phValue;
                pheromoneCandidates.Clear();
                pheromoneCandidates.Add(entry);
            }
        }

        if (pheromoneCandidates.Count == 1)
        {
            Debug.Log("HOW DID I GET HERE 1???????????????");
            return pheromoneCandidates[0];
        }
        else if (pheromoneCandidates.Count > 1)
        {
            Debug.Log("HOW DID I GET HERE 2???????????????");
            return ChooseRandomNeighbor(pheromoneCandidates);
        }

        return ChooseRandomNeighbor(validNeighbors);
    }

    private static MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        Debug.Log("I got here with those neighbours " + neighbors);
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}