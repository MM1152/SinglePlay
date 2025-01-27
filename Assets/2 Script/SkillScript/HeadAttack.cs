using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAttack : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get; set; }
    float currentCoolTime;

    Unit unit;
    Animator ani;
    float atttackDelay;
    private void Awake() {
        ani = GetComponent<Animator>();
        unit = GetComponent<Unit>();
        atttackDelay = 0.7f;
    }
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }

    public void UseSkill()
    {
        currentCoolTime -= Time.deltaTime;
        if(currentCoolTime <= 0 && Vector2.Distance(unit.target.transform.position , unit.transform.position) <= unit.unit.attackRadious) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            ani.SetBool("Skill2" , true);
            InWarningArea warningArea = PoolingManager.Instance.ShowObject("CircleWarningArea(Clone)").GetComponent<InWarningArea>();
            warningArea.SetPosition(unit.target.transform.position);
            warningArea.Setting(soulsSkillData.attackPercent / 100f , atttackDelay , unit);
            StartCoroutine(WaitAnimation());
        }
        
    }

    IEnumerator WaitAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("HEADATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.3f);
        ani.speed = 0f;
        yield return new WaitForSeconds(atttackDelay - 0.3f);
        ani.speed = 1f;
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("HEADATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        ani.SetBool("Skill2" , false);
    }
}
