using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeForm : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get; set; }
    [SerializeField] float currentCoolTime;
    PhaseStriker unit;
    Summon summon;
    Unit chnageUnit;
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }

    public void UseSkill()
    {
        if(unit.isDie) return;

        if(currentCoolTime <= 0) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            chnageUnit = unit.ChangeForm();
            summon.ChangeFormUnit(this);
        }
        else currentCoolTime -= Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<PhaseStriker>();
        currentCoolTime = soulsSkillData.skillCoolTime;
        summon = GetComponent<Summon>();
    }

    public Unit GetChangeUnitData(){
        return chnageUnit;
    }

    public Unit GetUnit(){
        return unit;
    }
}
