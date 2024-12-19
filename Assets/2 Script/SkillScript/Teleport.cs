using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : SkillParent
{
    public float skillCoolTime;
    Unit unit;
    Animator ani;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
    }
    private void Start()
    {
        skillCoolTime = soulsSkillData.skillCoolTime;
    }
    public override float GetSkillCoolTime()
    {
        return skillCoolTime;
    }

    private void Update() {
        UseSkill();
    }
    public override void UseSkill()
    {
        skillCoolTime -= Time.deltaTime;
        if (skillCoolTime <= 0 && !unit.isSkill && !unit.isAttack)
        {
            skillCoolTime = soulsSkillData.skillCoolTime;
            ani.SetBool("Skill2", true);
            unit.isSkill = true;
            StartCoroutine(WaitForAttackSkillAnimation("SKILL2", 0.4f));
        }
    }
    IEnumerator WaitForAttackSkillAnimation(string animationParameter, float normalizedTime)
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        unit.isAttack = false;
        ani.SetBool("Attack", false);


        unit.canFollow = false;

        GameObject WarningArea = PoolingManager.Instance.ShowObject("WarningArea(Clone)");
        InWarningArea inWarningArea = WarningArea.GetComponent<InWarningArea>();

        inWarningArea.Setting(1.2f, 1f, unit);
        inWarningArea.gameObject.SetActive(true);
        inWarningArea.SetPosition(unit.target.transform.position + Vector3.down * 0.6f);

        gameObject.transform.position = unit.target.transform.position;
        gameObject.transform.position += Vector3.forward * 5f;

        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f);

        unit.canFollow = true;
        gameObject.transform.position -= Vector3.forward * 5f;


        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        //unit.oneTime = true;
        unit.isSkill = false;
        ani.SetBool("Skill2", false);
    }

}
