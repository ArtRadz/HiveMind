using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CounterHandler;
public class GatherResourcesState : DroneState
{
    private int placeHolderCounter;
    public GatherResourcesState(DroneBase drone) : base(drone)
    {
        
    }
    public override void Enter()
    {
        placeHolderCounter = 3;
    }

    public override void Execute()
    {
        placeHolderCounter--;
        if (placeHolderCounter <= 0)
        {
            drone.droneData.Target = UniversalQualifierMarker.Queen;
            drone.droneData.bestStepToTarget = int.MaxValue;
            drone.droneData.PheromoneCounter.origin = UniversalQualifierMarker.Resource;
            drone.droneData.PheromoneCounter.distance = 0;
            drone.ChangeState(new SearchState(drone));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
