using UnityEngine;
using UnityEngine.Events;

public class Queen : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private float initialResourceCount = 1000f;
    [SerializeField] private float resourceDeliveryValue = 100f; // Amount added per delivery

    [Header("Production Settings")]
    [SerializeField] private int productionIntervalTicks = 10; // Produce every y ticks
    [SerializeField] private int maxDronesPerCycle = 3;         // Max drones spawned per cycle
    [SerializeField] private float costPerDrone = 50f;            // Resource cost per drone spawn

    [Header("Drone Settings")]
    [SerializeField] private GameObject dronePrefab;            // Prefab for drones to spawn
    [SerializeField] private Transform droneSpawnPoint;         // Spawn position (could be this.transform)

    private float currentResource;
    private int tickCounter = 0;
    private MetaTile myTile;

    // Reference to the central GameManager for tick events.
    [SerializeField] private GameManager gM;

    private void Start()
    {
        currentResource = initialResourceCount;
        if (gM == null)
        {
            gM = FindObjectOfType<GameManager>();
        }
        gM.onTick.AddListener(OnTick);
    }
    
    private void OnEnable()
    {
        if (gM != null)
        {
            gM.onTick.AddListener(OnTick);
        }
    }

    private void OnDisable()
    {
        if (gM != null)
        {
            gM.onTick.RemoveListener(OnTick);
        }
    }

    public void InitQueenData(MetaTile parentTile)
    {
        myTile = parentTile;
    }
    private void OnTick()
    {
        tickCounter++;

        // When we've reached the production interval...
        if (tickCounter >= productionIntervalTicks)
        {
            ProduceDrones();
            tickCounter = 0; // reset tick counter
        }
    }

    // Produces up to maxDronesPerCycle if enough resources are available.
    private void ProduceDrones()
    {
        int dronesSpawned = 0;
        while (dronesSpawned < maxDronesPerCycle && currentResource >= costPerDrone)
        {
            SpawnDrone();
            currentResource -= costPerDrone;
            dronesSpawned++;
        }
    }

    // Instantiates a drone prefab at the spawn point.
    private void SpawnDrone()
    {
        if (dronePrefab != null && droneSpawnPoint != null)
        {
            GameObject drone = Instantiate(dronePrefab, droneSpawnPoint.position, Quaternion.identity);
            drone.GetComponent<DroneBase>().InitDrone(myTile);
        }
        else
        {
            Debug.LogWarning("Drone prefab or spawn point not assigned.");
        }
    }

    // Called externally by a drone when delivering resource.
    public void DeliverResource(float amount)
    {
        currentResource += amount;
        Debug.Log($"Quin received {amount} resources. New total: {currentResource}");
    }
}
