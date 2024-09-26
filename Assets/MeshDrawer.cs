using UnityEngine;
using System.Collections.Generic;
using Clipper2Lib;

public class MeshDrawer : MonoBehaviour
{
    public enum ShapeType { Square, Circle, Polygon, COUNT};

    public struct CircleData
    {
        public Vector3 center;
        public float radius;

        public CircleData(Vector3 center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }
    }

    public int segments = 100; // ���� �׸� �� ����� ���׸�Ʈ ��
    public GameObject circlePrefab; // ���� �׸� ������

    private Vector3 firstClickPosition; // ù ��° Ŭ�� ��ġ
    private bool isFirstClick = true; // ù ��° Ŭ�� ����
    private bool isActive = false; // �� �׸��� Ȱ��ȭ ����
    private List<CircleData> existingCircles = new List<CircleData>(); // ���� �� ���
    private float zoomSpeed = 0.1f; // �� �ӵ�
    private float minScale = 0.1f;  // �ּ� ũ��
    private float maxScale = 10f;   // �ִ� ũ��
    private int 

    void Start()
    {
        // ���� �� ���� �׸� �������� ����
        // �������� ���� �׸��� ���� �⺻ ���ø��Դϴ�
        
    }

    void Update()
    {
        if (!isActive)
            return;

        // ���콺 ���� Ŭ��
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPosition.z = 0;

            if (isFirstClick)
            {
                firstClickPosition = curPosition;
                isFirstClick = false;
            }
            else
            {
                // �� ��° Ŭ�� ��ġ
                Vector3 secondClickPosition = curPosition;

                // ���� �߽ɰ� ������ ���
                Vector3 center = (firstClickPosition + secondClickPosition) / 2;
                float radius = Vector3.Distance(firstClickPosition, secondClickPosition) / 2;

                
                // ���ο� ���� �׸� ������ �ν��Ͻ��� ����
                GameObject newCircle = Instantiate(circlePrefab);
                MeshRenderer renderer = newCircle.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = newCircle.GetComponent<MeshFilter>();
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.sortingOrder = existingCircles.Count;

                if (IsOverlapping(center, radius))
                {
                    renderer.material.color = Camera.main.backgroundColor;
                }
                else
                {
                    renderer.material.color = Color.red;
                    existingCircles.Add(new CircleData(center, radius));
                }

                DrawCircle(meshFilter, center, radius);
                isFirstClick = true;
            }
        }

        // ���콺 �� ����
        if (Input.mouseScrollDelta.y != 0)
        {
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPosition.z = 0;

            Vector3 currentScale = transform.localScale;

            // �� ���
            float zoomAmount = 1 + Input.mouseScrollDelta.y * zoomSpeed;
            Vector3 newScale = currentScale * zoomAmount;

            // ũ�� ����
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            // ���ο� ũ�⸦ ����
            transform.localScale = newScale;
        }
    }
    public void DrawCircleOnButtonClick()
    {
        isActive = true;
    }

    public void DrawCircle(MeshFilter meshFilter, Vector3 center, float radius)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 1];
        int[] triangles = new int[segments * 3];

        // �߽��� ����
        vertices[0] = center;

        // �� �ѷ��� �� ���
        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            vertices[i + 1] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            if (i < segments - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        // ������ �ﰢ��
        triangles[(segments - 1) * 3] = 0;
        triangles[(segments - 1) * 3 + 1] = segments;
        triangles[(segments - 1) * 3 + 2] = 1;

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }

    private bool IsOverlapping(Vector3 center, float radius)
    {
        foreach (CircleData circle in existingCircles)
        {
            Vector3 existingCenter = circle.center;
            float existingRadius = circle.radius;

            // �� ���� ��ġ���� Ȯ��
            float distance = Vector3.Distance(center, existingCenter);
            if (distance < radius + existingRadius)
            {
                return true; // ��ġ�� ���
            }
        }
        return false; // ��ġ�� �ʴ� ���
    }
}