using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;

[CreateAssetMenu(fileName = "DroneBlueprint", menuName = "Drone/Blueprint")]
public class DroneBlueprint : ScriptableObject
{
    [SerializeField] private float PheromonMarkStrength =5f;
    [SerializeField] private float Strength =5f;
    private UQM Target;
}
