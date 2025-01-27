using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : SummonerSkillParent
{

    [SerializeField] GameObject skillPrefeb;
    LightningAttack lightning;
    Summoner summoner;
    Animator ani;


    private void Awake()
    {
        ani = GetComponent<Animator>();
        summoner = GetComponent<Summoner>();
        lightning = PoolingManager.Instance.ShowObject(skillPrefeb.name +"(Clone)" , skillPrefeb).GetComponent<LightningAttack>();   
        lightning.summoner = summoner; 
        PoolingManager.Instance.ReturnObject(lightning.name , lightning.gameObject);
    }
    private void Update() {
        if(SkillManager.Instance.LightningAttack &&  skillData == null) {
            skillData = SkillManager.Instance.GetSkillData("번개 공격");
            SetCoolTime();
        }  
        Skill();
    }
    public void Skill()
    {
        if (SkillManager.Instance.LightningAttack && summoner.target != null && !summoner.isDie && summoner.target.name != "NextStage")
        {
            if(currentSkillCoolTime <= 0){
                PoolingManager.Instance.ShowObject(skillPrefeb.name +"(Clone)");        
        
                summoner.target.GetComponent<IDamageAble>().Hit(summoner.damage * skillData.initPercent , AttackType.SkillAttack);
                SetCoolTime();
            }
            
            currentSkillCoolTime -= Time.deltaTime;
        }
    }
    

}
