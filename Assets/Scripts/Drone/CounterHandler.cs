using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;


static class CounterHandler 
{
    public static (UQM, int) UpdateCounters(TileData currentTileData, UQM target, (UQM origin, int distance) counter)
    {
        if (EvaluationStrategyManager.DirectTargetCheck(currentTileData, target))
        {
            counter.origin = target;
            counter.distance = 0;
            return counter;
        }
        int evaluatedValue = EvaluationStrategyManager.EvaluatePheromone(currentTileData, counter.origin);
        if (evaluatedValue < counter.distance)
        {
            counter.distance = evaluatedValue;
            return counter;
        }

        counter.distance++;
        return counter;
    }
}