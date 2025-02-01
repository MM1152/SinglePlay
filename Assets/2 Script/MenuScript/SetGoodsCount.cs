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
        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() => {
            Setting(GameDataManger.Instance.GetGameData().soul , GameDataManger.Instance.GetGameData().gem);
            GameDataManger.Instance.goodsSetting = Setting;
        }));
    }

    void Setting(int soul , int gem){
        soulCountText.text = soul + "";
        gemCountText.text = gem + "";
    }
}
