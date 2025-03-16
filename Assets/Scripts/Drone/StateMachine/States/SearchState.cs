using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : DroneState
{
    public DroneData.DroneTargets currentTarget;

    public SearchState(DroneBase drone, DroneData.DroneTargets target) : base(drone) {
        currentTarget = target;
    }

    public override void Enter() 
    {
        Debug.Log("Entering SearchState with target: " + currentTarget);
    }

    public override void Execute() 
    {
        drone.droneData.nextTile = DronePathFinding.ChooseNextTile(drone.GetTileInfo());
    }

    public override void Exit() 
    {
        // Cleanup logic when exiting the state.
        Debug.Log("Exiting SearchState.");
    }
}
