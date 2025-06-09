using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UQM = UniversalQualifierMarker;

[CreateAssetMenu(fileName = "TileData", menuName = "Tile/TileData", order = 1)]
public class TileBlueprint : ScriptableObject
{
    [Header("Default Meta Values")]
    public float PheromonalDecayValuePerTick;
    public List<Pheromone> pheromones = new();

    public int[] tilePosition = new int[2];
    public MetaTile[] neighborTiles;
    public Vector3 tileSize;
    public float maxPheromoneStrength = 100f;

    public UQM tileType = UQM.DefaultTile;
    public Dictionary<UQM, TileBase> TileBaseByUQM;

    [Header("Visual Assets")]
    public TileBase defaultTile;
    public TileBase resourceTile;
    public TileBase queenTile;
    public TileBase blockerTile;

    private void OnEnable()
    {
        TileBaseByUQM = new Dictionary<UQM, TileBase>
        {
            [UQM.DefaultTile] = defaultTile,
            [UQM.Resource]    = resourceTile,
            [UQM.Queen]       = queenTile,
            [UQM.Blocker]     = blockerTile
        };
    }
}