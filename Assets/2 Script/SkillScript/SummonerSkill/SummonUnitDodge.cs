using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnitDodge : SummonerSkillParent
{
    public List<Unit> summonUnit = new List<Unit>();
    int count = 0;

    SkillData criticalSkillData;
    SkillData randomBuffSkillData;

    
    int criticalCount = 0;
    Type[] statusEffects = {
        Type.GetType("AttackPowerBuffEffect"),
        Type.GetType("SpeedBuffEffect")
    };

    private void Update()
    {
        if(SkillManager.Instance.UpgradeSummonUnitDodge && skillData == null) {
            skillData = SkillManager.Instance.GetSkillData("소환수회피");
        }
        if(SkillManager.Instance.UpgradeSummonUnitCritical && criticalSkillData == null) {
            criticalSkillData = SkillManager.Instance.GetSkillData("소환수크리티컬");
        }
        if(SkillManager.Instance.UpgradeSummonUnitBuff && randomBuffSkillData == null) {
            randomBuffSkillData = SkillManager.Instance.GetSkillData("소환수 랜덤버프");
            currentSkillCoolTime = SkillManager.Instance.skillDatas[randomBuffSkillData];
        }
        RandomBuff();
        SettingDodge();
        SettingCritical();
    }

    private void RandomBuff(){
        if(randomBuffSkillData != null && currentSkillCoolTime <= 0) {
            
            SetCoolTime(randomBuffSkillData);
            
            
            for(int i = 0; i < summonUnit.Count; i++) {
                if(summonUnit[i] == null) {
                    summonUnit.RemoveAt(i);
                    continue;
                }
                int index = UnityEngine.Random.Range(0 , statusEffects.Length);
                Debug.Log("Add Buff unit : " + summonUnit[i].unit.name +" BuffType : " + statusEffects[index].ToString());
                summonUnit[i].statusEffectMuchine.SetStatusEffect((IStatusEffect) Activator.CreateInstance(statusEffects[index]) , summoner , 10f , 0.3f);
            }
        }
        else if(randomBuffSkillData != null) {
            currentSkillCoolTime -= Time.deltaTime;
        }
    }
    private void SettingDodge() {
        if(skillData != null && count != SkillManager.Instance.skillDatas[skillData]) {
            for(int i = 0; i < summonUnit.Count; i++) {
                if(summonUnit[i] == null) {
                    summonUnit.RemoveAt(i);
                    continue;
                }
                summonUnit[i].dodge += (SkillManager.Instance.skillDatas[skillData] - count) * skillData.levelUpPercent;
                Debug.Log("Dodge : " + summonUnit[i].dodge);
            }
            count = SkillManager.Instance.skillDatas[skillData];
        }
    }
    private void SettingCritical(){
        if(criticalSkillData != null && criticalCount != SkillManager.Instance.skillDatas[criticalSkillData]){
            for(int i = 0; i < summonUnit.Count; i++) {
                if(summonUnit[i] == null) {
                    summonUnit.RemoveAt(i);
                    continue;
                }
                summonUnit[i].critical += (SkillManager.Instance.skillDatas[criticalSkillData] - count) * skillData.levelUpPercent;
                Debug.Log("Critical : " + summonUnit[i].critical);
            }
            count = SkillManager.Instance.skillDatas[skillData];
        }
    }

}
