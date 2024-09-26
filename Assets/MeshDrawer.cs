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

    public int segments = 100; // 원을 그릴 때 사용할 세그먼트 수
    public GameObject circlePrefab; // 원을 그릴 프리팹

    private Vector3 firstClickPosition; // 첫 번째 클릭 위치
    private bool isFirstClick = true; // 첫 번째 클릭 여부
    private bool isActive = false; // 원 그리기 활성화 여부
    private List<CircleData> existingCircles = new List<CircleData>(); // 기존 원 목록
    private float zoomSpeed = 0.1f; // 줌 속도
    private float minScale = 0.1f;  // 최소 크기
    private float maxScale = 10f;   // 최대 크기
    private int 

    void Start()
    {
        // 시작 시 원을 그릴 프리팹을 설정
        // 프리팹은 원을 그리기 위한 기본 템플릿입니다
        
    }

    void Update()
    {
        if (!isActive)
            return;

        // 마우스 왼쪽 클릭
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
                // 두 번째 클릭 위치
                Vector3 secondClickPosition = curPosition;

                // 원의 중심과 반지름 계산
                Vector3 center = (firstClickPosition + secondClickPosition) / 2;
                float radius = Vector3.Distance(firstClickPosition, secondClickPosition) / 2;

                
                // 새로운 원을 그릴 프리팹 인스턴스를 생성
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

        // 마우스 휠 무브
        if (Input.mouseScrollDelta.y != 0)
        {
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPosition.z = 0;

            Vector3 currentScale = transform.localScale;

            // 줌 계산
            float zoomAmount = 1 + Input.mouseScrollDelta.y * zoomSpeed;
            Vector3 newScale = currentScale * zoomAmount;

            // 크기 제한
            newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
            newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
            newScale.z = Mathf.Clamp(newScale.z, minScale, maxScale);

            // 새로운 크기를 적용
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

        // 중심점 설정
        vertices[0] = center;

        // 원 둘레의 점 계산
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

        // 마지막 삼각형
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

            // 두 원이 겹치는지 확인
            float distance = Vector3.Distance(center, existingCenter);
            if (distance < radius + existingRadius)
            {
                return true; // 겹치는 경우
            }
        }
        return false; // 겹치지 않는 경우
    }
}