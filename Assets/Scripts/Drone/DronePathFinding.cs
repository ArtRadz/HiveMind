using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;


public class DronePathFinding : MonoBehaviour
{
    public MetaTile ChooseNextTile(TileData currentTileData, UQM currentTarget, int? currentCounter)
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
        int? bestValue = currentCounter != null ? currentCounter - 1 : null;
        foreach (var entry in validNeighbors)
        {
            int? phValue = EvaluationStrategyManager.EvaluatePheromone(entry.GetTileData(), currentTarget);
            
            if (phValue != null && (phValue <= bestValue || bestValue == null))
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

    private MetaTile ChooseRandomNeighbor(List<MetaTile> neighbors)
    {
        return neighbors[Random.Range(0, neighbors.Count)];
    }
}