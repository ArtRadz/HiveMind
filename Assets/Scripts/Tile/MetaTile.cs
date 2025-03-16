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

        return false;
    }

    public void Initialize(TileData data)
    {
        gM = FindObjectOfType<GameManager>();
        gM.onTick.AddListener(OnTick);
        tileData.tilePosition = data.tilePosition;
        tileData.neighborTiles = data.neighborTiles;
        tileData.tileSize = data.tileSize;
        tileData.pheromones = data.pheromones;
        tileData.PheromonalDecayValuePerTick = data.PheromonalDecayValuePerTick;
        gameObject.name = "Tile_" + tileData.tilePosition[0] + "_" + tileData.tilePosition[1];
    }
    private void OnTick()
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

    public void UpdatePheromone(int? distance, UQM type, float markStrength)
    {
        foreach (Pheromone pher in tileData.pheromones)
        {
            if (pher.Type == type)
            {
                pher.Distance = distance;
                pher.Strength += markStrength;
            }
        }
        // foreach (Pheromone pher in tileData.pheromones)
        // {
        //     if(pher.Type == pheromone.Type.Queen)
        //     {
        //         if ((queen != null && queen <= pher.Distance) || (queen != null && pher.Distance == null))
        //         {
        //             pher.Distance = queen;
        //             pher.Strength += markStrength;
        //         }
        //     }
        //     if(pher.Type == pheromone.Type.Resource)
        //     {
        //         if ((resource != null && resource <= pher.Distance) || (resource != null && pher.Distance == null))
        //         {
        //             pher.Distance = resource;
        //             pher.Strength += markStrength;
        //         }
        //     }
        // }
    }
}