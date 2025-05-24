using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UQM = UniversalQualifierMarker;

public class MapManager : MonoBehaviour
{
    [Header("Grid Settings")] [SerializeField]
    private int gridWidth = 10;

    [SerializeField] private int gridHeight = 10;

    [Header("Tile Template")] [SerializeField]
    private TileBlueprint tileTemplate;

    [Header("Tile Prefab")] [SerializeField]
    private GameObject metaTilePrefab;

    [Header("References")] [SerializeField]
    private GameManager gM;

    [SerializeField] private Tilemap tilemap;
    
    public Dictionary<Vector2Int, GameObject> MetaTilesTMP;
    // private Vector3 tileSize;

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
        TMPTIleUpdate();
    }

    private void TMPTIleUpdate()
    {
        foreach (var tilePair in MetaTilesTMP)
        {
            MetaTile metaTile = tilePair.Value.GetComponent<MetaTile>();
            bool hasChanged = metaTile.CheckSelfForUpdate(); //TODO this is a tmp implementation until I implement UI
            if (hasChanged)
            {
                TileBlueprint _currentTileData = Instantiate(tileTemplate);
                TileBase tileToSet = _currentTileData.defaultTile;
                if (metaTile.tileData.tileSpecialType == UQM.Queen)
                {
                    tileToSet = _currentTileData.queenTile;
                }
                else if (metaTile.tileData.tileSpecialType==UQM.Resource)
                {
                    tileToSet = _currentTileData.resourceTile;
                }
                else if (metaTile.tileData.tileSpecialType==UQM.Blocker)
                {
                    tileToSet = _currentTileData.blockerTile;
                }
                Vector3Int cellPos = new Vector3Int(metaTile.tileData.tilePosition[0], metaTile.tileData.tilePosition[1], 0);
                
                tilemap.SetTile(cellPos, tileToSet);
            }
        }
    }
    private void CreateMetaTileGrid()
    {
        Dictionary<Vector2Int, TileBlueprint> _tileDatas = new Dictionary<Vector2Int, TileBlueprint>();
        Dictionary<Vector2Int, GameObject> _metaTiles = new Dictionary<Vector2Int, GameObject>();
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int currentKey = new Vector2Int(x, y);
                TileBlueprint _currentTileData;
                _currentTileData = Instantiate(tileTemplate);
                _currentTileData.tilePosition[0] = x;
                _currentTileData.tilePosition[1] = y;
                GameObject _currentMetaTile = Instantiate(metaTilePrefab, new Vector3(0, 0, 0), Quaternion.identity);
                _tileDatas.Add(currentKey, _currentTileData);
                _metaTiles.Add(currentKey, _currentMetaTile);
            }
        }

        SetTileDataNeighbors(_tileDatas, _metaTiles);
        PopulateTileMap(_tileDatas);
        SetMetaTileData(_tileDatas,_metaTiles);
        PositionMetaTilesOnTilemap(_metaTiles);
    }

    private void SetTileDataNeighbors(Dictionary<Vector2Int, TileBlueprint> _tileDatas,
        Dictionary<Vector2Int, GameObject> _metaTiles)
    {
        foreach (var _tileData in _tileDatas)
        {
            Vector2Int _position = _tileData.Key;
            MetaTile[] _neighborArray = new MetaTile[6];
            int positionX = _position.x;
            int positionY = _position.y;


            Vector2Int[] _neighborKeys;

            if (positionY % 2 == 0)
            {
                _neighborKeys = new Vector2Int[]
                {
                    new Vector2Int(positionX + 1, positionY),
                    new Vector2Int(positionX, positionY + 1),
                    new Vector2Int(positionX - 1, positionY + 1),
                    new Vector2Int(positionX - 1, positionY),
                    new Vector2Int(positionX - 1, positionY - 1),
                    new Vector2Int(positionX, positionY - 1)
                };
            }
            else
            {
                _neighborKeys = new Vector2Int[]
                {
                    new Vector2Int(positionX + 1, positionY),
                    new Vector2Int(positionX + 1, positionY + 1),
                    new Vector2Int(positionX, positionY + 1),
                    new Vector2Int(positionX - 1, positionY),
                    new Vector2Int(positionX, positionY - 1),
                    new Vector2Int(positionX + 1, positionY - 1)
                };
            }

            int i = 0;
            foreach (var key in _neighborKeys)
            {
                if (_metaTiles.ContainsKey(key))
                {
                    GameObject _currentNeighbour = _metaTiles[key];
                    _neighborArray[i] = _currentNeighbour.GetComponent<MetaTile>();
                }
                else
                {
                    _neighborArray[i] = null;
                }

                i++;
            }

            _tileDatas[_position].neighborTiles = _neighborArray;
        }
    }

    private void PopulateTileMap( Dictionary<Vector2Int,TileBlueprint> _tileDatas)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector2Int key = new Vector2Int(x, y);
                tilemap.SetTile(cellPos,_tileDatas[key].defaultTile);
                Tile tile = _tileDatas[key].defaultTile as Tile;
                Vector3 tileSize = tile.sprite.bounds.size;
                tilemap.layoutGrid.cellSize = tileSize;
                _tileDatas[key].tileSize = tileSize;
            }
        }
    }

    private void SetMetaTileData( Dictionary<Vector2Int,TileBlueprint> _tileDatas, Dictionary<Vector2Int,GameObject> _metaTiles)
    {
        foreach (var metaTile in _metaTiles)
        {
            Vector2Int key = metaTile.Key;
            metaTile.Value.GetComponent<MetaTile>().Initialize(_tileDatas[key]);
        }
        MetaTilesTMP = _metaTiles;
     //TODO replace with UI system current is workaround solution   
    }

    private void PositionMetaTilesOnTilemap(Dictionary<Vector2Int, GameObject> _metaTiles)
    {
        foreach (var metaTile in _metaTiles)
        {
            var tileInfo = metaTile.Value.GetComponent<MetaTile>().tileData;
            int x = tileInfo.tilePosition[0];
            int y = tileInfo.tilePosition[1];
            Vector3 worldPos = tilemap.CellToWorld(new Vector3Int(x, y, 0));
            metaTile.Value.GetComponent<Transform>().position = worldPos;

        }
    }
}