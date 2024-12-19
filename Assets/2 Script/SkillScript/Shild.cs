using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : SkillParent
{
    float skillCoolTime;
    Unit unit;
    private void Awake() {
        unit = GetComponent<Unit>();
    }
    private void Start() {
        skillCoolTime = soulsSkillData.skillCoolTime;
    }
    public override float GetSkillCoolTime()
    {
        return skillCoolTime;
    }

    public override void UseSkill()
    {   
        skillCoolTime -= Time.deltaTime;

        if(skillCoolTime <= 0) {
            unit.shild += unit.maxHp * (soulsSkillData.skillInitPercent / 100f);
            skillCoolTime = soulsSkillData.skillCoolTime;
        }
    }
    private void Update() {
        UseSkill();
    }
}
