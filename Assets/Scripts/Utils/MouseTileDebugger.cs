// using UnityEngine;
//
// public class MouseTileDebugger : MonoBehaviour
// {
//     [SerializeField] private Camera cam;          
//
//     private MapManager _map;                      
//     private Vector2Int _lastAxial = new(int.MinValue, int.MinValue);
//
//     private void Awake()
//     {
//         if (cam == null) cam = Camera.main;
//         _map = FindObjectOfType<MapManager>();
//     }
//
//     private void Update()
//     {
//         if (_map == null) return;                
//
//
//         Vector3 screen = Input.mousePosition;
//         screen.z = -cam.transform.position.z;   
//         Vector3 world = cam.ScreenToWorldPoint(screen);
//
//
//         Vector2Int axial = HexGridUtil.WorldToAxial(
//                                world,
//                                _map.HexRadius,
//                                _map.BoardOrigin);
//
//
//         if (axial == _lastAxial) return;        
//         _lastAxial = axial;
//         
//         if (_map.MetaTilesTMP.TryGetValue(axial, out GameObject go))
//         {
//             MetaTile meta = go.GetComponent<MetaTile>();
//             Debug.Log($"Mouse over tile {axial}  |  Qualifier = {meta.tileData.tileSpecialType}");
//         }
//         else
//         {
//             Debug.Log($"Mouse over empty space  |  Axial = {axial}");
//         }
//     }
// }
