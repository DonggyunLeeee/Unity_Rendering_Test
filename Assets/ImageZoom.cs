using UnityEngine;
using UnityEngine.UI;

public class ImageZoomAndDrag : MonoBehaviour
{
    public RawImage displayImage;  // �̹����� ǥ���� UI
    public RectTransform canvasRect;  // ĵ���� RectTransform
    private float zoomSpeed = 0.1f;  // Ȯ��/��� �ӵ�
    private bool isDragging = false;  // �巡�� ���� ����
    private Vector2 lastMousePosition;  // ������ ���콺 ��ġ

    void Update()
    {
        // �� ��/�ƿ�
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomImage();
        }

        // �巡�� ����
        if (Input.GetMouseButtonDown(1)) // ��Ŭ�� ����
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }

        // �巡�� ��
        if (isDragging)
        {
            DragImage();
        }

        // �巡�� ����
        if (Input.GetMouseButtonUp(1)) // ��Ŭ�� ����
        {
            isDragging = false;
        }
    }

    void ZoomImage()
    {
        // ���콺 ��ġ ��������
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localMousePosition);

        // Ȯ��/��� ���� ���
        float zoomFactor = 1 + Input.mouseScrollDelta.y * zoomSpeed;

        // ���� �̹��� ũ��
        Vector2 oldSize = displayImage.rectTransform.sizeDelta;

        // �̹����� ũ�� ����
        displayImage.rectTransform.sizeDelta *= zoomFactor;

        // �� �̹��� ũ��
        Vector2 newSize = displayImage.rectTransform.sizeDelta;

        // �̹����� ��ġ�� ���콺 ����Ʈ �������� ����
        Vector2 pivotDelta = (localMousePosition - displayImage.rectTransform.anchoredPosition);
        Vector2 positionAdjustment = pivotDelta * (newSize - oldSize) / oldSize;

        displayImage.rectTransform.anchoredPosition -= positionAdjustment;
    }

    void DragImage()
    {
        // ���� ���콺 ��ġ�� ���� ���콺 ��ġ�� ���̸� ����Ͽ� �̹��� �̵�
        Vector2 delta = (Vector2)Input.mousePosition - lastMousePosition;
        displayImage.rectTransform.anchoredPosition += delta / canvasRect.localScale.x;

        // ������ ���콺 ��ġ�� ���� ��ġ�� ������Ʈ
        lastMousePosition = Input.mousePosition;
    }
}
