using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : LongRangeScript , ISummonUnit
{
    
    public Summoner summoner { get ; set ; }

    private void OnEnable() {
        base.OnEnable();
        if(summoner == null)  Spawn(GameManager.Instance.gameLevel);
        else SummonerSpawn(summoner ,
         GameManager.Instance.soulsInfo["Demon"].curStat.attackStat / 100f ,  GameManager.Instance.soulsInfo["Demon"].curStat.hpStat / 100f);
    }    
    
}
