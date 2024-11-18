using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingReclicsDataInPlayer : MonoBehaviour
{
    Unit player;
    List<int> reclicsLevel = new List<int>();
    
    private void Awake() {
        player = GetComponent<Unit>();

        reclicsLevel = GameDataManger.Instance.GetGameData().reclicsLevel;
        //\\TODO scriptable 오브젝트로 생성한 유물 데이터에 접근해서 데이터 적용시켜주기
    }
}
