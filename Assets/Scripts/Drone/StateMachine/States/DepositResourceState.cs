using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepositResourceState : DroneState
{
    private int placeHolderCounter = 3;
    public DepositResourceState(DroneBase drone) : base(drone)
    {
        
    }
    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        placeHolderCounter--;
        if (placeHolderCounter <= 0)
        {
            drone.ChangeState(new SearchState(drone));
            return;
        }
    }

    public override void Exit()
    {
        
    }
}
