using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingReclicsDataInPlayer : MonoBehaviour
{
    Unit player;
    
    private void Awake() {
        player = GetComponent<Unit>();
        //\\TODO BonusGoods , SummonUnitHp 연결필요
        player.damage += player.unit.damage * ((GameManager.Instance.reclicsDatas[0].inItPercent + GameManager.Instance.reclicsDatas[0].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[0]) / 100f);
        player.hp += player.unit.hp * ((GameManager.Instance.reclicsDatas[1].inItPercent + GameManager.Instance.reclicsDatas[1].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[1]) / 100f);
        player.setInitAttackSpeed -= player.unit.attackSpeed * ((GameManager.Instance.reclicsDatas[3].inItPercent + GameManager.Instance.reclicsDatas[3].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[3]) / 100f);
        player.speed += player.unit.speed * (( GameManager.Instance.reclicsDatas[4].inItPercent + GameManager.Instance.reclicsDatas[4].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[4]) / 100f);


    }
}
