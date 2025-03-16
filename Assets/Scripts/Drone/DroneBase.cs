using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroneBase : MonoBehaviour
{
    private DroneState currentState;

    public DroneData droneData;

    public void InitDrone(MetaTile _currentTile)
    {
        droneData.currentTile = _currentTile;
        GameManager gm = FindObjectOfType<GameManager>();
        gm.onTick.AddListener(OnTick);
    }

    private void OnTick()
    {
        // GetTileInfo();
        // UpdateTimers();
        // LeavePheromoneMark();
        // ChooseNextTile();
        // MoveDrone();
        // currentTile = nextTile;
    }

    public void ChangeState(DroneState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public MetaTile GetTileData()
    {
        TileData currentTileData = DroneData.currentTile.GetTileData();
        return currentTileData;
    }


    private void LeavePheromoneMark()
    {
        DroneData.currentTile.UpdatePheromone(droneData.PheromoneCounter[droneData.PheromoneOriginPoint],droneData.PheromoneOriginPoint , droneData.PheromonMarkStrength);
    }


    private void MoveDrone()
    {
        transform.position = DroneData.nextTile.GetTileTransform().position;
    }
}