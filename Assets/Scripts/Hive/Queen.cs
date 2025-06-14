using UnityEngine;

public class Queen : MonoBehaviour
{
	[Header("Resource Settings")]
	[SerializeField] private float initialResourceCount = 1000f;

	[Header("Production Settings")]
	[SerializeField] private int productionIntervalTicks = 10;
	[SerializeField] private int maxDronesPerCycle = 3;
	[SerializeField] private float costPerDrone = 50f;

	[Header("Drone Settings")]
	[SerializeField] private GameObject dronePrefab;
	[SerializeField] private Transform droneSpawnPoint;

	[Header("Manager Reference")]
	[SerializeField] private GameManager gM;

	private float currentResource;
	private int tickCounter = 0;
	private MetaTile myTile;

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

	private void OnTick(float tickDuration)
	{
		tickCounter++;

		if (tickCounter >= productionIntervalTicks)
		{
			ProduceDrones();
			tickCounter = 0;
		}
	}

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

	private void SpawnDrone()
	{
		if (dronePrefab == null || droneSpawnPoint == null)
		{
			return;
		}

		GameObject drone = Instantiate(dronePrefab, droneSpawnPoint.position, Quaternion.identity);
		drone.GetComponent<DroneBase>().InitDrone(myTile);
	}

	public void DeliverResource(float amount)
	{
		currentResource += amount;
	}

	private void OnResourceChanged(object newValue)
	{
		// Intentionally left empty; hook preserved for future display or debug
	}
}
