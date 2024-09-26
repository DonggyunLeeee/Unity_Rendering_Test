using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeDrawer : MonoBehaviour
{
    public Button squareButton;  // 사각형 버튼
    public Button circleButton;  // 원 버튼
    public Button lineButton;    // 선 버튼
    public Button polygonButton; // 폴리곤 버튼
    public RectTransform drawArea;  // 도형을 그릴 패널

    private string curShapeType = "";   // 현재 그리기 중인 도형 유형
    private string prevShapeType = "";   // 현재 그리기 중인 도형 유형
    private List<Vector2> points = new List<Vector2>();  // 점 리스트
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
        // 마우스 클릭 처리
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

        if (curShapeType == "Polygon" && Input.GetMouseButtonDown(1)) // 우클릭 시작
        {
            if (points.Count >= 2)  // 폴리곤이 2개의 점 이상일 때 그리기
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
            points.Add(position); // 첫 번째 좌표
        }
        else
        {
            // 두 번째 좌표
            float radius = Vector2.Distance(points[0], position) / 2;
            Vector3 center = (points[0] + position) / 2;
            int segments = 300;

            points.Clear();

            lineRenderer.positionCount = segments + 1;
            lineRenderer.widthMultiplier = 1f;
            lineRenderer.loop = true;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 재질 설정
            lineRenderer.material.color = Color.blue;
            lineRenderer.useWorldSpace = false; // 월드 좌표계를 사용

            // 원의 점들 계산
            Vector3[] circlePoints = new Vector3[segments + 1];
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                Vector3 tmp = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0); // 중심을 기준으로 좌표 계산
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

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // 선 두께 2
            rt.anchoredPosition = (start + end) / 2;

            // 선의 각도를 설정합니다.
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

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // 선 두께 2
            rt.anchoredPosition = (start + end) / 2;

            // 선의 각도를 설정합니다.
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image img = line.AddComponent<Image>();
            img.color = Color.cyan;
        }
    }

    void DrawPolygon()
    {
        // 간단하게 4개의 점을 이어 폴리곤을 그립니다.
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 start = points[i];
            Vector2 end = points[(i + 1) % points.Count];

            GameObject line = new GameObject("PolygonLine");
            line.transform.SetParent(drawArea.transform);
            RectTransform rt = line.AddComponent<RectTransform>();

            rt.sizeDelta = new Vector2(Vector2.Distance(start, end), 2);  // 선 두께
            rt.anchoredPosition = (start + end) / 2;

            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image img = line.AddComponent<Image>();
            img.color = Color.yellow;
        }

        points.Clear();
    }

    // 월드 좌표를 UI 좌표로 변환하는 함수
    Vector2 WorldToUIPosition(Vector2 screenPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(drawArea, screenPosition, null, out localPoint);
        return localPoint;
    }
}
