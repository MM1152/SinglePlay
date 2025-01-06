using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SummonUnitViewer : MonoBehaviour
{
    public string unitName;
    public Unit unit
    {
        set
        {
            unitName = value.name;
            image.sprite = value.unit.image;
            hpbar.target = value;
            shildBar.target = value;
            
            StartCoroutine(WaitForSettingSkill(value));
        }
    }
    [SerializeField] ShildBar shildBar;
    [SerializeField] Image image;
    [SerializeField] Hpbar hpbar;
    [SerializeField] GameObject skill_Infomation;
    [SerializeField] Transform skillInfomationParent;
    IEnumerator WaitForSettingSkill(Unit unit)
    {
        yield return new WaitUntil(() => unit.SkillSetting);

        for (int i = 0; i < unit.unit.soulsSkillData.Length; i++)
        {
            if (unit.unit.soulsSkillData[i].level <= GameDataManger.Instance.GetGameData().soulsLevel[unit.unit.typenumber - 1])
            {
                string findclass = unit.unit.soulsSkillData[i].skillData.skillName;
                SkillParent skilldata = unit.gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;

                GameObject skill_Infomation = Instantiate(this.skill_Infomation, skillInfomationParent);
                
                skill_Infomation.GetComponent<SkillInfomationSkillCoolTime>().Setting(skilldata , unit.unit.soulsSkillData[i].skillData);
            }
            else break;
        }
    }
}
