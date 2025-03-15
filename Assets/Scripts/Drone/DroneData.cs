using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneData : MonoBehaviour
{
    public MetaTile currentTile;
    public MetaTile nextTile;

    public float PheromonMarkStrength;
    public float Strength;
    public enum DroneTargets
    {
        Queen,Resource
    }
    public Dictionary<DroneTargets, int?> DistanceCounters;
    public DroneTargets CurrentTarget;
}
