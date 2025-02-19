 using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpinningAttack : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get ; set ; }
    public float currentCoolTime;
    float targetingTime;
    float durationSkillTime;
    float attackAbleTime = 0f ;

    bool isTargeting;
    bool isdurationSkill;

    Unit unit;
    Animator ani;
    GameObject waringArea;
    GameObject copyWarningArea;
    //HeadAttack , DashAttack 모두 대기시간 존재해야함.
    private void Start() {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
        targetingTime = 2f;
        waringArea = Resources.Load<GameObject>("UseSkillFolder/BoxWarningArea");
    }
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }

    public void UseSkill()
    {
        if(unit.isDie || unit.isAttack) return;
        
        if(currentCoolTime <= 0) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            StartCoroutine(WaitAnimationCorutaine());
        }else {
            currentCoolTime -= Time.deltaTime;
        }
    }
    private void Update() {
        if(isTargeting) {
            Vector3 dir = unit.target.transform.position - transform.position;

            copyWarningArea.transform.rotation = Quaternion.FromToRotation(Vector3.up , dir) ;
            copyWarningArea.transform.Rotate(1f , 1f , copyWarningArea.transform.rotation.y - 90f);

            copyWarningArea.transform.position =  gameObject.transform.position;
            copyWarningArea.transform.position += Vector3.forward * 6f;
            copyWarningArea.transform.localScale = new Vector3(Vector2.Distance(unit.target.transform.position , gameObject.transform.position) , 1f , 1f);
        }
        if(isdurationSkill) {
            if(Vector2.Distance(unit.target.transform.position , gameObject.transform.position) > 0.8f) {
                unit.transform.position += (unit.target.transform.position - gameObject.transform.position).normalized * unit.speed * Time.deltaTime;
            }
           
            durationSkillTime -= Time.deltaTime;
            attackAbleTime -= Time.deltaTime;
        }
    }
    IEnumerator WaitAnimationCorutaine(){
        copyWarningArea = PoolingManager.Instance.ShowObject(waringArea.name + "(Clone)" , waringArea);
        
        isTargeting = true;
        
        unit.isSkill = true;
        
        yield return new WaitForSeconds(targetingTime);
        ani.SetBool("Skill", true);

        durationSkillTime = 5f;
        isdurationSkill = true;
        
        yield return new WaitUntil(() => durationSkillTime <= 0);

        ani.SetBool("Skill" ,false);
        unit.isSkill = false;
        PoolingManager.Instance.ReturnObject(copyWarningArea.name, copyWarningArea);
        isTargeting = false;
        isdurationSkill = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other != null && other.gameObject.tag != unit.gameObject.tag && isdurationSkill) {
            IDamageAble damageAble;
            if(other.TryGetComponent<IDamageAble>(out damageAble)) {
                damageAble.Hit(unit.damage * (soulsSkillData.attackPercent / 100f) , unit , unit.clitical , AttackType.SkillAttack);
                attackAbleTime = 0.5f;
            }
            else {
                Debug.LogError("Get Fail other IDamageAble");
            }
        }   
    }

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log(other.gameObject.name);
        if(other != null && other.gameObject.tag != unit.gameObject.tag && isdurationSkill && attackAbleTime <= 0) {
            IDamageAble damageAble;
            if(other.TryGetComponent<IDamageAble>(out damageAble)) {
                damageAble.Hit(unit.damage * (soulsSkillData.attackPercent / 100f) , unit , unit.clitical , AttackType.SkillAttack);
                attackAbleTime = 0.5f;
            }
            else {
                Debug.LogError("Get Fail other IDamageAble");
            }
        }
    }

}
