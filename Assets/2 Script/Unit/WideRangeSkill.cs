using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideRangeSkill : MonoBehaviour
{
    public Unit unit;
    public SoulsSkillData skillData;
    Animator ani;

    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(WaitForAnimationCorutine());
    }

    IEnumerator WaitForAnimationCorutine(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag(unit.gameObject.tag) && !other.GetComponent<Unit>().isDie) {
            
            //fix :: skillDamage랑 연동시켜야함
            other.GetComponent<IDamageAble>().Hit(unit.damage * (skillData.attackPercent / 100f) , AttackType.SkillAttack);
        }
    }
    
}
