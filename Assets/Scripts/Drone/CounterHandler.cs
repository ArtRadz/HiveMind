using UnityEngine;
using UQM = UniversalQualifierMarker;

public static class CounterHandler
{
    public static (UQM, int?) UpdateCounters(TileData currentTileData, (UQM uqmToCompare, int? distance) counter)
    {
        if (EvaluationStrategyManager.DirectTargetCheck(currentTileData, counter.uqmToCompare))
        {
            counter.distance = 0;
            return counter;
        }

        int? evaluatedValue = EvaluationStrategyManager.EvaluatePheromone(currentTileData, counter.uqmToCompare);

        if (evaluatedValue != null)
        {
            if (evaluatedValue < counter.distance || counter.distance == null)
            {
                counter.distance = evaluatedValue;
                return counter;
            }
        }

        if (counter.distance != null)
        {
            counter.distance++;
        }

        return counter;
    }
}