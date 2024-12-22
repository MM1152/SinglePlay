using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetGoodsCount : MonoBehaviour
{
    [SerializeField] Text soulCountText;
    Text gemCountText;
    private void Start() {
        StartCoroutine(WaitforDownLoadData());
    }

    void Setting(int soul , int gem){
        //\\TODO유물이나 소울 레벨업당 재화설정 필요할거같음
        soulCountText.text = soul + "";
    }
    IEnumerator WaitforDownLoadData(){
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);
        GameData data = GameDataManger.Instance.GetGameData();
        Setting(data.soul , data.gem);
    }
}
