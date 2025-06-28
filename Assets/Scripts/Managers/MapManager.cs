using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UQM = UniversalQualifierMarker;
/// <summary>
/// Generates and manages the hex tile grid. Handles tile instantiation, neighbor linking,
/// and positioning of visual tile objects. Subscribes to tick events for future expansion.
/// </summary>
public class MapManager : MonoBehaviour
{
	[Header("Grid Settings")]
	[SerializeField] private int gridWidth = 10;
	[SerializeField] private int gridHeight = 10;
	public float HexRadius { get; private set; }
	public Vector3 BoardOrigin { get; private set; }

	[Header("Tile Template")]
	[SerializeField] private TileBlueprint tileTemplate;

	[Header("Tile Prefab")]
	[SerializeField] private GameObject metaTilePrefab;

	[Header("References")]
	[SerializeField] private GameManager gM;
	[SerializeField] public Tilemap tilemap;

	public Dictionary<Vector2Int, GameObject> MetaTiles;
	private Dictionary<UQM, TileBase> TileBaseByUQM;

	private void Awake()
	{
		CreateMetaTileGrid();
	}

	private void Start()
	{
		if (gM == null)
		{
			gM = FindObjectOfType<GameManager>();
		}
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

	private void OnTick(float tickDuration)
	{
		// Hook reserved for future functionality
	}

	private void CreateMetaTileGrid()
	{
		Dictionary<Vector2Int, TileBlueprint> tileDatas = new();
		Dictionary<Vector2Int, GameObject> metaTiles = new();
		TileBaseByUQM = tileTemplate.TileBaseByUQM;

		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				Vector2Int key = new(x, y);
				TileBlueprint tileData = Instantiate(tileTemplate);
				tileData.tilePosition[0] = x;
				tileData.tilePosition[1] = y;

				GameObject metaTile = Instantiate(metaTilePrefab, Vector3.zero, Quaternion.identity);
				tileDatas.Add(key, tileData);
				metaTiles.Add(key, metaTile);
			}
		}

		SetTileDataNeighbors(tileDatas, metaTiles);
		PopulateTileMap(tileDatas);
		SetMetaTileData(tileDatas, metaTiles);
		PositionMetaTilesOnTilemap(metaTiles);
	}

	private void SetTileDataNeighbors(Dictionary<Vector2Int, TileBlueprint> tileDatas, Dictionary<Vector2Int, GameObject> metaTiles)
	{
		// Assigns neighboring MetaTiles to each TileBlueprint in the grid.
		// This implementation assumes a flat-top hexagonal layout, where vertical alignment alternates
		// between even and odd rows (offset layout).
		// 
		// Even rows shift neighbor lookup positions slightly left,
		// while odd rows shift them slightly right to account for the staggered hex structure.
		// The neighbor positions are computed accordingly for each row parity.

		foreach (var pair in tileDatas)
		{
			Vector2Int pos = pair.Key;
			MetaTile[] neighbors = new MetaTile[6];

			Vector2Int[] neighborKeys = (pos.y % 2 == 0)
				? new[]
				{
					new Vector2Int(pos.x + 1, pos.y),
					new Vector2Int(pos.x, pos.y + 1),
					new Vector2Int(pos.x - 1, pos.y + 1),
					new Vector2Int(pos.x - 1, pos.y),
					new Vector2Int(pos.x - 1, pos.y - 1),
					new Vector2Int(pos.x, pos.y - 1)
				}
				: new[]
				{
					new Vector2Int(pos.x + 1, pos.y),
					new Vector2Int(pos.x + 1, pos.y + 1),
					new Vector2Int(pos.x, pos.y + 1),
					new Vector2Int(pos.x - 1, pos.y),
					new Vector2Int(pos.x, pos.y - 1),
					new Vector2Int(pos.x + 1, pos.y - 1)
				};

			for (int i = 0; i < 6; i++)
			{
				metaTiles.TryGetValue(neighborKeys[i], out GameObject neighborGO);
				neighbors[i] = neighborGO ? neighborGO.GetComponent<MetaTile>() : null;
			}

			tileDatas[pos].neighborTiles = neighbors;
		}
	}

	private void PopulateTileMap(Dictionary<Vector2Int, TileBlueprint> tileDatas)
	{
		Tile sampleTile = tileDatas[new Vector2Int(0, 0)].defaultTile as Tile;
		Vector3 tileSize = sampleTile.sprite.bounds.size;

		HexRadius = tileSize.y * 0.5f;
		BoardOrigin = tilemap.GetCellCenterWorld(Vector3Int.zero);
		tilemap.layoutGrid.cellSize = tileSize;

		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				Vector3Int cellPos = new(x, y, 0);
				Vector2Int key = new(x, y);

				tilemap.SetTile(cellPos, tileDatas[key].defaultTile);
			}
		}
	}

	private void SetMetaTileData(Dictionary<Vector2Int, TileBlueprint> tileDatas, Dictionary<Vector2Int, GameObject> metaTiles)
	{
		foreach (var pair in metaTiles)
		{
			pair.Value.GetComponent<MetaTile>().Initialize(tileDatas[pair.Key], this);
		}

		MetaTiles = metaTiles;
	}

	private void PositionMetaTilesOnTilemap(Dictionary<Vector2Int, GameObject> metaTiles)
	{
		foreach (var pair in metaTiles)
		{
			var data = pair.Value.GetComponent<MetaTile>().tileData;
			Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(data.tilePosition[0], data.tilePosition[1], 0));
			pair.Value.transform.position = worldPos;
		}
	}

	public void UpdateTile(Vector3 tilePosition, UQM newTileUqm)
	{
		Vector3Int cellPos = tilemap.WorldToCell(tilePosition);
		tilemap.SetTile(cellPos, TileBaseByUQM[newTileUqm]);
	}
}
