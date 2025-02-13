using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingMenuTools : MonoBehaviour
{
    [SerializeField] GameObject dailyGiftTab;
    [SerializeField] GameObject settingTab;
    public void HideOption(bool hide){
        dailyGiftTab.SetActive(!hide);
        settingTab.SetActive(!hide);
    }
}
