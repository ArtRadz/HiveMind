using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;
public class DroneBase : MonoBehaviour
{
    private DroneState currentState;

    public DroneData droneData;

    public void InitDrone(MetaTile _currentTile)
    {
        droneData.currentTile = _currentTile;
        droneData.Target = UQM.Resource;
        droneData.PheromoneCounter = (UQM.Queen, 0);
        currentState = new SearchState(this); //Todo currently first state is hardcoded refactor when relevant (probably when more than 1type of drones)
        GameManager gm = FindObjectOfType<GameManager>();
        gm.onTick.AddListener(OnTick);
    }

    private void OnTick()
    {
        droneData.PheromoneCounter = CounterHandler.UpdateCounters(GetTileData(),droneData.Target,droneData.PheromoneCounter);
        currentState.Execute();
    }

    public void ChangeState(DroneState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public TileData GetTileData()
    {
        TileData currentTileData = droneData.currentTile.GetTileData();
        return currentTileData;
    }


    public void LeavePheromoneMark()
    {
        droneData.currentTile.UpdatePheromone(droneData.PheromoneCounter , droneData.PheromonMarkStrength);
    }


    public void MoveDrone()
    {
        transform.position = droneData.nextTile.GetTileTransform().position;
    }
}