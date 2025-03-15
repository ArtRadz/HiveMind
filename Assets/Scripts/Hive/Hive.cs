using System;
using System.Collections;
using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] private int resourceAStartingAmount, resourceBStartingAmount;
    [SerializeField] private int resourceACurrentAmount, resourceBCurrentAmount;
    [SerializeField] private int droneCostResourceA, droneCostResourceB;
    [SerializeField] private int dronesBatch;
    [SerializeField] private float repeatSpawnInterval;
    [SerializeField] private GameObject drone;

    [SerializeField] private float spawnProbability;

    private void Start()
    {
        resourceACurrentAmount = resourceAStartingAmount;
        resourceBCurrentAmount = resourceBStartingAmount;
        for (int i =0; i<dronesBatch; i++)
        {
            Instantiate(drone, transform.position, Quaternion.identity);
        }
        StartCoroutine(SpawnDroneLoop());
    }

    IEnumerator SpawnDroneLoop()
    {
        while (true)
        {
            if (IfCanSpawn())
            {
                SpawnDrone();
            }
            yield return new WaitForSeconds(repeatSpawnInterval);
        }
    }

    private bool IfCanSpawn()
    {
        bool hasEnoughResources = resourceACurrentAmount >= droneCostResourceA && resourceBCurrentAmount >= droneCostResourceB;
        
        float randomChance = UnityEngine.Random.Range(0f, 1f);
        
        bool canSpawn = hasEnoughResources && (randomChance <= spawnProbability);

        return canSpawn;
    }
    
    private void SpawnDrone()
    {
        for (int i =0; i<dronesBatch; i++)
        {
            Instantiate(drone, transform.position, Quaternion.identity);
        }
        ConsumeResources();
    }
    private void ConsumeResources()
    {
        resourceACurrentAmount -= droneCostResourceA;
        resourceBCurrentAmount -= droneCostResourceB;
    }
    public void AddResources(int resourceAAmount, int resourceBAmount)
    {
        resourceACurrentAmount += resourceAAmount;
        resourceBCurrentAmount += resourceBAmount;
    }
}
