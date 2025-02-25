using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GiftBox : MonoBehaviour , IPointerClickHandler{
    public enum GiftType { Gem , PickUpTicket };
    [SerializeField] int soulGiftCount;
    [SerializeField] GiftType giftType;
    [SerializeField] int gemGiftCount;
    
    [SerializeField] GiftShow giftShowObject;
    [SerializeField] Sprite[] giftSprites;
    [SerializeField] int clearCount;
    [SerializeField] Sprite boxOpenSprite;
    [SerializeField] Outline outline;
    bool isOpen;
    public void Setting(bool isOpen){
        outline = GetComponent<Outline>();
        this.isOpen = isOpen;
        if(this.isOpen) {
            GetComponent<Image>().sprite = boxOpenSprite;
        }
    }
    void Update()
    {
        if(DailyQuestTab.dailyQuestTab.clearQuestCount >= clearCount) {
            outline.enabled = true;
        }
        else {
            outline.enabled = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isOpen && clearCount <= DailyQuestTab.dailyQuestTab.clearQuestCount) {
            GameData gameData = GameDataManger.Instance.GetGameData();
            gameData.soul  += soulGiftCount;
            gameData.gem += gemGiftCount;
            gameData.isBoxOpen[transform.GetSiblingIndex()] = true;
            GameDataManger.Instance.SaveData();

            isOpen = true;
            GetComponent<Image>().sprite = boxOpenSprite;
            return;
        }
        giftShowObject.transform.position = transform.position + Vector3.up * 50f;
        giftShowObject.Setting(soulGiftCount , gemGiftCount , giftSprites[(int) giftType]);
    }
}