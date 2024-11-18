using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReclicsTab : MonoBehaviour
{

    [SerializeField] ExplainTab explainReclicsTab;

    //정적으로 선언하니까 씬 전환이후에 계속 메모리에 남아있어서 
    //다시 메인씬에 돌아와서 파괴된 ReclicsTab에 setInfo에 접근해서 오류생김
    public Action<ReclicsInfo> setInfo;

    private void Start() {
        setInfo += SetInfo;
    }
    public void SetInfo(ReclicsInfo info){
        if(info == null) return;
        explainReclicsTab.SetReclicsData(info); // ExplainReclicsTab으로 데이터 전송해주는 코드임
        explainReclicsTab.gameObject.SetActive(true);
    }
      
}
