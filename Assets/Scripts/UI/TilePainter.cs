using UnityEngine;
using TMPro;
using UQM = UniversalQualifierMarker;

public class TilePainter : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private TextMeshProUGUI selectionText;

    private UQM[] brushes;
    private int currentIndex;

    private void Start()
    {
        brushes = (UQM[])System.Enum.GetValues(typeof(UQM));
        currentIndex = 0;
        UpdateSelectionDisplay();

        if (cam == null) cam = Camera.main;
        if (mapManager == null) mapManager = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cycle(1);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.z = mapManager.BoardOrigin.z - cam.transform.position.z;

            Vector3 world = cam.ScreenToWorldPoint(mouse);
            Vector3Int cell = mapManager.tilemap.WorldToCell(world);
            Vector2Int key = new Vector2Int(cell.x, cell.y);

            if (mapManager.MetaTiles.ContainsKey(key))
            {
                GameObject metaGO = mapManager.MetaTiles[key];
                metaGO.GetComponent<MetaTile>().TrySetSpecial(brushes[currentIndex]);
            }
        }
    }

    private void Cycle(int dir)
    {
        currentIndex = (currentIndex + dir + brushes.Length) % brushes.Length;
        UpdateSelectionDisplay();
    }

    private void UpdateSelectionDisplay()
    {
        if (selectionText != null)
        {
            selectionText.text = brushes[currentIndex].ToString();
        }
    }
}