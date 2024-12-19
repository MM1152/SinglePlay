using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : SkillParent
{
    Unit unit;

    GameObject tauntRadious;
    Animator ani;

    float skillCoolTime;
    bool useSkill;

    private void OnDisable() {
        StopAllCoroutines();     
    }

    private void Awake() {
        unit = GetComponent<Unit>();
    }

    private void Start() {
        skillCoolTime = soulsSkillData.skillCoolTime;

        tauntRadious = Resources.Load<GameObject>("UseSkillFolder/Taunt");
        tauntRadious = Instantiate(tauntRadious , transform);
        ani = tauntRadious.GetComponent<Animator>();
        tauntRadious.transform.localPosition = new Vector3(0f , -1f , 1f);
        tauntRadious.SetActive(false);
    }

    private void Update() {
        UseSkill();
    }

    public override void UseSkill()
    {
        if(skillCoolTime > 0 && !useSkill) skillCoolTime -= Time.deltaTime;
        else if(!useSkill){
            skillCoolTime = soulsSkillData.skillCoolTime;

            StartCoroutine(UseTauntSkill());
        }
    }

    IEnumerator UseTauntSkill(){
        tauntRadious.SetActive(true);
        useSkill = true;

        FindTarget();
        ani.Play("TauntAnimation" , 0 , 0f);
        yield return new WaitUntil(() =>ani.GetCurrentAnimatorStateInfo(0).IsName("TauntAnimation") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        tauntRadious.SetActive(false);
        useSkill = false;
        
    }


    void FindTarget(){
        foreach(Transform enemy in unit.targetList.transform) {
            if(Vector2.Distance(enemy.transform.position , transform.position) <= 10f) {
                Unit unit = enemy.GetComponent<Unit>();
                unit.target = this.gameObject;
                unit.statusEffectMuchine.SetStatusEffect(new TauntEffect() , unit);
            }
        }
    }

    public override float GetSkillCoolTime()
    {
        return skillCoolTime;
    }
}
