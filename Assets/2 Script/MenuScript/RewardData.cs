using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardData : MonoBehaviour
{
    [SerializeField] Text dateTimeText;
    [SerializeField] Text rewardText;
    [SerializeField] Image finImage;

    int reward;
    public void Setting(int datetime , int reward , bool gaveGift){
        this.reward = reward;
        dateTimeText.text = datetime + "";
        rewardText.text = reward + "";

        if(gaveGift) finImage.gameObject.SetActive(true);
        else finImage.gameObject.SetActive(false);
    }

    public void GetReward(){
        finImage.gameObject.SetActive(true);
        GameDataManger.Instance.GetGameData().dailyGift[transform.GetSiblingIndex()] = true;
        GameDataManger.Instance.GetGameData().getGift = true;
        GameDataManger.Instance.GetGameData().gem += reward;
        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
    }
}
