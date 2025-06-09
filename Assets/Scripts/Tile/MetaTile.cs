using UnityEngine;
using UQM = UniversalQualifierMarker;

public class MetaTile : MonoBehaviour
{
    [SerializeField] public TileData tileData;
    [SerializeField] private GameObject queenPrefab;
    [SerializeField] private GameObject tileGO;

    private GameObject objQueen;
    private GameManager gM;
    private MapManager mapManager;
    private Vector2Int axialCoords;

    public void Initialize(TileBlueprint data, MapManager mp)
    {
        mapManager = mp;

        tileData.pheromones = data.pheromones;
        foreach (Pheromone pher in tileData.pheromones)
            pher.Distance = null;

        gM = FindObjectOfType<GameManager>();
        gM.onTick.AddListener(OnTick);

        tileData.tileSpecialType = data.tileType;
        tileData.tilePosition = data.tilePosition;
        tileData.neighborTiles = data.neighborTiles;
        tileData.tileSize = data.tileSize;
        tileData.PheromonalDecayValuePerTick = data.PheromonalDecayValuePerTick;

        gameObject.name = $"Tile_{tileData.tilePosition[1]}_{tileData.tilePosition[0]}";
        axialCoords = new Vector2Int(tileData.tilePosition[0], tileData.tilePosition[1]);
    }

    private void OnTick(float tickDuration)
    {
        DecayPheromone();
    }

    private void DecayPheromone()
    {
        foreach (var pher in tileData.pheromones)
        {
            if (pher.Strength > 0f)
            {
                pher.Strength -= tileData.PheromonalDecayValuePerTick;
                
            }
            if (pher.Strength <= 0f)
            {
                pher.Strength = 0f;
                pher.Distance = null;
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
                pher.Distance = droneCounter.distance;
                pher.Strength = Mathf.Clamp(pher.Strength + markStrength, 0f, tileData.maxPheromoneStrength);
            }
        }
    }

    public Vector3 GetRandomPoint()
    {
        return HexGridUtil.GetRandomPointInCell(
            axialCoords,
            mapManager.HexRadius,
            mapManager.BoardOrigin
        );
    }

    public void TrySetSpecial(UQM newUQM)
    {
        if (newUQM == tileData.tileSpecialType)
            return;

        tileData.tileSpecialType = newUQM;
        if (newUQM == UQM.Queen)
            CreateQueen();
        else if (objQueen != null)
            Destroy(objQueen);

        mapManager.UpdateTile(transform.position, newUQM);
    }

    
}
