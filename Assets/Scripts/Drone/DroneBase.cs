using System.Collections;
using UnityEngine;
using UQM = UniversalQualifierMarker;

public class DroneBase : MonoBehaviour
{
    private DroneState currentState;

    public DroneData droneData;

    private float remainingTickDuration;
    [SerializeField] private SpriteRenderer SR;

    public void InitDrone(MetaTile currentTile)
    {
        droneData.currentTile = currentTile;
        droneData.PheromoneCounterToOrigin = (UQM.Queen, 0);
        droneData.PheromoneCounterToTarget = (UQM.Resource, null);

        currentState = new SearchState(this); // TODO: Hardcoded; refactor if multiple drone types are introduced

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
        return droneData.currentTile.GetTileData();
    }

    public void LeavePheromoneMark()
    {
        droneData.currentTile.UpdatePheromone(droneData.PheromoneCounterToOrigin, droneData.pheromoneMarkStrength);
    }

    public void MoveDrone()
    {
        StartCoroutine(SmoothMove(
            transform.position,
            droneData.nextTile.GetTileTransform().position,
            remainingTickDuration
        ));
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