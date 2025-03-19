using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class BattleList : MonoBehaviour
{
    [SerializeField] BattleMob prefeb;
    [SerializeField] Transform prefebSpawnPos;
    [SerializeField] Button button;
    [SerializeField] Text userNameText;
    [SerializeField] Text score;
    
    BattleUserData otherUserData;
    void Awake()
    {
        button.onClick.AddListener(() => {
            GameManager.Instance.otherBattleUserData = otherUserData;
            GameManager.Instance.mapName = "BattleMap";
            LoadingScene.LoadScene("BattleMap");
        });
    }
    /// <param name="index">BattleUserData에 index값으로 접근하기 위함</param>
    /// <param name="mob">Use MobData</param>
    public void Setting(BattleUserData mob)
    {
        otherUserData = mob;
        
        for(int i = 0 ; i < mob.mobList.Count; i++) {
            BattleMob battlemob = Instantiate(prefeb , prefebSpawnPos).GetComponent<BattleMob>();
            Sprite mobImage = SoulsManager.Instance.soulsInfos[mob.mobList[i].typeNumber].image;
            battlemob.Setting(mobImage , mob.mobList[i].level);
        }
        
        userNameText.text = mob.userName;
        score.text = mob.battleScore + "";
        
    }
    public void SetUserName(string userName){
        userNameText.text = userName;
    }
}
