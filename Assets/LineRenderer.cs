using UnityEngine;

public class LineRendererCircleDrawer : MonoBehaviour
{
    public int segments = 100; // 원을 그릴 때 사용할 세그먼트 수
    private LineRenderer lineRenderer;
    private Vector3 firstClickPosition; // 첫 번째 클릭 위치
    private bool isFirstClick = true; // 첫 번째 클릭 여부

    void Start()
    {
        // LineRenderer 컴포넌트 추가
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer 설정
        lineRenderer.positionCount = segments + 1; // 점의 개수 (시작점과 끝점을 동일하게 하기 위해 +1)
        lineRenderer.widthMultiplier = 0.1f; // 선의 굵기
        lineRenderer.loop = true; // 선을 연결하여 원을 닫음
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // 기본 재질 설정
        lineRenderer.material.color = Color.blue; // 선의 색상 설정
        lineRenderer.useWorldSpace = true; // 월드 좌표계를 사용

        // 초기에는 원을 그리지 않도록 비활성화
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // 마우스 클릭 이벤트 처리
        if (Input.GetMouseButtonDown(0)) // 왼쪽 마우스 버튼 클릭
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // 2D 게임에서 Z 축을 0으로 설정

            if (isFirstClick)
            {
                firstClickPosition = clickPosition;
                isFirstClick = false;
            }
            else
            {
                // 두 번째 클릭 위치
                Vector3 secondClickPosition = clickPosition;

                // 원의 중심과 반지름 계산
                Vector3 center = (firstClickPosition + secondClickPosition) / 2;
                float radius = Vector3.Distance(firstClickPosition, secondClickPosition) / 2;

                // 원을 그리기
                DrawCircle(center, radius);

                isFirstClick = true; // 첫 클릭으로 돌아가도록 설정
            }
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    public void DrawCircleOnButtonClick()
    {
        // 클릭된 위치를 기준으로 원을 그립니다.
        // (현재 마우스 위치를 사용하여 원을 그리도록 구현되어 있습니다)
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // 2D 게임에서 Z 축을 0으로 설정

        if (isFirstClick)
        {
            firstClickPosition = clickPosition;
            isFirstClick = false;
        }
        else
        {
            Vector3 secondClickPosition = clickPosition;

            Vector3 center = (firstClickPosition + secondClickPosition) / 2;
            float radius = Vector3.Distance(firstClickPosition, secondClickPosition) / 2;

            DrawCircle(center, radius);

            isFirstClick = true; // 첫 클릭으로 돌아가도록 설정
        }
    }

    // 원을 그리는 함수
    public void DrawCircle(Vector3 center, float radius)
    {
        Vector3[] circlePoints = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments; // 각도 계산
            circlePoints[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0); // 점의 좌표 계산
        }

        lineRenderer.SetPositions(circlePoints);
        lineRenderer.enabled = true; // 원을 그리도록 활성화
    }
}
