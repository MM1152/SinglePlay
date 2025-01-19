using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour , SkillParent
{
    //DashAttack 구현
    public SoulsSkillData soulsSkillData { get; set ; }
    Unit unit;
    public float GetSkillCoolTime()
    {
        return -1;
    }

    public void UseSkill()
    {
        return;
    }
    private void Start() {
        unit = GetComponent<Unit>();
        unit.dodge = soulsSkillData.skillInitPercent / 100f;
    }
}
