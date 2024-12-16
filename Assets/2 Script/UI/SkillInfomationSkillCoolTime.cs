using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfomationSkillCoolTime : MonoBehaviour
{
    [SerializeField] Image coolTime;
    [SerializeField] Image skill_Image;
    SkillParent skillCoolTime;
    SoulsSkillData skillData;
    Image image;
    private void Awake() {
        image = GetComponent<Image>();
        coolTime.type = Image.Type.Filled;
        coolTime.fillAmount = 0;
    }
    public void Setting(SkillParent skillParent , SoulsSkillData skillData){
        skillCoolTime = skillParent;
        this.skillData = skillData;

        skill_Image.sprite = skillData.skillImages;
    }
    private void Update() {
        if(skillCoolTime.GetSkillCoolTime() != -1) {
            coolTime.fillAmount = skillCoolTime.GetSkillCoolTime() / skillCoolTime.soulsSkillData.skillCoolTime;
        }    
    }
}
