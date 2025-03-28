using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBoom : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get ; set ; }
    Unit unit;
    Animator ani;
    [SerializeField] float currentCoolTime;
    private void Start() {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
        currentCoolTime = soulsSkillData.skillCoolTime;
    }
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }
    public void UseSkill()
    {
        if(unit.isDie) return;
        
        if(currentCoolTime <= 0 && Vector2.Distance(transform.position , unit.target.transform.position) <= unit.unit.attackRadious) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            ani.SetBool("Skill" , true);
            StartCoroutine(WaitAnimationCorutaine());
        }else {
            currentCoolTime -= Time.deltaTime;
        }
    }
    IEnumerator WaitAnimationCorutaine(){
        
        unit.isSkill = true;
        InWarningArea warning = PoolingManager.Instance.ShowObject("CircleWarningArea(Clone)").GetComponent<InWarningArea>();
        warning.Setting(soulsSkillData.attackPercent / 100f , 1f , unit);
        warning.SetPosition(unit.target.transform.position);
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        ani.SetBool("Skill" , false);
        unit.isSkill = false;
    }
}
