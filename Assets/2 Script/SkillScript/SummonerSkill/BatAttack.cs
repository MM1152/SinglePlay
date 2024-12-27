using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BatAttack : MonoBehaviour
{
    [SerializeField] GameObject batPrefeb;
    public SkillData batSkill;
    public SkillData batUpgradeAttackPercent;
    public SkillData batUpgradeCoolTime;
    public float currentCoolTime;
    int maxSpawnCount;
    int currSpawnCount;
    Summoner summoner;

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
        
        summoner = GetComponent<Summoner>();
    }

    private void Update()
    {
        if (SkillManager.Instance.batAttack && batSkill == null) {
            batSkill = SkillManager.Instance.GetSkillData("박쥐 소환");
            currentCoolTime = batSkill.coolTime;
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
        if (!SkillManager.Instance.batAttack && !summoner.isDie) return;

        if (currSpawnCount <= maxSpawnCount)
        {
            currentCoolTime -= Time.deltaTime;
        }

        if (currentCoolTime <= 0)
        {
            currentCoolTime = batSkill.coolTime - (batSkill.coolTime * (batUpgradeCoolTime != null ? batUpgradeCoolTime.initPercent * SkillManager.Instance.skillDatas[batUpgradeCoolTime] : 0));

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

            bat.Setting(summoner, batSkill.initPercent + (SkillManager.Instance.skillDatas[batSkill] * batSkill.levelUpPercent) , i , this , batUpgradeAttackPercent);
        }
    }
    public void Die(int spawnNumber){
        currSpawnCount--;
        canSpawn[spawnNumber] = false;
    }
}

