using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingReward : MonoBehaviour
{
    [SerializeField] Image rewardImage;
    [SerializeField] Text countText;

    public void Setting(Sprite image , int count){
        rewardImage.sprite = image;
        countText.text = count + "";
    }
}
