using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePair : SummonerSkillParent
{
    public GameObject healEffect;
    
    Unit unit;
    void Start()
    {
        unit = GetComponent<Unit>();
        healEffect = Instantiate(Resources.Load<GameObject>("UseSkillFolder/HealEffect"), transform);
        healEffect.SetActive(false);
    }

    private void Update()
    {
        if(SkillManager.Instance.UpgradeAutoRepair) {
            skillData = SkillManager.Instance.GetSkillData("자동 회복");
        }
        RepairSkill();
    }

    private void RepairSkill()
    {
        if (SkillManager.Instance.UpgradeAutoRepair && !unit.isDie)
        {
            if (currentSkillCoolTime <= 0)
            {
                SetCoolTime();
                
                if (unit.hp + (unit.maxHp * (SkillManager.Instance.skillDatas[skillData] * skillData.initPercent)) <= unit.maxHp)
                {
                    unit.hp += unit.maxHp * (SkillManager.Instance.skillDatas[skillData] * skillData.initPercent);
                }
                else
                {
                    unit.hp = unit.maxHp;
                }
                healEffect.SetActive(true);
            }

            currentSkillCoolTime -= Time.deltaTime;
        }
    }
}
