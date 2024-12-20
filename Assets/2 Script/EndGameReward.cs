using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameReward : MonoBehaviour
{
    [SerializeField] GameObject rewardPrefeb;
    [SerializeField] Transform RewardView;
    private void OnEnable() {
        GameObject soulReward = Instantiate(rewardPrefeb , RewardView);
        soulReward.GetComponent<SettingReward>();
    }
}
