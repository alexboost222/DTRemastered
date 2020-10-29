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

    private Plane _localPlane = new Plane(Vector3.forward, Vector3.zero);

    public float XCellSize => xCellSize;
    public float YCellSize => yCellSize;
    
    private Plane WorldPlane => new Plane(transform.forward, transform.position);

    public bool RaycastClosestCellCenter(Ray worldRay, out Vector3 center)
    {
        center = Vector3.zero;

        if (!WorldPlane.Raycast(worldRay, out float enter))
            return false;

        center = GetClosestCellCenter(worldRay.GetPoint(enter));
        return true;
    }

    public Vector3 GetClosestCellCenter(Vector3 worldPoint)
    {
        Vector3 projection = _localPlane.ClosestPointOnPlane(transform.InverseTransformPoint(worldPoint));
        
        Vector3 xLinesOffset = Vector3.up * yCellSize;
        Vector3 yLinesOffset = Vector3.right * xCellSize;
        
        Vector3 xLinesTo =  yLinesOffset * xCellsCount;
        Vector3 yLinesTo =  xLinesOffset * yCellsCount;
        
        Vector3 xPivotOffset = xLinesTo * -xPivot;
        Vector3 yPivotOffset = yLinesTo * -yPivot;
        Vector3 pivot = xPivotOffset + yPivotOffset;

        Vector3 pivotToProjection = projection - pivot;

        int xCellNumber = (int) Mathf.Clamp(pivotToProjection.x / xCellSize, 0, xCellsCount - 1);
        int yCellNumber = (int) Mathf.Clamp(pivotToProjection.y / yCellSize, 0, yCellsCount - 1);

        float x = (float) (xCellNumber + 0.5) * xCellSize;
        float y = (float) (yCellNumber + 0.5) * yCellSize;

        return transform.TransformPoint(pivot + new Vector3(x, y, 0));
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
        Vector3 pivot = xPivotOffset + yPivotOffset;
        
        DrawParallelLines(xLinesCount, pivot, xLinesTo + pivot,
            xLinesOffset, drawLine);
        DrawParallelLines(yLinesCount, pivot, yLinesTo + pivot,
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
    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gridGizmosColor;
        Draw(Gizmos.DrawLine);
    }
#endif
}
