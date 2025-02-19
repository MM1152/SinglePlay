using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BigDarker : ShortRangeScipt
{
    public GameObject darkerAttack;

    public DrainLife drainLife;
    private void Start() {
        TryGetComponent<DrainLife>(out drainLife);
    }
    private void OnEnable() {
        base.OnEnable();
        attackPattenChange = true;
    }
    private void Update() {
        base.Update();
        if(canAttack) {
            GameObject attack = PoolingManager.Instance.ShowObject(darkerAttack.name + "(Clone)" , darkerAttack);
            attack.GetComponent<BigDarkerAttack>().target = target.transform;
            target.GetComponent<IDamageAble>().Hit(damage , this , Critical : clitical );
            drainLife?.UseSkill();
        }
    }
    
}
