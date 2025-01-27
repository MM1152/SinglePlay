 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get ; set ; }
    public float currentCoolTime;
    Unit unit;
    Animator ani;
    private void Start() {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
    }
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }

    public void UseSkill()
    {
        if(unit.isDie) return;
        
        if(currentCoolTime <= 0) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            StartCoroutine(WaitAnimationCorutaine());
        }else {
            currentCoolTime -= Time.deltaTime;
        }
    }
    IEnumerator WaitAnimationCorutaine(){
        ani.SetBool("Skill", true);
        yield return new WaitForSeconds(0.1f);
        
        transform.localScale = Vector3.one;
        transform.position = unit.target.transform.position;
        ani.SetBool("Skill2" , true);
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Dash") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        ani.SetBool("Skill" , false);
        ani.SetBool("Skill2" , false);

        unit.target.GetComponent<IDamageAble>().Hit(unit.damage * soulsSkillData.attackPercent , AttackType.SkillAttack);
    }
}
