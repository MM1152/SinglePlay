using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    Transform target;
    Animator ani;
    Summoner summoner;
    SpriteRenderer sp;
    BatAttack batAttack;
    SkillData skillData;

    float attackPercent;
    float attackDamage;
    int spawnNumber;

    bool oneTime;
    bool isAttack;
    private void OnEnable(){
        isAttack = false;
        oneTime = false;
    }
    private void Awake() {
        attackPercent = 0.15f;
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        
    }

    public void Setting(Summoner summoner , float attackDamage , int spawnNumber , BatAttack batAttack , SkillData skillData){
        this.summoner = summoner;
        this.attackDamage = attackDamage;
        this.spawnNumber = spawnNumber;
        this.batAttack = batAttack;
        this.skillData = skillData;
    }

    void Update()
    {
        Debug.Log(attackPercent + (skillData != null ? skillData.initPercent * SkillManager.Instance.skillDatas[skillData] : 0));
        if(!oneTime && summoner.isAttack) {
            oneTime = true;
            float rand = Random.Range(0f , 1f); 
            
            if(rand <= attackPercent + (skillData != null ? skillData.initPercent * SkillManager.Instance.skillDatas[skillData] : 0)) {
                ani.SetBool("Attack" , true);
                isAttack = true;
            }
        }

        if(isAttack) {
            transform.position += (summoner.target.transform.position - transform.position) * 3f * Time.deltaTime;
        }

        if(!summoner.isAttack) {
            oneTime = false;
        }
        sp.flipX = summoner.sp.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isAttack && other.gameObject == summoner.target.gameObject) {
            other.GetComponent<IDamageAble>().Hit(summoner.damage * attackDamage);
            batAttack.Die(spawnNumber);
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }
}
