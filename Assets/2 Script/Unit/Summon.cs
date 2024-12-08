using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public Unit unit;
    public float repairHpCoolTime; 
    private void Awake() {
        unit = GetComponent<Unit>();
        //\\TODO 현재 캐릭터의 스킬데이터를 받아와 해금된 상태라면 그 스킬을 넣어준다.
    }
    
    private void Update() {
        RepairSkill();
        
    }

    private void RepairSkill(){
        if(SkillManager.Instance.UpgradeAutoRepair && !unit.isDie) {
            if(repairHpCoolTime <= 0) {
                repairHpCoolTime = 5f;
                if(unit.hp + (unit.maxHp * (SkillManager.Instance.skillDatas["자동 회복"] * 0.05f)) <= unit.maxHp) {
                    unit.hp += unit.maxHp * (SkillManager.Instance.skillDatas["자동 회복"] * 0.05f);
                }
                else {
                    unit.hp = unit.maxHp;
                }
            }
            
            repairHpCoolTime -= Time.deltaTime;
        }
    }

}
