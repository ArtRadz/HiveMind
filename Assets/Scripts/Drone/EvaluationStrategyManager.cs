using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EvaluationStrategyManager
{
    public delegate bool DirectTargetCheck(TileData tileData);

    public delegate int? PheromoneGetter(TileData tileData);

    public struct TargetEvaluationStrategy
    {
        public DroneData.DroneTargets Target;
        public DirectTargetCheck IsDirectTarget;
        public PheromoneGetter GetPheromoneDistance;
    }

    public static Dictionary<DroneData.DroneTargets, TargetEvaluationStrategy> targetStrategies =
        new Dictionary<DroneData.DroneTargets, TargetEvaluationStrategy>()
        {
            {
                DroneData.DroneTargets.Resource,
                new TargetEvaluationStrategy()
                {
                    Target = DroneData.DroneTargets.Resource,
                    IsDirectTarget = new DirectTargetCheck(tileData => tileData.hasResource),
                    GetPheromoneDistance = new PheromoneGetter(p =>
                    {
                        if (p.pheromones.type == Pheromone.PheromoneType.Resource)
                        {
                            return p.pheromones.distance;
                        }
                        else
                        {
                            return null;
                        }
                    })
                }
            },
            {
                DroneData.DroneTargets.Queen,
                new TargetEvaluationStrategy()
                {
                    Target = DroneData.DroneTargets.Queen,
                    IsDirectTarget = new DirectTargetCheck(tileData => tileData.hasQueen),
                    GetPheromoneDistance = new PheromoneGetter(p =>
                    {
                        if (p.pheromones.type == Pheromone.PheromoneType.Queen)
                        {
                            return p.pheromones.distance;
                        }
                        else
                        {
                            return null;
                        }
                    })
                }
            }
        };
}