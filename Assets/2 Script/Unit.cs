
using System.Collections;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;


public class Unit : MonoBehaviour {
    public UnitData unit;
    protected float setInitAttackSpeed; // 초기화될 공격속도
    [SerializeField] protected float currentAttackSpeed; // 현재 공격까지 남은시간
    protected Animator ani = null;
    protected SpriteRenderer sp;

    /**************Status****************/
    [Header("Status")]
    public bool canAttack;
    public bool isAttack;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
    public float maxHp;
    [Space(75)]
    /************************************/


    /************Targeting***************/
    public bool isDie;
    public GameObject target; // 공격할 대상
    [SerializeField] protected GameObject targetList; // 적이라면 Player를 담고있는 부모 , Player라면 적에 대한 정보를 담고있는 부모
    /************************************/

    /*************TestCode***************/
    [Space(10)]
    [Header("TestCodes")]
    public bool DontAttack;
    /************************************/
    
    protected void Respawn(){
        isDie = false;
        isAttack = false;
        hp = maxHp;
    }
    protected virtual void Init(float setStatus) {
        if(gameObject.CompareTag("Enemy")) targetList = GameObject.Find("PlayerList");
        sp ??= GetComponent<SpriteRenderer>() ;
        ani ??= GetComponent<Animator>() ?  GetComponent<Animator>() : null;

        hp = unit.hp * setStatus;
        mp = unit.mp;
        damage = unit.damage * setStatus;
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;
        
        maxHp = hp;
    }
    protected virtual void Init(Summoner summoner , float precent) {
        if(gameObject.CompareTag("Enemy")) targetList = GameObject.Find("PlayerList");
        sp ??= GetComponent<SpriteRenderer>() ;
        ani ??= GetComponent<Animator>() ?  GetComponent<Animator>() : null;

        hp = summoner.hp * precent;
        mp = unit.mp;
        damage = summoner.damage * precent;
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;
        
        maxHp = hp;
    }
    protected void ChangeStats(Summoner summoner , float precent){
        hp = summoner.hp * precent;
        mp = unit.mp;
        damage = summoner.damage * precent;
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;
        maxHp = hp;
    }
    protected virtual void Attack(){
        canAttack = !DontAttack && target != null && attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;
        
        if(canAttack) {
            isAttack = true;
            ani?.SetBool("Attack" , true);
            currentAttackSpeed = setInitAttackSpeed;
            StartCoroutine(WaitForAttackAnimationCorutine());
        }
        
    }
    protected virtual void KeepChcek() {
        currentAttackSpeed -= Time.deltaTime;
        Die();
        Flip();
        if(gameObject.name == "Player" && VirtualJoyStick.instance.isInput) {
            Debug.Log("Stop");
            return;
        } else {
            FollowTarget();
            ani?.SetBool("Move", FollowTarget());
        }
    }

    protected void Die(){
        //:fix DieAnimation 이후 SpawnEnemyCount-- 해주기
        if(hp <= 0) {
            isDie = true;
            ani?.SetBool("Die" , isDie);
            target = null;
            if(gameObject.CompareTag("Enemy")) {
                EnemySpawner.Instance.CheckDie();
            }
            
            StartCoroutine(WaitForDieAnimationCorutine());
        }
    }

    protected bool FollowTarget(){
        if(isAttack) return false;
        if(target != null && target.GetComponent<Unit>().isDie) target = null;
        if(target == null) {
            target = FindTarget(targetList);
            return false;
        }

        if(!isAttack) {
            target = FindTarget(targetList);
        }
        
        if(Vector2.Distance(target.transform.position , transform.position) < attackRadious || isAttack) return false;

        transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        return true;
    }

    protected GameObject FindTarget(GameObject TargetList){  

        GameObject returnGameObject = null;
        float minDistance = 9999999f;

        foreach(Transform targets in TargetList.transform) {
            
            if(Vector2.Distance(targets.position , transform.position) < minDistance && !targets.GetComponent<Unit>().isDie) {
                minDistance = Vector2.Distance(targets.position , transform.position);
                returnGameObject = targets.gameObject;
            }
        }
        return returnGameObject;
    }

    protected void Flip(){
        if(target != null) sp.flipX = (target.transform.position - transform.position).normalized.x >= 0 ? false : true;
        
    }

    protected IEnumerator WaitForDieAnimationCorutine(){ 
        if(ani != null) yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("DIE") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        StopAllCoroutines();
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
    protected IEnumerator WaitForAttackAnimationCorutine(){ 
        if(ani != null) yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        else yield return new WaitForSeconds(0.2f);
        ani?.SetBool("Attack" , false);
        isAttack = false;
    }


}