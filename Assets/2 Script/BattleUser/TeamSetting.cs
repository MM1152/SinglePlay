using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeamSetting : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Transform teamList;
    SoulsInfo[] soulsInfo;

    void Awake()
    {
        button.onClick.AddListener(() => SettingBattleMob());
        soulsInfo = SoulsManager.Instance.soulsInfos;
    }
    // void Start()
    // {
    //     SettingEquip();
    // }
    // void SettingEquip(){
    //     List<int> battleEquip = GameDataManger.Instance.GetGameData().battleEquip;
    //     for(int i = 0 ; i < teamList.childCount; i++) {
    //         if(battleEquip[i] != 0) {
    //             teamList.GetChild(i).GetComponent<EquipSouls>().SetSoulInfoForBattle(soulsInfo[battleEquip[i] - 1]);
    //         }
    //     }
    // }

    void SettingBattleMob(){
        BattleUserData battleUserData = new BattleUserData();
        battleUserData.userName = GameDataManger.Instance.GetGameData().userName;

        for(int i = 0; i < teamList.childCount; i++) {
            EquipSouls equip = teamList.GetChild(i).GetComponent<EquipSouls>();
            SoulsInfo info = equip.GetSoulInfo();
            if(info != null) battleUserData.mobList.Add(new MobData(info.GetUnitData().name , info.GetUnitData().typenumber - 1 , info.soulLevel));
        }
        GameManager.Instance.connectDB.SettingBattleData(battleUserData);
    }

}
