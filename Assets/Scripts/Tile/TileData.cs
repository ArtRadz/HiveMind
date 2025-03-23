using System;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;
[System.Serializable]
public class TileData
{
    public UQM tileSpecialType;
    public MetaTile[] neighborTiles;
    public int[] tilePosition;
    public Vector3 tileSize;
    public List<Pheromone> pheromones = new List<Pheromone>();
    public float PheromonalDecayValuePerTick;
    public float maxPheromoneStrength = 100f;

}