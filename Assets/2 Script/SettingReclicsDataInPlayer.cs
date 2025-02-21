using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingReclicsDataInPlayer : MonoBehaviour
{
    Summoner player;
    private void Awake() {
        player = GetComponent<Summoner>();
        player.damage += player.unit.damage * (ReturnPercent(0) / 100f);
        player.maxHp += player.unit.hp * (ReturnPercent(1) / 100f);
        player.setInitAttackSpeed -= player.unit.attackSpeed * (ReturnPercent(3) / 100f);
        player.speed += player.unit.speed * (ReturnPercent(4) / 100f);
        player.dodge += ReturnPercent(12) / 100f;
        player.drainLife += ReturnPercent(13) / 100f;
        player.critical += ReturnPercent(2) / 100f;
        player.hp = player.maxHp;

        
    }

    public void Increaes(){
        player.damage += ReturnPercent(7);
        player.maxHp += ReturnPercent(8);
    }

    private float ReturnPercent(int index){
        if(GameDataManger.Instance.GetGameData().reclicsCount[index] > 0 || GameDataManger.Instance.GetGameData().reclicsLevel[index] > 0) {
            return GameManager.Instance.reclicsDatas[index].inItPercent + (GameManager.Instance.reclicsDatas[index].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[index]);
        }
        return 0f;
    }
}
