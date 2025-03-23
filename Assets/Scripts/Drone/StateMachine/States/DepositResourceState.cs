using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositResourceState : DroneState
{
    private int placeHolderCounter ;
    public DepositResourceState(DroneBase drone) : base(drone)
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
            drone.droneData.PheromoneCounterToTarget = (UniversalQualifierMarker.Resource , null);
            drone.droneData.PheromoneCounterToOrigin = (UniversalQualifierMarker.Queen,0);
            drone.ChangeState(new SearchState(drone));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
