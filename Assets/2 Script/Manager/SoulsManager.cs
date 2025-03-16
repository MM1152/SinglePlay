using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class SoulsManager : MonoBehaviour
{
    // 이거  처음 실행할때 플레이어 전투 팀 설정 창에 사진안나옴
    public static SoulsManager Instance { private set; get; }

    [SerializeField] Transform FindequipSoulChild; // SoulInfo 쪽 Euqip
    [SerializeField] Transform FindBattleEquipChild; // 대전기능쪽 Equip
    public SoulsInfo[] soulsInfos;
    public Dictionary<SoulsInfo, EquipSouls> equipDic = new Dictionary<SoulsInfo, EquipSouls>();
    public Dictionary<SoulsInfo, EquipSouls> battleEquipDic = new Dictionary<SoulsInfo, EquipSouls>();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        soulsInfos = GameObject.FindObjectsOfType<SoulsInfo>();
        MergeSort<SoulsInfo> mergeSort = new MergeSort<SoulsInfo>(soulsInfos);
        soulsInfos = mergeSort.get();
        Array.Reverse(soulsInfos);

    }

    private void Start()
    {
        StartCoroutine(SetSoulInfoCoroutine());
        StartCoroutine(SetEuqipSoulsCoroutine());
    }

    IEnumerator SetSoulInfoCoroutine()
    {
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);

        GameData data = GameDataManger.Instance.GetGameData();
        for (int i = 0; i < data.soulsCount.Count; i++)
        {
            soulsInfos[i].Setting(data.soulsCount[i], data.soulsLevel[i]);
        }
    }

    IEnumerator SetEuqipSoulsCoroutine()
    {
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);

        GameData data = GameDataManger.Instance.GetGameData();
        for (int i = 0; i < data.soulsEquip.Count; i++)
        {
            EquipSouls equip = FindequipSoulChild.transform.GetChild(i).GetComponent<EquipSouls>();
            if (data.soulsEquip[i] != 0)
            {
                equip.SetSoulInfo(soulsInfos[data.soulsEquip[i] - 1]);
                equipDic.Add(soulsInfos[data.soulsEquip[i] - 1], equip);
            }
            else {
                equip.SetSoulInfo(null);
            }
        }

        for (int i = 0; i < data.battleEquip.Count; i++)
        {
            EquipSouls equip = FindBattleEquipChild.transform.GetChild(i).GetComponent<EquipSouls>();
            if (data.battleEquip[i] != 0 && !battleEquipDic.ContainsKey(soulsInfos[data.battleEquip[i] - 1]))
            {
                equip = FindBattleEquipChild.transform.GetChild(i).GetComponent<EquipSouls>();
                equip.SetSoulInfoForBattle(soulsInfos[data.battleEquip[i] - 1]);
                battleEquipDic.Add(soulsInfos[data.battleEquip[i] - 1], equip);
            }
            else if(data.battleEquip[i] != 0 && battleEquipDic.ContainsKey(soulsInfos[data.battleEquip[i] - 1])) equip.SetSoulInfoForBattle(null);
            else equip.SetSoulInfoForBattle(null);
        }
    }
}
