using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Boss , ISummonUnit
{
    public Summoner summoner { get; set ; }

    private void SummonUnitSetting(){
        ani.SetBool("PlaySpawnAni" , false);
        SummonerSpawn(summoner);
        gameObject.transform.localScale -= Vector3.one * 0.2f ;
    }
    
    private void Start(){
        if(summoner == null) BossSetting();
        else SummonUnitSetting();
    }

    protected override void Update()
    {

        if (!GameManager.Instance.playingAnimation)
        {
            base.Update();
        }

    }
}
