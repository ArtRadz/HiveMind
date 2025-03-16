public abstract class DroneState {
    protected DroneBase drone;
    
    public DroneBaseState(DroneBase drone) {
        this.drone = drone;
    }
    
    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}