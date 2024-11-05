using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingRewards : MonoBehaviour
{
    [SerializeField] SelectReward[] selectReward;
    
    
    private void OnEnable() {
        
        for(int i = 0; i < selectReward.Length; i++) {
            selectReward[i].SetRewardData(RewardManager.Instance.GetRewardData());
        }
        StartCoroutine(WaitForAnimationCorutine());
    }

    private void OnDisable() {
        GameManager.Instance.ResumeGame();
    }

    IEnumerator WaitForAnimationCorutine(){
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.StopGame();
    }
}
