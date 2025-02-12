using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    public Unit unit;
    CreateSummonUnitViewer summonUnitViewer;

    private void Awake() {
        unit = GetComponent<Unit>();
        summonUnitViewer = GameObject.FindObjectOfType<CreateSummonUnitViewer>();

        //유닛 레벨에 따른 스킬 적용
        for(int i = 0; i < unit.unit.soulsSkillData.Length; i++){
            if(unit.unit.soulsSkillData[i].level <= GameDataManger.Instance.GetGameData().soulsLevel[unit.unit.typenumber - 1] + 1) {
                string findclass = unit.unit.soulsSkillData[i].skillData.skillName;
                
                if(Type.GetType(findclass) == null) continue;

                gameObject.AddComponent(Type.GetType(findclass)); // 스킬 이름에 따라 스킬 컴포넌트를 붙여줘서 사용하는 방식
                SkillParent skilldata = gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;
                skilldata.soulsSkillData = unit.unit.soulsSkillData[i].skillData;
                unit.skillData.Add(skilldata);
            }
            else break;
        }

        unit.SkillSetting = true;
    }
    public void CreateSummonViewer(){
        summonUnitViewer.CreateViewer(unit.GetComponent<Unit>());
    }
    public void ChangeFormUnit(ChangeForm changeForm){
        summonUnitViewer.ChnageFormUnit(changeForm);
    }
}
