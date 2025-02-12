using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class SummonUnitViewer : MonoBehaviour , IPointerClickHandler
{
    public CameraMoveMent cameraMoveMent;
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
    Unit viewerTarget;
    [SerializeField] ShildBar shildBar;
    [SerializeField] Image image;
    [SerializeField] Hpbar hpbar;
    [SerializeField] GameObject skill_Infomation;
    [SerializeField] Transform skillInfomationParent;
    private void Awake() {
        cameraMoveMent = FindAnyObjectByType<CameraMoveMent>();
        
    }
    IEnumerator WaitForSettingSkill(Unit unit)
    {

        for(int i = 0; i < skillInfomationParent.childCount; i++) {
            Destroy(skillInfomationParent.GetChild(i));
        }

        yield return new WaitUntil(() => unit.SkillSetting);
        viewerTarget = unit;
        for (int i = 0; i < unit.unit.soulsSkillData.Length; i++)
        {
            if (unit.unit.soulsSkillData[i].level <= GameDataManger.Instance.GetGameData().soulsLevel[unit.unit.typenumber - 1] + 1)
            {
                string findclass = unit.unit.soulsSkillData[i].skillData.skillName;
                SkillParent skilldata = unit.gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;

                GameObject skill_Infomation = Instantiate(this.skill_Infomation, skillInfomationParent);
                
                skill_Infomation.GetComponent<SkillInfomationSkillCoolTime>().Setting(skilldata , unit.unit.soulsSkillData[i].skillData);
            }
            else break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        cameraMoveMent.SettingCameraTarget(viewerTarget);
    }
}
