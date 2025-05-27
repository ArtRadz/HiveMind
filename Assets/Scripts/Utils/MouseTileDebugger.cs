using UnityEngine;

/// <summary>
/// Logs the axial coord (and qualifier) of the tile under the mouse.
/// No UI, no selection — just debug output.
/// </summary>
public class MouseTileDebugger : MonoBehaviour
{
    [SerializeField] private Camera cam;          // assign or defaults to Camera.main

    private MapManager _map;                      // cache for radius/origin/dictionary
    private Vector2Int _lastAxial = new(int.MinValue, int.MinValue);

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        _map = FindObjectOfType<MapManager>();
    }

    private void Update()
    {
        if (_map == null) return;                // safety net if scene missing MapManager

        /* -----------------------------------------------------------
         * 1. Mouse -> world (XY)
         * --------------------------------------------------------- */
        Vector3 screen = Input.mousePosition;
        screen.z = -cam.transform.position.z;    // correct plane distance
        Vector3 world = cam.ScreenToWorldPoint(screen);

        /* -----------------------------------------------------------
         * 2. World -> axial
         * --------------------------------------------------------- */
        Vector2Int axial = HexGridUtil.WorldToAxial(
                               world,
                               _map.HexRadius,
                               _map.BoardOrigin);

        /* -----------------------------------------------------------
         * 3. Only log when we enter a *different* tile
         * --------------------------------------------------------- */
        if (axial == _lastAxial) return;         // cursor still on same hex
        _lastAxial = axial;

        /* -----------------------------------------------------------
         * 4. Fetch the MetaTile (if any) and log
         * --------------------------------------------------------- */
        if (_map.MetaTilesTMP.TryGetValue(axial, out GameObject go))
        {
            MetaTile meta = go.GetComponent<MetaTile>();
            Debug.Log($"Mouse over tile {axial}  |  Qualifier = {meta.tileData.tileSpecialType}");
        }
        else
        {
            Debug.Log($"Mouse over empty space  |  Axial = {axial}");
        }
    }
}
