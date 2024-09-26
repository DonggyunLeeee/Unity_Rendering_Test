using UnityEngine;
using UnityEngine.UI;

public class ImageZoomAndDrag : MonoBehaviour
{
    public RawImage displayImage;  // 이미지를 표시할 UI
    public RectTransform canvasRect;  // 캔버스 RectTransform
    private float zoomSpeed = 0.1f;  // 확대/축소 속도
    private bool isDragging = false;  // 드래그 상태 여부
    private Vector2 lastMousePosition;  // 마지막 마우스 위치

    void Update()
    {
        // 줌 인/아웃
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomImage();
        }

        // 드래그 시작
        if (Input.GetMouseButtonDown(1)) // 우클릭 시작
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        // 드래그 중
        if (isDragging)
        {
            DragImage();
        }

        // 드래그 종료
        if (Input.GetMouseButtonUp(1)) // 우클릭 종료
        {
            isDragging = false;
        }
    }

    void ZoomImage()
    {
        // 마우스 위치 가져오기
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localMousePosition);

        // 확대/축소 비율 계산
        float zoomFactor = 1 + Input.mouseScrollDelta.y * zoomSpeed;

        // 기존 이미지 크기
        Vector2 oldSize = displayImage.rectTransform.sizeDelta;

        // 이미지의 크기 조정
        displayImage.rectTransform.sizeDelta *= zoomFactor;

        // 새 이미지 크기
        Vector2 newSize = displayImage.rectTransform.sizeDelta;

        // 이미지의 위치를 마우스 포인트 기준으로 조정
        Vector2 pivotDelta = (localMousePosition - displayImage.rectTransform.anchoredPosition);
        Vector2 positionAdjustment = pivotDelta * (newSize - oldSize) / oldSize;

        displayImage.rectTransform.anchoredPosition -= positionAdjustment;
    }

    void DragImage()
    {
        // 현재 마우스 위치와 이전 마우스 위치의 차이를 계산하여 이미지 이동
        Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
        displayImage.rectTransform.anchoredPosition += delta / canvasRect.localScale.x;

        // 마지막 마우스 위치를 현재 위치로 업데이트
        lastMousePosition = Input.mousePosition;
    }
}
