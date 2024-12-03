using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SoulsManager : MonoBehaviour
{
    public static SoulsManager Instance {private set; get;}
    
    [SerializeField] Transform FindequipSoulChild;
    public SoulsInfo[] soulsInfos;
    public Dictionary<SoulsInfo , EquipSouls> equipDic = new Dictionary<SoulsInfo , EquipSouls>();
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
        StartCoroutine(SetSoulInfoCoroutine());
        StartCoroutine(SetEuqipSoulsCoroutine());
    }

    IEnumerator SetSoulInfoCoroutine(){
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);
        GameData data = GameDataManger.Instance.GetGameData();
        for(int i = 0; i < data.soulsCount.Count; i++){
            soulsInfos[i].Setting(data.soulsCount[i] , data.soulsLevel[i]);
            GameManager.Instance.soulsInfo.Add(soulsInfos[i].GetSoulsInfo());
        }
    }

    IEnumerator SetEuqipSoulsCoroutine(){
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);

        GameData data = GameDataManger.Instance.GetGameData();
        for(int i = 0 ; i < data.soulsEquip.Count; i++) {
            if(data.soulsEquip[i] != 0) {
                EquipSouls equip = FindequipSoulChild.transform.GetChild(i).GetComponent<EquipSouls>();

                equip.SetSoulInfo(soulsInfos[data.soulsEquip[i] - 1]);
                equipDic.Add(soulsInfos[data.soulsEquip[i] - 1] , equip);
            }
        }
    }
}
