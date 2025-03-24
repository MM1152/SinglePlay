using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class UI_RankTable : MonoBehaviour
{
    public List<BattleUserData> sort ;
    Transform spawntrans;
    public void Init() {
        sort = new List<BattleUserData>();
        spawntrans = transform.GetChild(1).GetChild(0);
        for(int i = 0; i < GameDataManger.Instance.GetBattleData().user.Count; i++) {
            sort.Add(GameDataManger.Instance.GetBattleData().user[i]);
            sort[i].spawnProbabillity = GameDataManger.Instance.GetBattleData().user[i].battleScore;
        }

        MergeSort<BattleUserData> merge = new MergeSort<BattleUserData>(sort.ToArray());
        sort = merge.get().ToList();

        for(int i = 0 ; i < 3; i++){
            if(sort.Count > i) {
                GameObject go = BaseUI.instance.SynchLoadUi("BattleList");
                go = Instantiate(go, spawntrans);
                BattleList list = go.GetOrAddComponent<BattleList>();
                list.Init(false);
                list.Setting(sort[i]);
                list.SettingRank(i);
            }
        }
    }
}
