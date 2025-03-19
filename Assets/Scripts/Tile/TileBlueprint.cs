using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UQM = UniversalQualifierMarker;

[CreateAssetMenu(fileName = "TileData", menuName = "Tile/TileData", order = 1)]
public class TileBlueprint : ScriptableObject
{
    [Header("Default Meta Values")] 
    public float PheromonalDecayValuePerTick;
    public List<Pheromone> pheromones = new List<Pheromone>();
    
    public int[] tilePosition = new int[2];
    public MetaTile[] neighborTiles;
    public Vector3 tileSize;

    public UQM tileType = UQM.DefaultTile;

    [Header("Visual Assets")]
    public TileBase defaultTile;    // Visual for a default tile.
    public TileBase resourceTile;   // Visual for a tile with a resource.
    public TileBase queenTile;       // Visual for a tile with a Quin (hive).
}