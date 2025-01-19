using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkeleton : ShortRangeScipt , ISummonUnit
{
    public Summoner summoner { get ; set ; }
    private void OnEnable() {
        base.OnEnable();
        if(summoner == null)  Spawn(GameManager.Instance.currentStage);
        else SummonerSpawn(summoner);
    }    
}
