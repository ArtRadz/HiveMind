using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;
using static DronePathFinding;
using static CounterHandler;

public class SearchState : DroneState
{

    public SearchState(DroneBase drone) : base(drone)
    {
        
    }

    public override void Enter()
    {
        
    }

    public override void Execute()
    {
        DroneData droneData = drone.droneData;
        TileData tileData = drone.GetTileData();
        if (tileData.tileSpecialType == droneData.Target)
        {
            DroneState nextState;
            if (droneData.Target == UQM.Resource)
            {
                nextState = new GatherResourcesState(drone);
            }

            // if (droneData.Target == UQM.Queen)
            else
            {
                nextState = new DepositResourceState(drone);
            }
            droneData.Target = droneData.PheromoneCounter.origin;
            drone.ChangeState(nextState);
            return;
        }
        droneData.PheromoneCounter = CounterHandler.UpdateCounters(tileData,droneData.Target,droneData.PheromoneCounter);
        droneData.nextTile = DronePathFinding.ChooseNextTile(tileData,droneData.Target,droneData.PheromoneCounter.distance);
        drone.LeavePheromoneMark();
        drone.MoveDrone();
        droneData.currentTile = droneData.nextTile;
    }

    public override void Exit()
    {
        
    }
}