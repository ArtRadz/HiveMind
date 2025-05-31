using UnityEngine;
using TMPro;
using UQM = UniversalQualifierMarker;

public class TilePainter : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private TextMeshProUGUI selectionText;

    private UQM[] _brushes;
    private int _currentIndex;

    void Start()
    {
        // initialize brush list and UI
        _brushes     = (UQM[])System.Enum.GetValues(typeof(UQM));
        _currentIndex = 0;
        UpdateSelectionDisplay();

        // fallback assignments
        if (cam == null)       cam       = Camera.main;
        if (mapManager == null) mapManager = FindObjectOfType<MapManager>();
    }

    void Update()
    {
        // right-click to cycle brush
        if (Input.GetMouseButtonDown(1))
            Cycle(1);

        // left-click to paint
        if (Input.GetMouseButtonDown(0))
        {
            // 1) project mouse into world at the tilemap's Z-plane
            Vector3 mouse = Input.mousePosition;
            mouse.z       = mapManager.BoardOrigin.z - cam.transform.position.z;
            Vector3 world = cam.ScreenToWorldPoint(mouse);

            // 2) ask the Tilemap for the cell under that world point
            Vector3Int cell = mapManager.tilemap.WorldToCell(world);
            Vector2Int key  = new Vector2Int(cell.x, cell.y);

            // 3) lookup and paint
            var metaGO = mapManager.MetaTiles[key];
            metaGO.GetComponent<MetaTile>()
                .TrySetSpecial(_brushes[_currentIndex]);
        }
    }

    void Cycle(int dir)
    {
        _currentIndex = (_currentIndex + dir + _brushes.Length) % _brushes.Length;
        UpdateSelectionDisplay();
    }

    void UpdateSelectionDisplay()
    {
        if (selectionText != null)
            selectionText.text = _brushes[_currentIndex].ToString();
    }
}