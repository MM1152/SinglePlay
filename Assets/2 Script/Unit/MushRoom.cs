using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MushRoom : ShortRangeScipt, ISummonUnit
{
    public Summoner summoner { get; set; }

    private void OnEnable() {
        base.OnEnable();
        if(summoner == null)  Spawn(GameManager.Instance.gameLevel);
        else SummonerSpawn(summoner , GameManager.Instance.soulsInfo["MushRoom"].curStat.attackStat / 100f 
        ,  GameManager.Instance.soulsInfo["MushRoom"].curStat.hpStat / 100f);
    }
}
