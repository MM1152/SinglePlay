using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePair : MonoBehaviour
{
    public GameObject healEffect;
    public float repairHpCoolTime; 
    public SkillData repairSkillData;
    
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
            repairSkillData = SkillManager.Instance.GetSkillData("자동 회복");
        }
        RepairSkill();
    }

    private void RepairSkill()
    {
        if (SkillManager.Instance.UpgradeAutoRepair && !unit.isDie)
        {
            if (repairHpCoolTime <= 0)
            {
                repairHpCoolTime = 10f;
                
                
                if (unit.hp + (unit.maxHp * (SkillManager.Instance.skillDatas[repairSkillData] * repairSkillData.initPercent)) <= unit.maxHp)
                {
                    unit.hp += unit.maxHp * (SkillManager.Instance.skillDatas[repairSkillData] * repairSkillData.initPercent);
                }
                else
                {
                    unit.hp = unit.maxHp;
                }
                healEffect.SetActive(true);
            }

            repairHpCoolTime -= Time.deltaTime;
        }
    }
}
