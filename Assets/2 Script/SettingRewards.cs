using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingRewards : MonoBehaviour
{
    [SerializeField] SelectReward[] selectReward;
    [SerializeField] ReRollReward[] reRollReward;
    [SerializeField] Image timerImage;
    [SerializeField] SettingTabButton settingTabButton;
    public int maxCount;
    Coroutine running;
    bool isAuto;
    private void Awake() {
        maxCount = 1;
    }

    private void OnEnable() {
        SetReward();
        reRollReward[0].Setting(maxCount , SetReward);
        reRollReward[1].Setting(maxCount , SetReward);
    }

    public void SetReward(){
        GameManager.Instance.ResumeGame();
        StartCoroutine(WaitForAnimationCorutine());
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
        
        if(running != null) StopCoroutine(running);

        yield return new WaitForSeconds(0.3f);
        GameManager.Instance.StopGame();
        running = StartCoroutine(AutoSelect());
    }
    IEnumerator AutoSelect(){
        float timer = 5f;

        
        while(timer >= 0f) {
            if(settingTabButton.isSelect && !GoogleAdMobs.instance.isPlayAd) {
                timer -= Time.unscaledDeltaTime;
            }
            timerImage.fillAmount = timer / 5f;
            yield return null;
        } 

        selectReward[0].SettingRewardAdditional();
    } 
}
