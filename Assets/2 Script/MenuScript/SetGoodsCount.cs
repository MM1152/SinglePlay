using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGoodsCount : MonoBehaviour
{
    [SerializeField] Text soulCountText;
    [SerializeField] Text gemCountText;
    [SerializeField] GetSoulAnimation getSoulAnimation;
    [SerializeField] List<GetSoulAnimation> getSoulAnimations;
    private void Start() {
        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() => {
            Setting(GameDataManger.Instance.GetGameData().soul , GameDataManger.Instance.GetGameData().gem);
            GameDataManger.Instance.goodsSetting = Setting;
        }));
        for(int i = 0 ; i < 5; i++) {
            getSoulAnimations.Add(Instantiate(getSoulAnimation , transform));
            getSoulAnimations[i].gameObject.SetActive(false);
        }
    }

    void Setting(int soul , int gem){
        if(soul > int.Parse(soulCountText.text)) {
            foreach(GetSoulAnimation ani in getSoulAnimations) {
                ani.StartGetSoulAnimation();
            }
        }
        soulCountText.text = soul + "";
        gemCountText.text = gem + "";
    }
}
