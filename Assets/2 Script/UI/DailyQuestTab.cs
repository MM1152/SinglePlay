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
    public static DailyQuestTab dailyQuestTab { get ; private set; }
    bool[] clear;
    int clearQuestCount;
    void Awake()
    {
        dailyQuestTab = this;
        clear = new bool[dailyQuestTab.transform.childCount];
        sliderImage.fillAmount = 0f;

        parent.SetActive(false);
    }
    void OnEnable()
    {
        sliderImage.fillAmount = (float) clearQuestCount / 7f;
    }
    public void Setting()
    {
        List<DailyQuestData> gamedata = GameDataManger.Instance.GetGameData().questData;

        for(int i = 0; i < gamedata.Count; i++) {
            QuestType index = (QuestType) Enum.Parse(typeof(QuestType) , gamedata[i].type);
            dailyQuestTab.transform.GetChild((int)index)
                                            .GetComponent<DailyQuest>()
                                            .Init(gamedata[i].type , gamedata[i].isClear , gamedata[i].count);
            DailyQuestTab.ClearDailyQuest(index , gamedata[i].count);
        }
    }
    public static void ClearDailyQuest(QuestType type, int value = 0)
    {
        bool isClear = dailyQuestTab.transform.GetChild((int)type).GetComponent<DailyQuest>().Setting(value);
        if (isClear) dailyQuestTab.CheckClear(type);
    }
    public void CheckClear(QuestType type)
    {
        if (clear[(int)type]) return;

        clear[(int)type] = true;
        clearQuestCount++;
        if(clearQuestCount >= 7) clearQuestCount = 7;
        Debug.Log("Clear Quest : "  +  clearQuestCount);
        sliderImage.fillAmount = clearQuestCount / clear.Length;
    }
}
