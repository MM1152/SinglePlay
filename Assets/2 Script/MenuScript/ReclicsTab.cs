using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReclicsTab : MonoBehaviour
{

    [SerializeField] ExplainTab explainReclicsTab;
    public static Action<ReclicsInfo> setInfo;

    private void Start() {
        setInfo += SetInfo;
    }
    public void SetInfo(ReclicsInfo info){
        explainReclicsTab.SetReclicsData(info); // ExplainReclicsTab으로 데이터 전송해주는 코드임
        explainReclicsTab.gameObject.SetActive(true);
    }
      
}
