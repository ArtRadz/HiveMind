using UnityEngine;
using UQM = UniversalQualifierMarker;

[System.Serializable]
public class Pheromone
{
    public int? Distance { get; set; }
    public float Strength { get; set; }
    public UQM Type;

    public Pheromone(UQM type, int? distance, float strength)
    {
        Type = type;
        Distance = distance;
        Strength = strength;
    }
}