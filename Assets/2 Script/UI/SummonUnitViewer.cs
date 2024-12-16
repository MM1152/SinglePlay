using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SummonUnitViewer : MonoBehaviour
{

    public Unit unit
    {
        set
        {
            image.sprite = value.unit.image;
            hpbar.target = value;

            StartCoroutine(WaitForSettingSkill(value));
        }
    }

    [SerializeField] Image image;
    [SerializeField] Hpbar hpbar;
    [SerializeField] GameObject skill_Infomation;

    IEnumerator WaitForSettingSkill(Unit unit)
    {
        yield return new WaitUntil(() => unit.SkillSetting);

        for (int i = 0; i < unit.unit.soulsSkillData.Length; i++)
        {
            if (unit.unit.soulsSkillData[i].level <= GameDataManger.Instance.GetGameData().soulsLevel[unit.unit.typenumber - 1])
            {
                string findclass = unit.unit.soulsSkillData[i].skillData.skillName;
                SkillParent skilldata = unit.gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;

                GameObject skill_Infomation = Instantiate(this.skill_Infomation, transform);
                
                skill_Infomation.GetComponent<SkillInfomationSkillCoolTime>().Setting(skilldata , unit.unit.soulsSkillData[i].skillData);
            }
            else break;
        }
    }
}
