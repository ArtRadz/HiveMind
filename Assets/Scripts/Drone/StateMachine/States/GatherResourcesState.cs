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
        drone.ChangeSpriteColor(Color.black);
        placeHolderCounter = 3;
    }

    public override void Execute()
    {
        placeHolderCounter--;
        if (placeHolderCounter <= 0)
        {
            drone.droneData.PheromoneCounterToTarget = (UniversalQualifierMarker.Queen , null);
            drone.droneData.PheromoneCounterToOrigin = (UniversalQualifierMarker.Resource,0);
            drone.ChangeState(new SearchState(drone));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
