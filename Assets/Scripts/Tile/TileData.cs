using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public bool hasResource;
    public bool hasQueen;
    public MetaTile[] neighborTiles;
    public int[] tilePosition;
    public Vector3 tileSize;
    public List<Pheromone> pheromones = new List<Pheromone>();
    public float PheromonalDecayValuePerTick;
}