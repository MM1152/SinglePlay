using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DailyQuest : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private int count;
    [SerializeField] private int maxCount;
    [SerializeField] private Text countText;
    [SerializeField] private Text questText;
    [SerializeField] private GameObject clearQuest;

    string questType;
    bool clear;
    int index;
    public void Init(string questType , bool clear , int count)
    {
        this.questType = questType;
        this.clear = clear;
        this.count = count;

        index = (int)Enum.Parse(typeof(QuestType) , questType);
    }
    public bool Setting(int plus){
        countText.text = $"{count}/{maxCount}";
        
        if(clear) {
            clearQuest.SetActive(true);
            return true;
        } 

        count += plus;
        
        if(count >= maxCount) count = maxCount;

        countText.text = $"{count}/{maxCount}";

        GameDataManger.Instance.GetGameData()
        .questData[index]
        .count = count;

        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);

        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(clear) return;

        StartCoroutine(StartAnimation());
        if(count == maxCount) {
            Debug.Log("Clear");
            clear = true;
            DailyQuestTab.ClearDailyQuest((QuestType) Enum.Parse(typeof(QuestType) , questType));

           
            GameDataManger.Instance.GetGameData()
            .questData[index]
            .isClear = clear;

            GameDataManger.Instance.GetGameData()
            .questData[index]
            .count = count;

            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
        }
    }

    IEnumerator StartAnimation(){
        for(float i = 0f ; i < 1f ; i += 0.2f) {
            transform.localScale -= new Vector3(0.01f , 0.01f , 0f);
            yield return new WaitForSeconds(0.005f);
        }

        for(float i = 0f ; i < 1f ; i += 0.2f) {
            transform.localScale += new Vector3(0.01f , 0.01f , 0f);
            yield return new WaitForSeconds(0.005f);
        }
    }
}
