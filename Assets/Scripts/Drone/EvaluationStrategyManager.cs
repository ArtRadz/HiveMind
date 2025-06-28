using UnityEngine;
using UQM = UniversalQualifierMarker;

/// <summary>
/// Provides evaluation strategies used by drones to decide on targets or follow pheromones.
/// </summary>
public static class EvaluationStrategyManager
{
    public static bool DirectTargetCheck(TileData tileData, UQM uqmToCompare)
    {
        return tileData.tileSpecialType == uqmToCompare;
    }

    public static int? EvaluatePheromone(TileData tileData, UQM uqmToCompare)
    {
        foreach (Pheromone pher in tileData.pheromones)
        {
            if (pher.Type == uqmToCompare)
            {
                return pher.Distance;
            }
        }

        return null;
    }
}