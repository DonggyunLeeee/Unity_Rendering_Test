using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawShapeVector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class DrawCircleWithLineRenderer : MonoBehaviour
{
    public float radius = 1f;
    public int segments = 100;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 1;
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.loop = true;

        Vector3[] points = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            points[i] = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }

        lineRenderer.SetPositions(points);
    }
}

public class DrawPolygonWithVectors : MonoBehaviour
{
    public Vector2[] vertices;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (vertices.Length < 2)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 start = vertices[i];
            Vector2 end = vertices[(i + 1) % vertices.Length];
            Gizmos.DrawLine(start, end);
        }
    }
}
