using UnityEngine;

public abstract class DroneState
{
    protected Drone drone;
    public DroneState(Drone drone)
    {
        this.drone = drone;
    }
    // Called once on state entry.
    public abstract void Enter();
    // Called on every tick.
    public abstract void ExecuteTick();
    // Called on state exit.
    public abstract void Exit();
}