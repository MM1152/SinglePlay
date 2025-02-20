using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhaseStriker : Boss
{
    // 폼 체인지 시 체력 유지
    // 먼저 유닛은 스탯 설정이 완료된뒤 소환, 폼 체인지 시도후 해당하는 유닛의 체력 공격력만 받아온 뒤 나머지 스탯은 UnitData에서 받아와야함.
    // 스킬로 빼서 할까..? 폼 체인지는 자체 기능으로 만들고싶은데 , 그럼 스킬은 다르게 하자.
    [SerializeField] UnitData changeFormUnit;
    [SerializeField] protected GameObject archor;
    [SerializeField] protected DaggerPhaseStriker daggerPhaseStriker;
    [SerializeField] protected ArchorPhaseStriker archorPhaseStriker;

    static float c_hp;
    private void OnEnable() {}

    protected void Start() {
        base.OnEnable();
        base.Start();
    }
    protected void Update() {
        
        c_hp = hp;

        base.Update();

        if(isDie && unit.name == "DaggerPhaseStriker" && archorPhaseStriker.gameObject != null) {
            Destroy(archorPhaseStriker.gameObject);
        } 
        else if(isDie && unit.name == "ArchorPhaseStriker" &&  daggerPhaseStriker.gameObject != null){
            daggerPhaseStriker.summonUnit.DieSummonUnit(); 
            Destroy(daggerPhaseStriker.gameObject);
        }
        /*
        formChangeCoolTime -= Time.deltaTime;

        if(formChangeCoolTime < 0) {
            isSkill = true;
            formChangeCoolTime = 60f;
            ChangeForm();
        }
        */
    }

    public Unit ChangeForm(){
        PhaseStriker phaseStriker = default;
        
        if(GetType().ToString() == "DaggerPhaseStriker") {
            phaseStriker = archorPhaseStriker;
        }
        else {
            phaseStriker = daggerPhaseStriker;
        }

        StartCoroutine(WaitHideAnimation(phaseStriker));
        return phaseStriker;
    }
    
    public void ChangeFormSetting(){
        hp = c_hp;
        isAttack = false;
        
        gameObject.SetActive(true);
        ani.SetBool("FormChange" , true);
        StartCoroutine(WaitShowAnimation());
    }

    IEnumerator WaitHideAnimation(PhaseStriker changeUnit){
        isSkill = true;
        ani.SetBool("Skill2" , true);
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Hide") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        changeUnit.ChangeFormSetting();
        changeUnit.transform.position = gameObject.transform.position;
        canFollow = false;
        isSkill = false;
        gameObject.SetActive(false);
    }

    IEnumerator WaitShowAnimation(){
        yield return new WaitUntil(() =>  ani.GetCurrentAnimatorStateInfo(0).IsName("Show") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);
        if(GetType().ToString() == "DaggerPhaseStriker" && Vector2.Distance(target.transform.position , gameObject.transform.position) <= unit.attackRadious) {
            target.GetComponent<IDamageAble>().Hit(damage , this , clitical , AttackType.SkillAttack);
        }
        canFollow = true;
        ani.SetBool("FormChange" , false);
    }
}
