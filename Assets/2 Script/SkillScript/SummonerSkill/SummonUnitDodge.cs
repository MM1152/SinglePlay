using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnitDodge : SummonerSkillParent
{
    public List<Unit> summonUnit = new List<Unit>();
    int count = 0;
    private void Update()
    {
        if(SkillManager.Instance.UpgradeSummonUnitDodge && skillData == null) {
            skillData = SkillManager.Instance.GetSkillData("소환수회피");
        }
        if(skillData != null && count != SkillManager.Instance.skillDatas[skillData]) {
            for(int i = 0; i < summonUnit.Count; i++) {
                summonUnit[i].dodge += (SkillManager.Instance.skillDatas[skillData] - count) * skillData.levelUpPercent;
                Debug.Log("Dodge : " + summonUnit[i].dodge);
            }
            count = SkillManager.Instance.skillDatas[skillData];
        }
    }
}
