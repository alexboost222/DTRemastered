
using System;
using UnityEngine;

public class LineRendererGrid : Grid
{
    [SerializeField] [Min(0)] private float lineWidth;
    [SerializeField] private Material material;

    private void DrawLine(Vector3 from, Vector3 to)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        
        Transform lineTransform = lineRenderer.transform;
        lineTransform.parent = transform;
        lineTransform.localPosition = Vector3.zero;
        lineTransform.localRotation = Quaternion.identity;

        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);

        lineRenderer.material = material;
        lineRenderer.useWorldSpace = false;
    }

    private void Awake()
    {
        if(!Application.isPlaying)
            return;
        
        Draw(DrawLine);
    }
}
