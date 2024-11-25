using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SoulsManager : MonoBehaviour
{
    public static SoulsManager Instance {private set; get;}

    public SoulsInfo[] soulsInfos;
    void Awake()
    {
        if(Instance == null) {
            Instance = this;
        }

        soulsInfos = GameObject.FindObjectsOfType<SoulsInfo>();
        MergeSort<SoulsInfo> mergeSort = new MergeSort<SoulsInfo>(soulsInfos);
        soulsInfos = mergeSort.get();
        Array.Reverse(soulsInfos);

    }

    private void Start() {
        StartCoroutine(SetSoulInfoCorutine());
    }

    IEnumerator SetSoulInfoCorutine(){
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);
        GameData data = GameDataManger.Instance.GetGameData();
        for(int i = 0; i < data.soulsCount.Count; i++){
            soulsInfos[i].Setting(data.soulsCount[i] , data.soulsLevel[i]);
            GameManager.Instance.soulsInfo.Add(soulsInfos[i].GetSoulsInfo());
        }
    }
}
