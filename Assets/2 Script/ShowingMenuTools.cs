using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingMenuTools : MonoBehaviour
{
    [SerializeField] GameObject dailyGiftTab;

    public void HideOption(bool hide){
        dailyGiftTab.SetActive(!hide);
    }
}
