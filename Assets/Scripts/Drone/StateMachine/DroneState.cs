public abstract class DroneState
{
    protected DroneBase drone;

    protected DroneState(DroneBase drone)
    {
        this.drone = drone;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}