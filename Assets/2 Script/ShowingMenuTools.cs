using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingMenuTools : MonoBehaviour
{
    [SerializeField] GameObject dailyGiftTab;
    [SerializeField] GameObject settingTab;
    [SerializeField] GameObject dailyQuest;
    public void HideOption(bool hide){
        dailyGiftTab.SetActive(!hide);
        settingTab.SetActive(!hide);
        dailyQuest.SetActive(!hide);
    }
}
