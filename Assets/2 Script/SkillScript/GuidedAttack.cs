using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedAttack : MonoBehaviour , SkillParent
{
    public float skillCoolTime;
    Unit unit;
    Animator ani;
    GameObject guidedSkillPrefeb;

    public SoulsSkillData soulsSkillData { get ; set ; }

    private void Awake()
    {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
        guidedSkillPrefeb = Resources.Load<GameObject>("UseSkillFolder/GuidedAttackPrefeb");
    }

    private void Start()
    {
        skillCoolTime = soulsSkillData.skillCoolTime;
    }
    public float GetSkillCoolTime()
    {
        return skillCoolTime;
    }

    public void UseSkill()
    {
        skillCoolTime -= Time.deltaTime;
        if (skillCoolTime <= 0 && !unit.isSkill && !unit.isAttack)
        {
            skillCoolTime = soulsSkillData.skillCoolTime;
            ani.SetBool("Skill", true);
            unit.isSkill = true;
            StartCoroutine(WaitForAttackSkillAnimation("SKILL", 0.5f));
        }
    }
    IEnumerator WaitForAttackSkillAnimation(string animationParameter, float normalizedTime)
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        unit.isAttack = false;
        ani.SetBool("Attack", false);

        for (float i = 0.2f; i <= 1f; i += 0.2f)
        {
            GameObject skill = PoolingManager.Instance.ShowObject(guidedSkillPrefeb.name + "(Clone)", guidedSkillPrefeb);
            skill.GetComponent<WideRangeSkill>().unit = unit;
            skill.GetComponent<WideRangeSkill>().skillData = soulsSkillData;
            skill.transform.position = Vector2.Lerp(transform.position, unit.target.transform.position, i);
            yield return new WaitForSeconds(0.2f);
        }


        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        //oneTime = true;
        unit.isSkill = false;
        ani.SetBool("Skill", false);
    }

}
