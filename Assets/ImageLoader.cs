using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public RawImage displayImage;  // 이미지를 표시할 UI
    public string imagePath;  // 이미지 경로
    public Button loadImageButton;  // 이미지를 불러올 버튼

    void Start()
    {
        // 버튼 클릭 이벤트에 LoadImage 함수를 연결
        loadImageButton.onClick.AddListener(() => LoadImage(imagePath));
    }

    // 이미지를 불러오는 함수
    void LoadImage(string path)
    {
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture != null)
        {
            displayImage.texture = texture;
            displayImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);  // 이미지 크기를 텍스처 크기에 맞춤
        }
        else
        {
            Debug.LogError("이미지를 불러올 수 없습니다: " + path);
        }
    }
}
