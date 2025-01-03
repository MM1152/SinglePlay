using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    [SerializeField] GameObject skillPrefeb;
    SkillData skilldata;
    Summoner summoner;
    Animator ani;

    float currentSkillCoolTime;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        summoner = GetComponent<Summoner>();
        
    }
    private void Update() {
        if(SkillManager.Instance.LightningAttack &&  skilldata == null) {
            skilldata = SkillManager.Instance.GetSkillData("번개 공격");
            currentSkillCoolTime = skilldata.coolTime;
        }  
        Skill();
    }
    public void Skill()
    {
        if (SkillManager.Instance.LightningAttack && summoner.target != null && !summoner.isDie && summoner.target.name != "NextStage")
        {
            if(currentSkillCoolTime <= 0){
                LightningAttack lightning = PoolingManager.Instance.ShowObject(skillPrefeb.name +"(Clone)" , skillPrefeb).GetComponent<LightningAttack>();   
                lightning.summoner = summoner; 

                summoner.target.GetComponent<IDamageAble>().Hit(summoner.damage * skilldata.initPercent , AttackType.SkillAttack);
                currentSkillCoolTime = skilldata.coolTime;
            }
            
            currentSkillCoolTime -= Time.deltaTime;
        }
    }
    

}
