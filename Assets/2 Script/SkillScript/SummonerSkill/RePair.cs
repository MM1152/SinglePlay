using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePair : SummonerSkillParent
{
    public GameObject healEffect;
    
    void Start()
    {
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
        if (SkillManager.Instance.UpgradeAutoRepair && !summoner.isDie)
        {
            if (currentSkillCoolTime <= 0)
            {
                SetCoolTime();
                
                if (summoner.hp + (summoner.maxHp * (SkillManager.Instance.skillDatas[skillData] * skillData.initPercent)) <= summoner.maxHp)
                {
                    summoner.hp += summoner.maxHp * (SkillManager.Instance.skillDatas[skillData] * skillData.initPercent);
                }
                else
                {
                    summoner.hp = summoner.maxHp;
                }
                healEffect.SetActive(true);
            }

            currentSkillCoolTime -= Time.deltaTime;
        }
    }
}
