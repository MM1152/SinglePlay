using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resurrection : SummonerSkillParent {
    Summoner summoner;
    Queue<string> dieUnitList = new Queue<string>(); 
    private void Awake(){
        summoner = GetComponent<Summoner>();
    }

    private void Update(){
        if(skillData == null && SkillManager.Instance.UpgradeResurrection) {
            skillData = SkillManager.Instance.GetSkillData("부활");
        }
        UseSkill();
    }
    //1. 이미 죽은 유닛이 있는지 확인 하는 로직
    //2. 이미 죽은 유닛이 존재한다면 , 부활스킬 쿨타임 적용
    //3. 이후에 죽는 유닛이 생기면 부활스킬 쿨타임 적용
    // 부활 스킬 쿨타임적용은 Invoke? Corutaine★?
    public void UseSkill(){
        if(!summoner.isDie && !SkillManager.Instance.UpgradeResurrection) return;
    
        if(dieUnitList.Count > 0) {
            string unit = dieUnitList.Dequeue();
            StartCoroutine(WaitResurrectionCoolTime(unit));
        }
    }
    public void DieUnit(string name){
        name = name.Replace("(Clone)" , "");
        dieUnitList.Enqueue(name);
    }

    IEnumerator WaitResurrectionCoolTime(string unit){
        yield return new WaitForSeconds(skillData.coolTime);
        summoner.SpawnSoul(unit);
    }
}