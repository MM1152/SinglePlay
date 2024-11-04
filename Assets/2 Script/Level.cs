using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    Text levelText;
    int level;
    
    void Awake()
    {
        level = 1;
        levelText = GetComponent<Text>();
        levelText.text = level.ToString();
    }

    public void LevelUp(){
        level++;
        SkillManager.Instance.statPoint++;
        levelText.text = level.ToString();
        RewardManager.Instance.ShowRewardView();
    }
}
