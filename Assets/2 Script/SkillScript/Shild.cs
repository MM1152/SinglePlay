using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour , SkillParent
{
    float skillCoolTime;
    Unit unit;

    public SoulsSkillData soulsSkillData { get ; set ; }

    private void Awake() {
        unit = GetComponent<Unit>();
    }
    private void Start() {
        skillCoolTime = soulsSkillData.skillCoolTime;
    }
    public float GetSkillCoolTime()
    {
        return skillCoolTime;
    }

    public void UseSkill()
    {   
        skillCoolTime -= Time.deltaTime;

        if(skillCoolTime <= 0) {
            unit.shild += unit.maxHp * (soulsSkillData.skillInitPercent / 100f);
            skillCoolTime = soulsSkillData.skillCoolTime;
        }
    }
}
