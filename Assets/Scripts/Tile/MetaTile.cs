using System;
using System.Collections.Generic;
using UnityEngine;
using UQM = UniversalQualifierMarker;

public class MetaTile : MonoBehaviour
{

    [SerializeField] public TileData tileData;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject tileGO;
    private bool hasQueen = false;
    private bool hasResource = false;
    private bool hasBlocker = false;
    private GameObject objQueen;
    private GameManager gM;

    public bool CheckSelfForUpdate()
    {
        if (hasQueen != (tileData.tileSpecialType == UQM.Queen))
        {
            if (tileData.tileSpecialType == UQM.Queen)
            {
                CreateQueen();
            }
            else if (tileData.tileSpecialType != UQM.Queen && objQueen != null)
            {
                Destroy(objQueen);
            }
            hasQueen = tileData.tileSpecialType == UQM.Queen;
            return true;
        }

        if (hasResource != (tileData.tileSpecialType == UQM.Resource))
        {
            hasResource = tileData.tileSpecialType == UQM.Resource;
           
            return true;
        }

        if (hasBlocker != (tileData.tileSpecialType == UQM.Blocker))
        {
            hasBlocker = tileData.tileSpecialType == UQM.Blocker;
            return true;
        }

        return false;
    }

    public void Initialize(TileBlueprint data)
    {
        tileData.pheromones = data.pheromones;
        foreach (Pheromone pher in tileData.pheromones)
        {
            pher.Distance = null;
        }

        gM = FindObjectOfType<GameManager>();
        gM.onTick.AddListener(OnTick);
        tileData.tileSpecialType = data.tileType;
        tileData.tilePosition = data.tilePosition;
        tileData.neighborTiles = data.neighborTiles;
        tileData.tileSize = data.tileSize;
        tileData.PheromonalDecayValuePerTick = data.PheromonalDecayValuePerTick;
        gameObject.name = "Tile_" + tileData.tilePosition[1] + "_" + tileData.tilePosition[0];
        
    }
    private void OnTick(float tickDuration)
    {
        // Debug.Log(gameObject.name + " status is : pheromones " +tileData.pheromones+ " has queen " + tileData.hasQueen + " has resource " + tileData.hasResource );
        DecayPheromone();
    }

    private void DecayPheromone()
    {
        foreach (var Phero in tileData.pheromones)
        {
            if (Phero.Strength > 0)
            {
                Phero.Strength -= tileData.PheromonalDecayValuePerTick;
                if (Phero.Strength <= 0)
                {
                    Phero.Distance = null;
                    Phero.Strength = 0;
                }
            }
        }
    }
    private void CreateQueen()
    {
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        objQueen = Instantiate(queenPrefab, spawnPosition, spawnRotation);
        objQueen.GetComponent<Queen>().InitQueenData(this);
    }

    public TileData GetTileData()
    {
        return tileData;
    }

    public Transform GetTileTransform()
    {
        return tileGO.transform;
    }

    public void UpdatePheromone((UQM phType, int? distance) droneCounter, float markStrength)
    {
        foreach (Pheromone pher in tileData.pheromones)
        {
            if (pher.Type == droneCounter.phType)
            {
                Debug.Log("tile updated " + droneCounter.phType+" "+ droneCounter.distance);
                pher.Distance = droneCounter.distance;
                pher.Strength = Mathf.Clamp(pher.Strength + markStrength, 0f, tileData.maxPheromoneStrength);
            }
        }
    }
}