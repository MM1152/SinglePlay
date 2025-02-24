
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabScript : MonoBehaviour
{
    [SerializeField] GameObject childObject;
    [SerializeField] Sprite initImage;
    [SerializeField] Sprite selectedImage;

    RectTransform tabImage;
    Vector2 tabImageInitPos;
    RectTransform rect;
    Button button;
    Image image;

    Color selectColor = new Color(0.5f, 0.5f, 0.5f, 1f); // 버튼 선택시 칼라
    Color noneSelectColor = new Color(1f, 1f, 1f, 1f);
    private static TabScript currentGameObject;
    bool isSelect
    {
        set
        {
            if (value)
            {
                //image.color = selectColor;
                image.sprite = selectedImage;
                button.interactable = false;
                childObject.SetActive(true);
                StartCoroutine(PlayAnimation());
            }
            else
            {
                //image.color = noneSelectColor;
                rect.sizeDelta = new Vector2(150f , 150f);
                tabImage.sizeDelta = new Vector2(150f , 150f);
                image.sprite = initImage;
                button.interactable = true;
                tabImage.localPosition = tabImageInitPos;
                childObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        tabImage = transform.GetChild(0).GetComponent<RectTransform>();
        tabImageInitPos = tabImage.localPosition;
        image = GetComponent<Image>();
        //StartCoroutine(WaitSort());
    }

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
        isSelect = false;
    }
    
    void OnClickButton()
    {
        if(currentGameObject != null) currentGameObject.isSelect = false;
    
        currentGameObject = this.gameObject.GetComponent<TabScript>();
        isSelect = true;
    }

    IEnumerator PlayAnimation(){
        SoundManager.Instance.Play(SoundManager.SFX.Open);
        for(int i = 0; i <= 25; i += 5) {
            rect.sizeDelta += new Vector2(4 , 4);
            tabImage.sizeDelta += new Vector2(10, 10);
            tabImage.localPosition += new Vector3(0f , 5f , 0);
            yield return new WaitForSeconds(0.005f);
        }
    }

    IEnumerator WaitSort(){
        yield return new WaitUntil(() => GameManager.Instance.sortReclis && GameManager.Instance.sortSoul);
        isSelect = false;
    }
}
