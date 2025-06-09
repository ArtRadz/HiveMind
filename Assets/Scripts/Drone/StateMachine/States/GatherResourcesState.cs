using UnityEngine;

public class GatherResourcesState : DroneState
{
    private int placeholderCounter;

    public GatherResourcesState(DroneBase drone) : base(drone)
    {
    }

    public override void Enter()
    {
        drone.ChangeSpriteColor(Color.black);
        placeholderCounter = 3;
    }

    public override void Execute()
    {
        placeholderCounter--;

        if (placeholderCounter <= 0)
        {
            drone.droneData.PheromoneCounterToTarget = (UniversalQualifierMarker.Queen, null);
            drone.droneData.PheromoneCounterToOrigin = (UniversalQualifierMarker.Resource, 0);
            drone.ChangeState(new SearchState(drone));
        }
    }

    public override void Exit()
    {
    }
}