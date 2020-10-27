using System;
using UnityEngine;

[ExecuteAlways]
public class Grid : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] [Min(0)] private float xCellSize;
    [SerializeField] [Min(0)] private float yCellSize;
    
    [Space]
    [SerializeField] [Min(0)] private int xCellsCount;
    [SerializeField] [Min(0)] private int yCellsCount;

    [Space]
    [SerializeField] [Range(0, 1)] private float xPivot;
    [SerializeField] [Range(0, 1)] private float yPivot;
    
    [Header("Debug")]
    [SerializeField] private Color gridGizmosColor;

    private Plane _plane = new Plane(Vector3.forward, Vector3.zero);

    public float XCellSize => xCellSize;
    public float YCellSize => yCellSize;

    public bool Raycast(Ray ray, out Vector3 closestCellCenter)
    {
        closestCellCenter = Vector3.zero;
        
        if (!_plane.Raycast(ray, out float enter))
            return false;

        closestCellCenter = ClosestCellCenter(ray.GetPoint(enter));
        return true;
    }
    
    public Vector3 ClosestCellCenter(Vector3 point)
    {
        Vector3 projection = _plane.ClosestPointOnPlane(transform.TransformPoint(point));

        Vector3 xLinesOffset = Vector3.up * yCellSize;
        Vector3 yLinesOffset = Vector3.right * xCellSize;
        
        Vector3 xLinesTo =  yLinesOffset * xCellsCount;
        Vector3 yLinesTo =  xLinesOffset * yCellsCount;
        
        Vector3 xPivotOffset = xLinesTo * -xPivot;
        Vector3 yPivotOffset = yLinesTo * -yPivot;
        Vector3 pivotOffset = xPivotOffset + yPivotOffset;

        int xLeftCell = (int) (pivotOffset.x / xCellSize);
        int xRightCell = (int) (xLinesTo.x + pivotOffset.x / xCellSize);
        
        int yLeftCell = (int) (pivotOffset.y / yCellSize);
        int yRightCell = (int) (yLinesTo.y + pivotOffset.y / yCellSize);

        int xCell = (int) Mathf.Clamp(projection.x / xCellSize, xLeftCell, xRightCell);
        int yCell = (int) Mathf.Clamp(projection.y / yCellSize, yLeftCell, yRightCell);

        return transform.InverseTransformPoint(new Vector3(xCell * xCellSize, yCell * yCellSize, 0));
    }

    private void Draw(Action<Vector3, Vector3> drawLine)
    {
        int xLinesCount = yCellsCount + 1;
        int yLinesCount = xCellsCount + 1;
        
        Vector3 xLinesOffset = Vector3.up * yCellSize;
        Vector3 yLinesOffset = Vector3.right * xCellSize;
        
        Vector3 xLinesTo =  yLinesOffset * xCellsCount;
        Vector3 yLinesTo =  xLinesOffset * yCellsCount;
        
        Vector3 xPivotOffset = xLinesTo * -xPivot;
        Vector3 yPivotOffset = yLinesTo * -yPivot;
        Vector3 pivotOffset = xPivotOffset + yPivotOffset;
        
        DrawParallelLines(xLinesCount, pivotOffset, xLinesTo + pivotOffset,
            xLinesOffset, drawLine);
        DrawParallelLines(yLinesCount, pivotOffset, yLinesTo + pivotOffset,
            yLinesOffset, drawLine);
    }

    private static void DrawParallelLines(int count, Vector3 from, Vector3 to, Vector3 offset, Action<Vector3, Vector3> draw)
    {
        for (int i = 0; i < count; ++i)
        {
            draw(from, to);
            from += offset;
            to += offset;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gridGizmosColor;
        Draw(Gizmos.DrawLine);
    }
#endif
}
