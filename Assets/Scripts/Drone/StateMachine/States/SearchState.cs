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
        UpdateOriginCounter(droneData,tileData);
        HandleTargetSearch(droneData, tileData);
        UpdateTargetCounter(droneData, tileData);
        drone.MoveDrone();
        droneData.currentTile = droneData.nextTile;
    }

    private void UpdateTargetCounter(DroneData droneData, TileData tileData)
    {
        
        droneData.PheromoneCounterToTarget = CounterHandler.UpdateCounters(tileData, droneData.PheromoneCounterToTarget);
        
    }
    private void HandleTargetSearch(DroneData droneData, TileData tileData)
    {
        UQM target = droneData.PheromoneCounterToTarget.target;
        int? stepsToTarget = droneData.PheromoneCounterToTarget.distanceToTarget;
        UQM origin = droneData.PheromoneCounterToOrigin.origin;
        if (tileData.tileSpecialType == droneData.PheromoneCounterToTarget.target)
        {
            DroneState nextState;
            if (droneData.PheromoneCounterToTarget.target == UQM.Resource)
            {
                nextState = new GatherResourcesState(drone);
            }
            else
            {
                nextState = new DepositResourceState(drone);
            }
            droneData.PheromoneCounterToTarget.target = origin;
            droneData.PheromoneCounterToOrigin.origin = target;
            drone.ChangeState(nextState);
            return;
        }
        droneData.nextTile = DronePathFinding.ChooseNextTile(tileData,target,stepsToTarget);
        
    }
    
    private void UpdateOriginCounter(DroneData droneData, TileData tileData)
    {
        int? currentDistance = droneData.PheromoneCounterToOrigin.distanceToOrigin;
        droneData.PheromoneCounterToOrigin =
            CounterHandler.UpdateCounters(tileData, droneData.PheromoneCounterToOrigin);
        if (currentDistance < droneData.PheromoneCounterToOrigin.distanceToOrigin)
        {
            drone.LeavePheromoneMark();
        }
        else
        {
            droneData.PheromoneCounterToOrigin.distanceToOrigin = currentDistance;
        }
    }
    
    public override void Exit()
    {
        
    }
}