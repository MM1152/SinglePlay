using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField]Animator ani;
    [SerializeField]Summoner summoner;
    [SerializeField]SpriteRenderer sp;
    [SerializeField]BatAttack batAttack;
    [SerializeField]SkillData skillData;
    SummonerSkillParent summonerSkillParent;

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

    public void Setting(Summoner summoner , float attackDamage , int spawnNumber , BatAttack batAttack , SkillData skillData , SummonerSkillParent skillParent = null){
        this.summoner = summoner;
        this.attackDamage = attackDamage;
        this.spawnNumber = spawnNumber;
        this.batAttack = batAttack;
        this.skillData = skillData;
    }

    void Update()
    {
        if(!oneTime && summoner.isAttack) {
            oneTime = true;
            float rand = Random.Range(0f , 1f); 
            
            if(rand <= attackPercent + (skillData != null ? skillData.initPercent * SkillManager.Instance.skillDatas[skillData] : 0)) {
                ani.SetBool("Attack" , true);
                isAttack = true;
            }
        }

        if(isAttack) {
            if(summoner.target.name != "NextStage") transform.position += (summoner.target.transform.position - transform.position) * 3f * Time.deltaTime;
            if(summoner.target.name == "NextStage") PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }

        if(!summoner.isAttack) {
            oneTime = false;
        }
        sp.flipX = summoner.sp.flipX;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(isAttack && other.gameObject == summoner.target.gameObject) {
            summonerSkillParent.SkillAttack(other.gameObject , summoner.damage * attackDamage);
            batAttack.Die(spawnNumber);
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }
}
