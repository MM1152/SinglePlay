using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ChangeShopListButtonAd : MonoBehaviour
{
    Button button;
    ChangeShopListButton changeBNT;
    public Action rewardFunction;
    void Start()
    {
        button = GetComponent<Button>();
        changeBNT = GetComponent<ChangeShopListButton>();
        button.onClick.AddListener(() => {
            if(GoogleAdMobs.instance.ShowRewardedAd(rewardFunction)) {
                changeBNT.count--;
                GameDataManger.Instance.GetGameData().shopListChangeCount[1] = changeBNT.count;
                GameDataManger.Instance.SaveData();
            }
        });
    }

    
    
}
