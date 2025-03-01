using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : SummonerSkillParent
{

    [SerializeField] GameObject skillPrefeb;
    SkillData divisionLightningAttack;
    SkillData electricEffect;
    LightningAttack lightning;
    Animator ani;
    Heap heap;

    bool oneTime;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        base.Awake();
    }
    private void Update()
    {
        if (SkillManager.Instance.LightningAttack && skillData == null)
        {
            skillData = SkillManager.Instance.GetSkillData("번개 공격");
            SetCoolTime();
        }
        if (SkillManager.Instance.LightningAttackUpgrade && divisionLightningAttack == null)
        {
            divisionLightningAttack = SkillManager.Instance.GetSkillData("번개 분할");
        }
        if (SkillManager.Instance.LightningElectricEffetUpgrade && electricEffect == null)
        {
            electricEffect = SkillManager.Instance.GetSkillData("번개감전효과");
        }
        Skill();
    }
    public void Skill()
    {
        if (SkillManager.Instance.LightningAttack && summoner.target != null && !summoner.isDie && summoner.target.name != "NextStage")
        {
            if (currentSkillCoolTime <= 0)
            {
                PoolingManager.Instance.ShowObject(skillPrefeb.name + "(Clone)", skillPrefeb).GetComponent<LightningAttack>().Init(summoner.target.transform.position, summoner.transform.position);

                float damage = SetDamage(summoner.damage * skillData.initPercent);
                SkillAttack(summoner.target , damage);
                if(electricEffect != null) summoner.target.GetComponent<Unit>().statusEffectMuchine.SetStatusEffect(new ElectricEffect());
                SetCoolTime(); 
                if (divisionLightningAttack != null)
                {
                    heap = new Heap();
                    FindNearEnemy();

                    if (heap.heap.Count > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Transform nearTarget = heap.Pop();
                            if(nearTarget == default) continue;
                            PoolingManager.Instance.ShowObject(skillPrefeb.name + "(Clone)", skillPrefeb).GetComponent<LightningAttack>().Init(nearTarget.position, summoner.target.transform.position);
                            damage = SetDamage(summoner.damage * (divisionLightningAttack.initPercent + (SkillManager.Instance.skillDatas[divisionLightningAttack] * divisionLightningAttack.levelUpPercent)));
                            SkillAttack(nearTarget.gameObject , damage);
                            if(electricEffect != null) nearTarget.GetComponent<Unit>().statusEffectMuchine.SetStatusEffect(new ElectricEffect());
                        }
                    }
                }

                if(GameManager.Instance.isPlayingTutorial && !oneTime) {
                    oneTime = true;
                    GameManager.Instance.StartTutorial(13);
                    GameManager.Instance.StopGame();
                }
            }

            currentSkillCoolTime -= Time.deltaTime;
        }
    }

    void FindNearEnemy()
    {

        foreach (Transform nearTarget in summoner.targetList.transform)
        {
            if (summoner.target != nearTarget.gameObject 
                && Vector2.Distance(summoner.target.transform.position, nearTarget.transform.position) < 5f
                && nearTarget.GetComponent<IFollowTarget>().canFollow)
            {
                heap.Add(Vector2.Distance(summoner.target.transform.position, nearTarget.transform.position), nearTarget);
            }
        }
    }

}
