using UnityEngine;
using UQM = UniversalQualifierMarker;

[CreateAssetMenu(fileName = "DroneBlueprint", menuName = "Drone/Blueprint")]
public class DroneBlueprint : ScriptableObject
{
    [SerializeField] private float pheromoneMarkStrength = 5f;
    [SerializeField] private float strength = 5f;

    [SerializeField] private UQM target;

    // Optionally, expose public getters if needed later
    public float PheromoneMarkStrength => pheromoneMarkStrength;
    public float Strength => strength;
    public UQM Target => target;
}