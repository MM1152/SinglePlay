using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BattleList : MonoBehaviour
{
    [SerializeField] BattleMob prefeb;
    [SerializeField] Transform prefebSpawnPos;
    [SerializeField] Button button;

    int index;
    void Awake()
    {
        button.onClick.AddListener(() => {
            GameManager.Instance.otherBattleUserData = GameDataManger.Instance.GetBattleData().battleUserDatas[index];
            GameManager.Instance.mapName = "BattleMap";
            LoadingScene.LoadScene("BattleMap");
        });
    }
    /// <param name="index">BattleUserData에 index값으로 접근하기 위함</param>
    /// <param name="mob">Use MobData</param>
    public void Setting(int index , MobData mob)
    {
        BattleMob battlemob = Instantiate(prefeb , prefebSpawnPos).GetComponent<BattleMob>();
        Sprite mobImage = SoulsManager.Instance.soulsInfos[mob.typeNumber].image;
        this.index = index;
        battlemob.Setting(mobImage , mob.level);
    }
}
