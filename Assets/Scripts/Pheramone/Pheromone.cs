using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pheromone 
{
    public enum PheromoneType
    {
        Queen,
        Resource
    }

    public int? Distance { get; set; }
    public float Strength { get; set; }
    public PheromoneType Type;
    public Pheromone(PheromoneType type, int? distance, float strength)
    {
        Type = type;
        Distance = distance;
        Strength = strength;
    }
}
