using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CounterHandler : MonoBehaviour
{
    
    public int? UpdateCounters(TileData currentTileData, DroneData.DroneTargets target, int currentCounter)
    {
        TargetEvaluationStrategy strategy = counterStrategies[target];
        if (strategy.IsDirectTarget(currentTileData))
        {
            return 0;
        }
        else if (strategy.IsRelevantPheromoneMark(currentTileData))
        {
            // return currentCounter = ;
        }
        
    }
//     // If the current tile directly contains a target:
//     if (currentTileInfo.hasResource || currentTileInfo.hasQueen)
//     {
//         if (currentTileInfo.hasQueen)
//         {
//             // When on a queen tile:
//             queenCounter = 0;        // At queen, distance is 0.
//             currentTarget = target.resource;  // Now drone will search for a resource.
//             resourceCounter = null;
//         }
//         if (currentTileInfo.hasResource)
//         {
//             // When on a resource tile:
//             resourceCounter = 0;     // At resource, distance is 0.
//             currentTarget = target.queen;     // Now drone will search for the queen.
//             queenCounter = null;
//         }
//     }
//     else
//     {
//         // When not on a target tile, update the appropriate counter.
//         if (currentTarget == target.resource)
//         {
//             // Drone is following a path from queen (target resource means it left the queen)
//             // So we update queenCounter.
//             if (queenCounter == null)
//             {
//                 queenCounter = 1; // First step away from queen.
//             }
//             else
//             {
//                 queenCounter = queenCounter.Value + 1;
//             }
//             // Now check pheromone markings on this tile for queen type.
//             foreach (var pher in currentTileInfo.pheromones)
//             {
//                 if (pher.Type == Pheromone.PheromoneType.Queen && pher.Distance.HasValue)
//                 {
//                     // If the tile already indicates a shorter path than our measured steps, adopt it.
//                     if (pher.Distance.Value < queenCounter.Value)
//                     {
//                         queenCounter = pher.Distance;
//                     }
//                 }
//             }
//         }
//         else if (currentTarget == target.queen)
//         {
//             // Drone is following a path from resource (target queen means it left the resource)
//             // So we update resourceCounter.
//             if (resourceCounter == null)
//             {
//                 resourceCounter = 1; // First step away from resource.
//             }
//             else
//             {
//                 resourceCounter = resourceCounter.Value + 1;
//             }
//             // Check pheromone markings for resource type.
//             foreach (var pher in currentTileInfo.pheromones)
//             {
//                 if (pher.Type == Pheromone.PheromoneType.Resource && pher.Distance.HasValue)
//                 {
//                     if (pher.Distance.Value < resourceCounter.Value)
//                     {
//                         resourceCounter = pher.Distance;
//                     }
//                 }
//             }
//         }
//     }
    // }
}