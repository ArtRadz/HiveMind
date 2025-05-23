using System.Collections;
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
        // droneData.Target = UQM.Resource;
        droneData.PheromoneCounterToOrigin = (UQM.Queen, 0);
        droneData.PheromoneCounterToTarget = (UQM.Resource, null);
        currentState = new SearchState(this); //Todo currently first state is hardcoded refactor when relevant (probably when more than 1type of drones)
        GameManager gm = FindObjectOfType<GameManager>();
        gm.onTick.AddListener(OnTick);
    }

    private void OnTick(float tickDuration)
    {
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
        droneData.currentTile.UpdatePheromone(droneData.PheromoneCounterToOrigin , droneData.PheromonMarkStrength);
    }


    public void MoveDrone()
    {
        // transform.position = droneData.nextTile.GetTileTransform().position;
        StartCoroutine(SmoothMove(transform.position, droneData.nextTile.GetTileTransform().position, 0.5f));
        // Vector3 start = transform.position;
        // Vector3 end = droneData.nextTile.GetTileTransform().position;
        //
        // int steps = 999; // Number of visual steps, higher = smoother
        // float stepSize = 1f / steps;
        //
        // for (int i = 1; i <= steps; i++)
        // {
        //     float t = i * stepSize;
        //     transform.position = Vector3.Lerp(start, end, t);
        // }
        //
        // // Final snap to ensure no rounding errors
        // transform.position = end;
    }
    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        // Clear coroutine reference when done
        // currentMoveCoroutine = null;
    }
}