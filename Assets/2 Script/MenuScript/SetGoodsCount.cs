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
    static bool first;
    static int soulCount; 
    //\\TODO : 게임 처음 시작할때 소울획득 애니메이션 재생되는거 막아야됌
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
        if(soul > soulCount && first) {
            foreach(GetSoulAnimation ani in getSoulAnimations) {
                ani.StartGetSoulAnimation();
            }
        }

        first = true;

        soulCount = soul;
        soulCountText.text = soul + "";
        gemCountText.text = gem + "";
    }
}
