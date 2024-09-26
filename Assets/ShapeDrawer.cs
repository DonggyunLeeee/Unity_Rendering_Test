using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeDrawer : MonoBehaviour
{
    public Button squareButton;  // �簢�� ��ư
    public Button circleButton;  // �� ��ư
    public Button lineButton;    // �� ��ư
    public Button polygonButton; // ������ ��ư
    public RectTransform drawArea;  // ������ �׸� �г�

    private string curShapeType = "";   // ���� �׸��� ���� ���� ����
    private string prevShapeType = "";   // ���� �׸��� ���� ���� ����
    private List<Vector2> points = new List<Vector2>();  // �� ����Ʈ
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        squareButton.onClick.AddListener(() => { curShapeType = "Square"; });
        circleButton.onClick.AddListener(() => { curShapeType = "Circle"; });
        lineButton.onClick.AddListener(() => { curShapeType = "Line"; });
        polygonButton.onClick.AddListener(() => { curShapeType = "Polygon"; points.Clear(); });
    }

    void Update()
    {
        // ���콺 Ŭ�� ó��
        if (Input.GetMouseButtonDown(0))
        {
            if (curShapeType == "Square")
            {
                DrawSquare(Input.mousePosition);
                prevShapeType = curShapeType;
            }
            else if (curShapeType == "Circle")
            {
                DrawCircle(Input.mousePosition);
                prevShapeType = curShapeType;
            }
            else if (curShapeType == "Line")
            {
                DrawLine(Input.mousePosition);
                prevShapeType = curShapeType;
            }
            else if (curShapeType == "Polygon")
            {
                AddPolygonPoint(Input.mousePosition);
                prevShapeType = curShapeType;
            }
        }

        if (curShapeType == "Polygon" && Input.GetMouseButtonDown(1)) // ��Ŭ�� ����
        {
            if (points.Count >= 2)  // �������� 2���� �� �̻��� �� �׸���
            {
                DrawPolygon();
            }
        }
    }

    void DrawSquare(Vector2 position)
    {
        if (prevShapeType != curShapeType)
        {
            points.Clear();
        }

        if (points.Count == 0)
        {
            points.Add(position);
        }
        else
        {
            float radius = Vector2.Distance(points[0], position) / 2;
            Vector2 center = (points[0] + position) / 2;

            GameObject square = new GameObject("Square");
            square.transform.SetParent(drawArea.transform);
            RectTransform rt = square.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(radius * 2, radius * 2);
            rt.anchoredPosition = WorldToUIPosition(center);

            Image img = square.AddComponent<Image>();
            img.color = Color.red;
            
            points.Clear();
        }
    }

    void DrawCircle(Vector2 position)
    {
        if (prevShapeType != curShapeType)
        {
            points.Clear();
        }

        if (points.Count == 0)
        {
            points.Add(position); // ù ��° ��ǥ
        }
        else
        {
            // �� ��° ��ǥ
            float radius = Vector2.Distance(points[0], position) / 2;
            Vector3 center = (points[0] + position) / 2;
            int segments = 300;

            points.Clear();

            lineRenderer.positionCount = segments + 1;
            lineRenderer.widthMultiplier = 1f;
            lineRenderer.loop = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���� ����
            lineRenderer.material.color = Color.blue;
            lineRenderer.useWorldSpace = false; // ���� ��ǥ�踦 ���

            // ���� ���� ���
            Vector3[] circlePoints = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                Vector3 tmp = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0); // �߽��� �������� ��ǥ ���
                circlePoints[i] = WorldToUIPosition(tmp + center);
            }

            lineRenderer.SetPositions(circlePoints);
        }
    }

    void DrawLine(Vector2 position)
    {
        if(prevShapeType != curShapeType)
        {
            points.Clear();
        }

        if (points.Count == 0)
        {
            points.Add(position);
        }
        else
        {
            Vector2 start = WorldToUIPosition(points[0]);
            Vector2 end = WorldToUIPosition(position);

            GameObject line = new GameObject("Line");
            line.transform.SetParent(drawArea.transform);
            RectTransform rt = line.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // �� �β� 2
            rt.anchoredPosition = (start + end) / 2;

            // ���� ������ �����մϴ�.
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image img = line.AddComponent<Image>();
            img.color = Color.green;

            points.Clear();
        }
    }

    void AddPolygonPoint(Vector2 position)
    {
        if(prevShapeType != curShapeType)
        {
            points.Clear();
        }

        if (points.Count == 0)
        {
            points.Add(WorldToUIPosition(position));
        }
        else
        {
            Vector2 start = points[points.Count - 1];
            Vector2 end = WorldToUIPosition(position);
            points.Add(end);

            GameObject line = new GameObject("Line");
            line.transform.SetParent(drawArea.transform);
            RectTransform rt = line.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // �� �β� 2
            rt.anchoredPosition = (start + end) / 2;

            // ���� ������ �����մϴ�.
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image img = line.AddComponent<Image>();
            img.color = Color.cyan;
        }
    }

    void DrawPolygon()
    {
        // �����ϰ� 4���� ���� �̾� �������� �׸��ϴ�.
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[(i + 1) % points.Count];

            GameObject line = new GameObject("PolygonLine");
            line.transform.SetParent(drawArea.transform);
            RectTransform rt = line.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // �� �β�
            rt.anchoredPosition = (start + end) / 2;

            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image img = line.AddComponent<Image>();
            img.color = Color.yellow;
        }

        points.Clear();
    }

    // ���� ��ǥ�� UI ��ǥ�� ��ȯ�ϴ� �Լ�
    Vector2 WorldToUIPosition(Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawArea, screenPosition, null, out localPoint);
        return localPoint;
    }
}
