using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BigDarker : ShortRangeScipt , ISummonUnit
{
    public GameObject darkerAttack;

    public Summoner summoner { get; set; }
    public DrainLife drainLife;
    private void Start() {
        TryGetComponent<DrainLife>(out drainLife);
    }
    private void OnEnable() {
        base.OnEnable();
        if( summoner != null ) SummonerSpawn(summoner);
        attackPattenChange = true;
    }
    private void Update() {
        base.Update();
        if(canAttack) {
            GameObject attack = PoolingManager.Instance.ShowObject(darkerAttack.name + "(Clone)" , darkerAttack);
            attack.GetComponent<BigDarkerAttack>().target = target.transform;
            target.GetComponent<IDamageAble>().Hit(damage);
            drainLife?.UseSkill();
        }
    }
    
}
