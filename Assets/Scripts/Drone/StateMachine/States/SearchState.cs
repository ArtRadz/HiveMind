using UnityEngine;
using UQM = UniversalQualifierMarker;
using static DronePathFinding;
using static CounterHandler;
/// <summary>
/// Drone state for searching resources or the queen. Updates counters, evaluates neighbors,
/// and handles transitions to gather or deposit states.
/// </summary>
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

        UpdateOriginCounter(droneData, tileData);
        HandleTargetSearch(droneData, tileData);
        UpdateTargetCounter(droneData, tileData);

        drone.MoveDrone();
        droneData.currentTile = droneData.nextTile;
    }

    private void UpdateTargetCounter(DroneData droneData, TileData tileData)
    {
        droneData.PheromoneCounterToTarget =
            UpdateCounters(tileData, droneData.PheromoneCounterToTarget);
    }

    private void HandleTargetSearch(DroneData droneData, TileData tileData)
    {
        UQM target = droneData.PheromoneCounterToTarget.target;
        int? stepsToTarget = droneData.PheromoneCounterToTarget.distanceToTarget;
        UQM origin = droneData.PheromoneCounterToOrigin.origin;

        if (tileData.tileSpecialType == target)
        {
            DroneState nextState = target == UQM.Resource
                ? new GatherResourcesState(drone)
                : new DepositResourceState(drone);

            droneData.PheromoneCounterToTarget.target = origin;
            droneData.PheromoneCounterToOrigin.origin = target;

            drone.ChangeState(nextState);
            return;
        }

        droneData.nextTile = ChooseNextTile(tileData, target, stepsToTarget);
    }

    private void UpdateOriginCounter(DroneData droneData, TileData tileData)
    {
        int? currentDistance = droneData.PheromoneCounterToOrigin.distanceToOrigin;

        droneData.PheromoneCounterToOrigin =
            UpdateCounters(tileData, droneData.PheromoneCounterToOrigin);

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