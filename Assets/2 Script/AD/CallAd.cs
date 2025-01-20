using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CallAd : MonoBehaviour
{
    Button button;
    public Action rewardFunction;
    //\\TODO : Count 수 데이터 저장 필요
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => GoogleAdMobs.instance.ShowRewardedAd(rewardFunction));
    }

    
    
}
