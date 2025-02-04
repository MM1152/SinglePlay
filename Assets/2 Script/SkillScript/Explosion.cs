using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour , SkillParent 
{
    public SoulsSkillData soulsSkillData { get ; set ; }
    GameObject explosionObject;
    [SerializeField] float currentSkillCoolTime;
    Unit unit;
    Animator ani;
    public float GetSkillCoolTime()
    {
        return currentSkillCoolTime;
    }

    public void UseSkill()
    {
        
        currentSkillCoolTime -= Time.deltaTime;

        if(currentSkillCoolTime < 0 && !unit.isAttack && !unit.isSkill) {
            currentSkillCoolTime = soulsSkillData.skillCoolTime;
            unit.isSkill = true;
            ani.SetBool("Skill" , true);

            ExplosionObject pre = PoolingManager.Instance.ShowObject(explosionObject.name + "(Clone)" , explosionObject).GetComponent<ExplosionObject>();
            pre.transform.position = unit.target.transform.position + Vector3.down + Vector3.forward * 2f;
            pre.tag = tag;
            pre.Setting(unit.damage * soulsSkillData.attackPercent , unit);            
            StartCoroutine(WaitAttack());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        explosionObject = Resources.Load<GameObject>("UseSkillFolder/Explosion");
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
        currentSkillCoolTime = soulsSkillData.skillCoolTime;
    }

    IEnumerator WaitAttack(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        unit.isSkill = false;
        ani.SetBool("Skill" , false);
    }
}
