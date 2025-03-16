using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;
[System.Serializable]
public class DroneData
{
    public MetaTile currentTile;
    public MetaTile nextTile;
    public TileData currentTileData;
    public TileData nextTileData;
    public float PheromonMarkStrength;
    public float Strength;
    public UQM Target;
    public UQM PheromoneOriginPoint;
    public Dictionary<UQM , int?> PheromoneCounter;

}
