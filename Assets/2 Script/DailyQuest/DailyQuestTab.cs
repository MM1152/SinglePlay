using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    Login, ClearMonster, UpgradeReclic, UpgradeSoul, BuyShop,
    PlayGame, PlayAds, ClearBoss, PickUp
}

public class DailyQuestTab : MonoBehaviour
{
    [SerializeField] Image sliderImage;
    [SerializeField] GameObject parent;
    [SerializeField] Transform boxGrounp;
    
    public static DailyQuestTab dailyQuestTab { get ; private set; }
    bool[] clear;
    public int clearQuestCount {get; private set;}
    void Awake()
    {
        if(dailyQuestTab == null) {
            dailyQuestTab = this;
            clear = new bool[dailyQuestTab.transform.childCount];
            sliderImage.fillAmount = 0f;
        }


        parent.SetActive(false);
    }
    void OnEnable()
    {
        sliderImage.fillAmount = (float) clearQuestCount / 7f;
    }
    public void Setting()
    {
        List<DailyQuestData> gamedata = GameDataManger.Instance.GetGameData().questData;
        // 현재 퀘스트들이 깨졌는지 안깨졌는지 확인하는 부분
        for(int i = 0; i < gamedata.Count; i++) {
            QuestType index = (QuestType) Enum.Parse(typeof(QuestType) , gamedata[i].type);
            dailyQuestTab.transform.GetChild((int)index)
                                            .GetComponent<DailyQuest>()
                                            .Init(gamedata[i].type , gamedata[i].isClear , gamedata[i].count);
            DailyQuestTab.ClearDailyQuest(index , gamedata[i].count);
        }
        
        List<bool> isBoxOpen = GameDataManger.Instance.GetGameData().isBoxOpen;
        //현재 박스들의 오픈 상태
        for(int i = 0; i < 3; i++) {
            boxGrounp.GetChild(i).GetComponent<GiftBox>().Setting(isBoxOpen[i]);
        }
    }
    public static void ClearDailyQuest(QuestType type, int value = 0)
    {
        bool isClear = dailyQuestTab.transform.GetChild((int)type)
                        .GetComponent<DailyQuest>()
                        .Setting(value);
        if (isClear) dailyQuestTab.CheckClear(type);
    }
    public void CheckClear(QuestType type)
    {
        if (clear[(int)type]) return;

        clear[(int)type] = true;
        clearQuestCount++;
        if(clearQuestCount >= 7) clearQuestCount = 7;
        Debug.Log("Clear Quest : "  +  clearQuestCount);
        sliderImage.fillAmount = (float)    clearQuestCount / 7f;
    }
}
