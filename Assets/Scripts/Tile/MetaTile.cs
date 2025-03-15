using System;
using System.Collections.Generic;
using UnityEngine;

public class MetaTile : MonoBehaviour
{

    [SerializeField] public TileInfo tileInfo;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject tileGO;
    private bool hasQueen = false;
    private bool hasResource = false;
    private GameObject objQueen;
    private GameManager gM;

    public bool CheckSelfForUpdate()
    {
        if (hasQueen != tileInfo.hasQueen)
        {
            if (tileInfo.hasQueen)
            {
                CreateQueen();
            }
            else if (!tileInfo.hasQueen && objQueen != null)
            {
                Destroy(objQueen);
            }
            hasQueen = tileInfo.hasQueen;
            return true;
        }

        if (hasResource != tileInfo.hasResource)
        {
            hasResource = tileInfo.hasResource;
           
            return true;
        }

        return false;
    }

    public void Initialize(TileData data)
    {
        gM = FindObjectOfType<GameManager>();
        gM.onTick.AddListener(OnTick);
        tileInfo.tilePosition = data.tilePosition;
        tileInfo.neighborTiles = data.neighborTiles;
        tileInfo.tileSize = data.tileSize;
        tileInfo.pheromones = data.pheromones;
        tileInfo.PheromonalDecayValuePerTick = data.PheromonalDecayValuePerTick;
        gameObject.name = "Tile_" + tileInfo.tilePosition[0] + "_" + tileInfo.tilePosition[1];
    }
    private void OnTick()
    {
        // Debug.Log(gameObject.name + " status is : pheromones " +tileInfo.pheromones+ " has queen " + tileInfo.hasQueen + " has resource " + tileInfo.hasResource );
        DecayPheromone();
    }

    private void DecayPheromone()
    {
        foreach (var Phero in tileInfo.pheromones)
        {
            if (Phero.Strength > 0)
            {
                Phero.Strength -= tileInfo.PheromonalDecayValuePerTick;
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

    public TileInfo GetTileInfo()
    {
        return tileInfo;
    }

    public Transform GetTileTransform()
    {
        return tileGO.transform;
    }

    public void UpdatePheromone(int? queen, int? resource, float markStrength)
    {
        foreach(Pheromone pher in tileInfo.pheromones)
        {
            if(pher.Type == Pheromone.PheromoneType.Queen)
            {
                if ((queen != null && queen <= pher.Distance) || (queen != null && pher.Distance == null))
                {
                    pher.Distance = queen;
                    pher.Strength += markStrength;
                }
            }
            if(pher.Type == Pheromone.PheromoneType.Resource)
            {
                if ((resource != null && resource <= pher.Distance) || (resource != null && pher.Distance == null))
                {
                    pher.Distance = resource;
                    pher.Strength += markStrength;
                }
            }
        }
    }
}