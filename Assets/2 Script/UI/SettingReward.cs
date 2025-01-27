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

        countText.gameObject.SetActive(true);
    }
    
    public void Setting(Sprite image , int count , float bonus){
        rewardImage.sprite = image;
        countText.text = count + " ";
        countText.text += $"<color=green>({bonus})</color>";
        countText.gameObject.SetActive(true);
    }
}
