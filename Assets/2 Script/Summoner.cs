using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : LongRangeScript
{   
    [SerializeField] GameObject summons;
    [SerializeField] float skillCoolDown;
    [SerializeField] float skillCurrentTime;
    private void Awake() {
        base.Awake();
        skillCoolDown = 2f;
        skillCurrentTime = skillCoolDown;
    }
    private void Update() {
        base.Update();
        SummonSkill();
    }

    public void Move(Vector3 movePos)
    {
        transform.position += movePos * Time.deltaTime * unit.speed;
    }
    
    public void SummonSkill(){
        if(skillCurrentTime <= 0 && SummonUnit.unitCount < 1){
            GameObject unit = Instantiate(summons , gameObject.transform.parent);
            unit.transform.position = transform.position + Vector3.right;
            skillCurrentTime = skillCoolDown;
        }
        else {
            skillCurrentTime -= Time.deltaTime;
        }
    }
}
