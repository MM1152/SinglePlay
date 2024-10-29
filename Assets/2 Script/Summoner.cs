using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : LongRangeScript, IDamageAble
{
    [SerializeField] float skillCoolDown;
    [SerializeField] float skillCurrentTime;
    private void Awake()
    {
        base.Init(1);
        skillCoolDown = 2f;
        skillCurrentTime = skillCoolDown;
    }
    private void Update()
    {
        if (!isDie)
        {
            base.KeepChcek();
            if (SkillManager.Instance.SummonSkill) SummonSkill();
        }
    }
    public void Move(Vector3 movePos)
    {
        transform.position += movePos * Time.deltaTime * unit.speed;
    }

    public float getHp()
    {
        return hp;
    }

    public void SummonSkill()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ISummonUnit.unitCount < SkillManager.Instance.UpgradeMaxSummonCount)
        {
            if (skillCurrentTime <= 0)
            {
                ani?.SetBool("Skill", true);
                StartCoroutine(WaitForSkillAnimationCorutine());
            }
            skillCurrentTime -= Time.deltaTime;
        }
    }

    IEnumerator WaitForSkillAnimationCorutine()
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        ani.SetBool("Skill", false);
        
        GameObject unit = SkillManager.Instance.GetSummonGameObjet();
        unit = PoolingManager.Instance.ShowObject(unit.name + "(Clone)", unit);
        unit.transform.SetParent(transform.parent);
        unit.GetComponent<ISummonUnit>().summoner = this;
        unit.tag = gameObject.tag;
        unit.transform.position = transform.position + Vector3.right;

        skillCurrentTime = skillCoolDown;
    }

    
}
