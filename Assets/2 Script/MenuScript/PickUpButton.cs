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
        //\\TODO 현재 재화에 맞춰 뽑기 실행하도록 해야함
        if(pickUp_Cost <= GameDataManger.Instance.GetGameData().gem) {
            GameDataManger.Instance.GetGameData().gem -= pickUp_Cost;
            GameDataManger.Instance.SaveData();

            coroutine.StartCoroutine(coroutine.ShowingReclics(count));
        }
        else return;
    }

    public void PickUp(int count , SoulPickUp coroutine) {
        //\\TODO 현재 재화에 맞춰 뽑기 실행하도록 해야함
        if(pickUp_Cost <= GameDataManger.Instance.GetGameData().gem) {
            GameDataManger.Instance.GetGameData().gem -= pickUp_Cost;
            GameDataManger.Instance.SaveData();

            coroutine.StartCoroutine(coroutine.ShowingSoul(count));
        }
        else return;
    }
}
