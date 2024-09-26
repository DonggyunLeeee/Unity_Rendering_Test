using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    public RawImage displayImage;  // �̹����� ǥ���� UI
    public string imagePath;  // �̹��� ���
    public Button loadImageButton;  // �̹����� �ҷ��� ��ư

    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� LoadImage �Լ��� ����
        loadImageButton.onClick.AddListener(() => LoadImage(imagePath));
    }

    // �̹����� �ҷ����� �Լ�
    void LoadImage(string path)
    {
        Texture2D texture = Resources.Load<Texture2D>(path);
        if (texture != null)
        {
            displayImage.texture = texture;
            displayImage.rectTransform.sizeDelta = new Vector2(texture.width, texture.height);  // �̹��� ũ�⸦ �ؽ�ó ũ�⿡ ����
        }
        else
        {
            Debug.LogError("�̹����� �ҷ��� �� �����ϴ�: " + path);
        }
    }
}
