using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;

public class DailyGift : MonoBehaviour
{
    [SerializeField] GameObject RewardPrefeb;
    [SerializeField] Transform gridLayOut;
    [SerializeField] Button onGiveItemBNT;
    private void Start(){
        StartCoroutine(GameDataManger.WaitForDownLoadData(() => Setting()));
        onGiveItemBNT.onClick.AddListener(() => {
            GiveGift();
        });
    }
    private void Setting() {
        GameData gameData = GameDataManger.Instance.GetGameData();
        for(int i = 0; i < 28; i++) {
            RewardData reward = Instantiate(RewardPrefeb, gridLayOut)
                                .GetComponent<RewardData>();
            reward.Setting(i + 1, (i + 1) % 7 == 0 ? 30 : 10 , gameData.dailyGift[i]);
        }

        if(gameData.getGift) onGiveItemBNT.interactable = false;
        else onGiveItemBNT.interactable = true;
    }

    private void GiveGift(){
        GameData gameData = GameDataManger.Instance.GetGameData() ;
        for(int i = 0; i < gameData.dailyGift.Count; i++) {
            if(!gameData.dailyGift[i]) {
                gridLayOut.GetChild(i)
                .GetComponent<RewardData>()
                .GetReward();
                break;
            }
        }
        onGiveItemBNT.interactable = false;
    }   
}
