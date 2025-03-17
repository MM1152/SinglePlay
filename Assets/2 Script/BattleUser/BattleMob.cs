using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMob : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text level;
    public void Setting(Sprite image , int level){
        this.image.sprite = image;
        this.level.text = "Lv." + (level + 1);
    }
}
