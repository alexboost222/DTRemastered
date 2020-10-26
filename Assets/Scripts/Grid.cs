using System;
using UnityEngine;

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

        int horLinesCount = yCellsCount + 1;
        int vertLinesCount = xCellsCount + 1;
        
        Vector3 horLinesOffset = Vector3.up * yCellSize;
        Vector3 vertLinesOffset = Vector3.right * xCellSize;
        
        Vector3 horLinesTo =  vertLinesOffset * xCellsCount;
        Vector3 vertLinesTo =  horLinesOffset * yCellsCount;
        
        Vector3 xPivotOffset = horLinesTo * -xPivot;
        Vector3 yPivotOffset = vertLinesTo * -yPivot;
        Vector3 pivotOffset = xPivotOffset + yPivotOffset;
        
        DrawParallelLines(horLinesCount, pivotOffset, horLinesTo + pivotOffset,
            horLinesOffset, Gizmos.DrawLine);
        DrawParallelLines(vertLinesCount, pivotOffset, vertLinesTo + pivotOffset,
            vertLinesOffset, Gizmos.DrawLine);
    }
#endif
}
