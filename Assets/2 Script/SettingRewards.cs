using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingRewards : MonoBehaviour
{
    [SerializeField] SelectReward[] selectReward;
    [SerializeField] ReRollReward[] reRollReward;
    public int maxCount;

    private void Awake() {
        maxCount = 1;
    }

    private void OnEnable() {
        SetReward();
        reRollReward[0].Setting(maxCount , SetReward);
        reRollReward[1].Setting(maxCount , SetReward);
        StartCoroutine(WaitForAnimationCorutine());
    }

    public void SetReward(){
        
        for(int i = 0; i < selectReward.Length; i++) {
            selectReward[i].gameObject.SetActive(false);
            selectReward[i].SetRewardData(RewardManager.Instance.GetRewardData());
            selectReward[i].gameObject.SetActive(true);
        }
    }

    private void OnDisable() {
        GameManager.Instance.ResumeGame();
    }

    IEnumerator WaitForAnimationCorutine(){
        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.StopGame();
    }
}
