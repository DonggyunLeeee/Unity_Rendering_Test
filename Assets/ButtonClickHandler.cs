using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public Button drawCircleButton; // ��ư
    public MeshDrawer circleDrawer; // LineRenderer�� ����� �� �׸��� ��ũ��Ʈ

    void Start()
    {
        // ��ư Ŭ�� �� �� �׸��� �Լ� ȣ��
        drawCircleButton.onClick.AddListener(() => circleDrawer.DrawCircleOnButtonClick());
    }
}
