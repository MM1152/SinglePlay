using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePair : SummonerSkillParent
{
    public GameObject healEffect;
    SkillData autoRepairShiled;
    public List<Unit> summonUnit;
    void Start()
    {
        healEffect = Resources.Load<GameObject>("UseSkillFolder/HealEffect");
        
    }

    private void Update()
    {
        if(SkillManager.Instance.UpgradeAutoRepair && skillData == null) {
            skillData = SkillManager.Instance.GetSkillData("자동 회복");
            SetCoolTime();
        }
        if(SkillManager.Instance.UpgradeAutoRepairShiled && autoRepairShiled == null) {
            autoRepairShiled = SkillManager.Instance.GetSkillData("보호막회복");
        }
        RepairSkill();
    }

    private void RepairSkill()
    {
        if (skillData != null)
        {
            if (currentSkillCoolTime <= 0)
            {
                SetCoolTime();
                for(int i = 0; i < summonUnit.Count; i++){

                    if(summonUnit[i] == null) {
                        summonUnit.RemoveAt(i);
                        continue;
                    }

                    float repairValue = summonUnit[i].maxHp * (SkillManager.Instance.skillDatas[skillData] * skillData.initPercent);
                    if (summonUnit[i].hp + repairValue <= summonUnit[i].maxHp)
                    {
                        summonUnit[i].hp += repairValue;
                    }
                    else
                    {
                        if(autoRepairShiled != null) {
                            float repairShild = summonUnit[i].hp + repairValue - summonUnit[i].maxHp;
                            if(repairShild + summonUnit[i].shild <= summonUnit[i].maxHp * 0.2f){
                                summonUnit[i].shild += repairShild;
                            }
                            else {
                                if(summonUnit[i].shild < summonUnit[i].maxHp * 0.2f) {
                                    summonUnit[i].shild += summonUnit[i].maxHp * 0.2f - summonUnit[i].shild;
                                }
                            }
                        }

                        summonUnit[i].hp = summonUnit[i].maxHp;
                        
                    }
                    HealEffect heal = PoolingManager.Instance.ShowObject(healEffect.gameObject.name + "(Clone)", healEffect).GetComponent<HealEffect>();
                    heal.Setting(summonUnit[i].transform);
                }
            }

            currentSkillCoolTime -= Time.deltaTime;
        }
    }
}
