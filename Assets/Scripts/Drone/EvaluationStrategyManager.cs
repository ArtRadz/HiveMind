using UnityEngine;
using UQM = UniversalQualifierMarker;

public static class EvaluationStrategyManager
{
    public static bool DirectTargetCheck(TileData tileData, UQM uqmToCompare)
    {
        return tileData.tileSpecialType == uqmToCompare;
    }

    public static int? EvaluatePheromoneDistance(TileData tileData, UQM uqmToCompare)
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
    public static float EvaluatePheromoneStrength(TileData tileData, UQM uqmToCompare)
    {
        foreach (Pheromone pher in tileData.pheromones)
        {
            if (pher.Type != uqmToCompare)
            {
                return pher.Strength;
            }

            return 0;
        }

        return 0;
    }
}