using UnityEngine;

[ExecuteAlways]
public class TilePlacer : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Camera gameModeCamera;
    [SerializeField] private Color gizmosColor;

    private Vector3? _closestGridCenter;

    private void Update()
    {
        if(grid == null)
            return;

        Camera raycastCamera = Application.isPlaying ? gameModeCamera : Camera.current;
        
        if(raycastCamera == null)
            return;
        
        Ray cameraRay = raycastCamera.ScreenPointToRay(Input.mousePosition);
        if (grid.Raycast(cameraRay, out Vector3 closestGridCenter))
            _closestGridCenter = closestGridCenter;
        else
            _closestGridCenter = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_closestGridCenter == null)
            return;
        
        Gizmos.color = gizmosColor;
        Gizmos.matrix = grid.transform.localToWorldMatrix;
        Vector3 center = grid.transform.TransformPoint(_closestGridCenter.Value);
        
        Vector3 from = center - new Vector3(grid.XCellSize / 2, grid.YCellSize / 2);
        Vector3 to = center + new Vector3(grid.XCellSize / 2, grid.YCellSize / 2);
        Gizmos.DrawLine(from, to);
        
        from = center - new Vector3(grid.XCellSize / 2, -grid.YCellSize / 2);
        to = center + new Vector3(grid.XCellSize / 2, -grid.YCellSize / 2);
        Gizmos.DrawLine(from, to);
    }
#endif
}