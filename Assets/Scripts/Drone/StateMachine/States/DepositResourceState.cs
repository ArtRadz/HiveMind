using UnityEngine;

public class DepositResourceState : DroneState
{
    private int placeholderCounter;

    public DepositResourceState(DroneBase drone) : base(drone)
    {
    }

    public override void Enter()
    {
        drone.ChangeSpriteColor(Color.white);
        placeholderCounter = 3;
    }

    public override void Execute()
    {
        placeholderCounter--;

        if (placeholderCounter <= 0)
        {
            drone.droneData.PheromoneCounterToTarget = (UniversalQualifierMarker.Resource, null);
            drone.droneData.PheromoneCounterToOrigin = (UniversalQualifierMarker.Queen, 0);
            drone.ChangeState(new SearchState(drone));
        }
    }

    public override void Exit()
    {
    }
}