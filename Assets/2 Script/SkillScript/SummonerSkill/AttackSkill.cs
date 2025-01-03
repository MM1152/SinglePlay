using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    //\\TODO 1.summoner 스킬 묶을 부모 만들고 
    //\\     2.묶은 스킬들의 float skillCoolTime , SkillData 도담아놓을 변수 필요할 듯 (스킬 쿨타임 재설정시 필요)선언
    //\\     3.자식에서 스킬 쿨타임 설정시 부모의 SkillCoolTimeReSet() 함수를 이용해 설정
    //\\     4.부모에서 Reclics 쿨타임 데이터 접근해서 쿨타임 감소 진행할 예정 
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
