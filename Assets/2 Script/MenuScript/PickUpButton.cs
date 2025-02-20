using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpButton : MonoBehaviour
{
    [SerializeField] private int pickUp_Count;
    [SerializeField] private int pickUp_Cost;
    Button button;
    void Awake()
    {   
        button = GetComponent<Button>();
    }   
    public void Init(ReclicsPickUp coroutine){
        button.onClick.AddListener(() => PickUp(pickUp_Count , coroutine));
    }
    public void Init(SoulPickUp coroutine){
        button.onClick.AddListener(() => PickUp(pickUp_Count , coroutine));
    }
    public void PickUp(int count , ReclicsPickUp coroutine) {
        if(pickUp_Cost <= GameDataManger.Instance.GetGameData().gem) {
            GameDataManger.Instance.GetGameData().gem -= pickUp_Cost;
            DailyQuestTab.ClearDailyQuest(QuestType.PickUp , count);
            GameDataManger.Instance.SaveData();
            
            coroutine.StartCoroutine(coroutine.ShowingReclics(count));
        }
        else return;
    }

    public void PickUp(int count , SoulPickUp coroutine) {
        if(pickUp_Cost <= GameDataManger.Instance.GetGameData().gem) {
            GameDataManger.Instance.GetGameData().gem -= pickUp_Cost;
            DailyQuestTab.ClearDailyQuest(QuestType.PickUp , count);
            GameDataManger.Instance.SaveData();

            coroutine.StartCoroutine(coroutine.ShowingSoul(count));
        }
        else return;
    }
}
