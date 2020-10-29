using UnityEngine;

[ExecuteAlways]
public class TilePlacer : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Camera gameModeCamera;

    private Vector3? _closestCellPoint;

    private void Update()
    {
        if(grid == null)
            return;
        
        if(gameModeCamera == null)
            return;
        
        Ray cameraRay = gameModeCamera.ScreenPointToRay(Input.mousePosition);
        if (grid.RaycastClosestCellCenter(cameraRay, out Vector3 enter))
            _closestCellPoint = enter;
        else
            _closestCellPoint = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(_closestCellPoint == null)
            return;
            
        Gizmos.matrix = grid.transform.localToWorldMatrix;
        Vector3 center = grid.transform.InverseTransformPoint(_closestCellPoint.Value);
        Gizmos.color = Color.red;
        
        Gizmos.DrawSphere(center, 0.1f);
        
        Vector3 from = center - new Vector3(grid.XCellSize / 2, grid.YCellSize / 2);
        Vector3 to = center + new Vector3(grid.XCellSize / 2, grid.YCellSize / 2);
        Gizmos.DrawLine(from, to);
        
        from = center - new Vector3(grid.XCellSize / 2, -grid.YCellSize / 2);
        to = center + new Vector3(grid.XCellSize / 2, -grid.YCellSize / 2);
        Gizmos.DrawLine(from, to);
    }
#endif
}