using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGoodsCount : MonoBehaviour
{
    [SerializeField] Text soulCountText;
    [SerializeField] Text gemCountText;
    
    private void Start() {
        Setting(GameDataManger.Instance.GetGameData().soul , GameDataManger.Instance.GetGameData().gem);
    }

    void Setting(int soul , int gem){
        soulCountText.text = soul + "";
        gemCountText.text = gem + "";
    }
}
