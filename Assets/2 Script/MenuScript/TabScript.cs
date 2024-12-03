
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabScript : MonoBehaviour
{
    [SerializeField] GameObject childObject;
    [SerializeField] Sprite initImage;
    [SerializeField] Sprite selectedImage;
    Button button;
    Image image;

    Color selectColor = new Color(0.5f, 0.5f, 0.5f, 1f); // 버튼 선택시 칼라
    Color noneSelectColor = new Color(1f, 1f, 1f, 1f);
    private static GameObject currentGameObject;
    bool isSelect
    {
        set
        {
            if (value)
            {
                //image.color = selectColor;
                image.sprite = selectedImage;
                button.interactable = false;
            }
            else
            {
                //image.color = noneSelectColor;
                image.sprite = initImage;
                button.interactable = true;
            }
        }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }
    private void Update()
    {
        if (currentGameObject == this.gameObject)
        {
            childObject.SetActive(true);
            isSelect = true;
        }
        else
        {
            childObject.SetActive(false);
            isSelect = false;
        }
    }

    void OnClickButton()
    {
        currentGameObject = this.gameObject;
    }
}
