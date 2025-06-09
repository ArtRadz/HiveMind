using UnityEngine;
using UQM = UniversalQualifierMarker;

[System.Serializable]
public class DroneData
{
    public MetaTile currentTile;
    public MetaTile nextTile;

    public TileData currentTileData;
    public TileData nextTileData;

    public float pheromoneMarkStrength;
    public float strength;

    public (UQM target, int? distanceToTarget) PheromoneCounterToTarget;
    public (UQM origin, int? distanceToOrigin) PheromoneCounterToOrigin;
}