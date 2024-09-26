using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public Button drawCircleButton; // 버튼
    public MeshDrawer circleDrawer; // LineRenderer를 사용한 원 그리기 스크립트

    void Start()
    {
        // 버튼 클릭 시 원 그리기 함수 호출
        drawCircleButton.onClick.AddListener(() => circleDrawer.DrawCircleOnButtonClick());
    }
}
