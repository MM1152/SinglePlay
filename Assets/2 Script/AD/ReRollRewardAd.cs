using System;
using UnityEngine;
using UnityEngine.UI;

public class ReRollRewardAd : MonoBehaviour{
    Button button;
    ReRollReward reRollReward;
    public Action rewardAction {
        set {
            button.onClick.AddListener(() =>{
            Debug.Log(value.ToString());
            if(GoogleAdMobs.instance.ShowRewardedAd(value)) {
                reRollReward.count--;
                GameManager.Instance.StopGame();
            }});
        }
    }
    private void Awake() {
        button = GetComponent<Button>();
        reRollReward = GetComponent<ReRollReward>();
    }
}