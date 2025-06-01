using System.Collections;
using System.Linq;
using UnityEngine;
using UQM = UniversalQualifierMarker;
public class DroneBase : MonoBehaviour
{
    private DroneState currentState;

    public DroneData droneData;

    private float remainingTickDuration;
    [SerializeField] private SpriteRenderer SR;

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
        StartCoroutine(TickCountdownCoroutine(tickDuration));
        currentState.Execute();
    }
    
    private IEnumerator TickCountdownCoroutine(float tickDuration)
    {
        remainingTickDuration = tickDuration;

        while (remainingTickDuration > 0f)
        {
            remainingTickDuration -= Time.deltaTime;
            yield return null; 
        }

        remainingTickDuration = 0f; 
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
        StartCoroutine(SmoothMove(transform.position, droneData.nextTile.GetTileTransform().position,
            remainingTickDuration));
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

    }
    public void ChangeSpriteColor(Color colorToSet)
    {
        SR.color = colorToSet;
    }

}