using UnityEngine;

public class LineRendererCircleDrawer : MonoBehaviour
{
    public int segments = 100; // ���� �׸� �� ����� ���׸�Ʈ ��
    private LineRenderer lineRenderer;
    private Vector3 firstClickPosition; // ù ��° Ŭ�� ��ġ
    private bool isFirstClick = true; // ù ��° Ŭ�� ����

    void Start()
    {
        // LineRenderer ������Ʈ �߰�
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // LineRenderer ����
        lineRenderer.positionCount = segments + 1; // ���� ���� (�������� ������ �����ϰ� �ϱ� ���� +1)
        lineRenderer.widthMultiplier = 0.1f; // ���� ����
        lineRenderer.loop = true; // ���� �����Ͽ� ���� ����
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // �⺻ ���� ����
        lineRenderer.material.color = Color.blue; // ���� ���� ����
        lineRenderer.useWorldSpace = true; // ���� ��ǥ�踦 ���

        // �ʱ⿡�� ���� �׸��� �ʵ��� ��Ȱ��ȭ
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // ���콺 Ŭ�� �̺�Ʈ ó��
        if (Input.GetMouseButtonDown(0)) // ���� ���콺 ��ư Ŭ��
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPosition.z = 0; // 2D ���ӿ��� Z ���� 0���� ����

            if (isFirstClick)
            {
                firstClickPosition = clickPosition;
                isFirstClick = false;
            }
            else
            {
                // �� ��° Ŭ�� ��ġ
                Vector3 secondClickPosition = clickPosition;

                // ���� �߽ɰ� ������ ���
                Vector3 center = (firstClickPosition + secondClickPosition) / 2;
                float radius = Vector3.Distance(firstClickPosition, secondClickPosition) / 2;

                // ���� �׸���
                DrawCircle(center, radius);

                isFirstClick = true; // ù Ŭ������ ���ư����� ����
            }
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void DrawCircleOnButtonClick()
    {
        // Ŭ���� ��ġ�� �������� ���� �׸��ϴ�.
        // (���� ���콺 ��ġ�� ����Ͽ� ���� �׸����� �����Ǿ� �ֽ��ϴ�)
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0; // 2D ���ӿ��� Z ���� 0���� ����

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

            isFirstClick = true; // ù Ŭ������ ���ư����� ����
        }
    }

    // ���� �׸��� �Լ�
    public void DrawCircle(Vector3 center, float radius)
    {
        Vector3[] circlePoints = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments; // ���� ���
            circlePoints[i] = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0); // ���� ��ǥ ���
        }

        lineRenderer.SetPositions(circlePoints);
        lineRenderer.enabled = true; // ���� �׸����� Ȱ��ȭ
    }
}
