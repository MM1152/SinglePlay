using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPowerBuff : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get; set; }
    public float skillCoolTime;
    Transform playerList;
    private void Start() {
        playerList = GameObject.Find("PlayerList").transform;
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
            foreach(Transform child in playerList) {
                child.GetComponent<Unit>().statusEffectMuchine.SetStatusEffect(new AttackPowerBuffEffect());
            }
            skillCoolTime = soulsSkillData.skillCoolTime;
        }
    }
}
