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
    [SerializeField] Tier tier;
    [SerializeField] Image trophy;
    BattleUserData otherUserData;

    [SerializeField] Sprite[] trophyImages; 

    public void Init(bool inList , Action callback = null) {
        if(inList) {
                button?.onClick.AddListener(() => {
                    if(GameDataManger.Instance.GetGameData().playAbleCount < 1) return;

                    GameDataManger.Instance.GetGameData().settingBattleUserData = false;
                    GameManager.Instance.otherBattleUserData = otherUserData;
                    GameDataManger.Instance.GetGameData().playAbleCount--;
                    GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);

                    callback?.Invoke();
                    GameManager.Instance.mapName = "BattleMap";
                    LoadingScene.LoadScene("BattleMap");
                });
                button.gameObject.SetActive(true);
        }
        else {
            trophy.gameObject.SetActive(true);
        }
        tier = transform.GetComponentInChildren<Tier>();
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
        tier.Init(mob.battleScore);
    }
    public void SettingRank(int index){
        trophy.sprite = trophyImages[index];
    }
    public void SetUserName(string userName){
        userNameText.text = userName;
    }
}
