using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSlime : ShortRangeScipt , ISummonUnit 
{
    
    public Summoner summoner { get ; set ; }
    private BossShowAnimation bossShow;
    private void OnEnable() {
        base.OnEnable();
        if(summoner == null)  Spawn(GameManager.Instance.currentStage);
        else SummonerSpawn(summoner);
    }    

    
}
