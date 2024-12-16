using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public Unit unit;
    public float repairHpCoolTime; 
    public GameObject healEffect;
    private void Awake() {
        unit = GetComponent<Unit>();
        healEffect = Instantiate(Resources.Load<GameObject>("UseSkillFolder/HealEffect") , transform);
        healEffect.SetActive(false);
        
        //유닛 레벨에 따른 스킬 적용
        for(int i = 0; i < unit.unit.soulsSkillData.Length; i++){
            if(unit.unit.soulsSkillData[i].level <= GameDataManger.Instance.GetGameData().soulsLevel[unit.unit.typenumber - 1]) {
                string findclass = unit.unit.soulsSkillData[i].skillData.skillName;
                gameObject.AddComponent(Type.GetType(findclass)); // 스킬 이름에 따라 스킬 컴포넌트를 붙여줘서 사용하는 방식
                SkillParent skilldata = gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;
                skilldata.soulsSkillData = unit.unit.soulsSkillData[i].skillData;
            }
            else break;
        }
        unit.SkillSetting = true;
    }
    
    private void Update() {
        RepairSkill();
    }

    private void RepairSkill(){
        if(SkillManager.Instance.UpgradeAutoRepair && !unit.isDie) {
            if(repairHpCoolTime <= 0) {
                repairHpCoolTime = 10f;
                if(unit.hp + (unit.maxHp * (SkillManager.Instance.skillDatas["자동 회복"] * 0.05f)) <= unit.maxHp) {
                    unit.hp += unit.maxHp * (SkillManager.Instance.skillDatas["자동 회복"] * 0.05f);
                }
                else {
                    unit.hp = unit.maxHp;
                }
                healEffect.SetActive(true);
            }
            
            repairHpCoolTime -= Time.deltaTime;
        }
    }


}
