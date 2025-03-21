using System.Collections.Generic;
using UnityEngine;

public class SummonerSkillParent : MonoBehaviour
{
    //\\     1.summoner 스킬 묶을 부모 만들고 
    //\\     2.묶은 스킬들의 float skillCoolTime , SkillData 도담아놓을 변수 필요할 듯 (스킬 쿨타임 재설정시 필요)선언
    //\\     3.자식에서 스킬 쿨타임 설정시 부모의 SetCoolTime() 함수를 이용해 설정
    //\\     4.부모에서 Reclics 쿨타임 데이터 접근해서 쿨타임 감소 진행할 예정 
    protected SkillData skillData;
    protected Summoner summoner;
    float repairSkillCoolTime;
    float extraSkillDamage;
    [SerializeField] protected float currentSkillCoolTime;
    protected void Awake() {
        summoner = GetComponent<Summoner>();
        repairSkillCoolTime = ReturnPercent(9) / 100f;
        extraSkillDamage = ReturnPercent(10) / 100f;
    }
    protected void SetCoolTime() {
        currentSkillCoolTime = skillData.coolTime;
        currentSkillCoolTime -= currentSkillCoolTime * repairSkillCoolTime;
    }
    protected void SetCoolTime(SkillData otherSkillData) {
        currentSkillCoolTime = otherSkillData.coolTime;
        currentSkillCoolTime -= currentSkillCoolTime * repairSkillCoolTime;
    }
    protected float SetDamage(float damage){ 
        damage = damage + (damage * extraSkillDamage);
        return damage;
    }
    private float ReturnPercent(int index){
        return GameManager.Instance.reclicsDatas[index].inItPercent + (GameManager.Instance.reclicsDatas[index].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[index]);
    }
    public void SkillAttack(GameObject hitunit , float damage){
        if(summoner.hp + (damage * summoner.drainLife) <= summoner.maxHp) {
            summoner.hp += damage * summoner.drainLife;
        }
        hitunit.GetComponent<IDamageAble>().Hit(damage , summoner ,summoner.critical , AttackType.SkillAttack);
        
    }
    
}

