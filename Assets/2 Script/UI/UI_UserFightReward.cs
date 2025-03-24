using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_UserFightReward : MonoBehaviour
{
    Tier tier;
    bool init;
    public void Init(int score = 0){
        if(init) {
            gameObject.SetActive(true);
            return;
        }
        
        init = true;
        UI_Event getReward = transform.Find("GetReward").GetOrAddComponent<UI_Event>();
        getReward.SetClickAction(GetReward);

        tier = transform.Find("Tier").GetOrAddComponent<Tier>();
        tier.Init(score);

        Text reward = transform.Find("Gem").GetComponentInChildren<Text>();
        reward.text = tier.GetReward() + "";
    }
    private void GetReward(){
        GameDataManger.Instance.GetGameData().gem += tier.GetReward();
        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
        
        GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore = 0;
        GameDataManger.Instance.SaveData(GameDataManger.SaveType.BattleData);
        GameManager.Instance.connectDB.WriteBattleScore();
    }
}
