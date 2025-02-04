using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BatAttack : SummonerSkillParent
{
    [SerializeField] GameObject batPrefeb;
    public SkillData batUpgradeAttackPercent;
    public SkillData batUpgradeCoolTime;
    int maxSpawnCount;
    int currSpawnCount;

    Vector2[] spawnPosition = {
        new Vector2(-0.5f , 0f),
        new Vector2(-0.35f , 0.4f),
        new Vector2(0.3f , 0.4f),
        new Vector2(0.4f , 0f)
    };

    bool[] canSpawn = {
        false,
        false,
        false,
        false
    };
    private void Awake()
    {
        maxSpawnCount = 3;
        currSpawnCount = 0;
        base.Awake();
    }

    private void Update()
    {
        if (SkillManager.Instance.batAttack && skillData == null) {
            skillData = SkillManager.Instance.GetSkillData("박쥐 소환");
            SetCoolTime();
        } 
        if (SkillManager.Instance.batUpgradeAttackPercent && batUpgradeAttackPercent == null) {
            batUpgradeAttackPercent = SkillManager.Instance.GetSkillData("박쥐 공격확률 증가");
        } 
        if (SkillManager.Instance.batUpgradeCoolTime && batUpgradeCoolTime == null) {
            batUpgradeCoolTime = SkillManager.Instance.GetSkillData("박쥐 생성속도 증가");
        } 
        BatSkill();
    }
    private void BatSkill()
    {
        if (!SkillManager.Instance.batAttack || summoner.isDie) return;

        if (currSpawnCount <= maxSpawnCount)
        {
            currentSkillCoolTime -= Time.deltaTime;
        }

        if (currentSkillCoolTime <= 0)
        {
            SetCoolTime();
            currentSkillCoolTime = currentSkillCoolTime - (skillData.coolTime * (batUpgradeCoolTime != null ? batUpgradeCoolTime.initPercent * SkillManager.Instance.skillDatas[batUpgradeCoolTime] : 0));

            Bat bat = PoolingManager.Instance.ShowObject(batPrefeb.name + "(Clone)" , batPrefeb).GetComponent<Bat>();
            bat.transform.SetParent(transform);

            int i;
            for (i = 0; i < canSpawn.Length; i++)
            {
                if (!canSpawn[i])
                {
                    bat.transform.localPosition = spawnPosition[i];
                    canSpawn[i] = true;
                    currSpawnCount++;
                    break;
                }
            }

            float damage = SetDamage(skillData.initPercent + (SkillManager.Instance.skillDatas[skillData] * skillData.levelUpPercent));
            bat.Setting(summoner, damage , i , this , batUpgradeAttackPercent , this);
        }
    }
    public void Die(int spawnNumber){
        currSpawnCount--;
        canSpawn[spawnNumber] = false;
    }
}

