
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using Unity.VisualScripting;
using UnityEngine;


public class Unit : MonoBehaviour, IFollowTarget, ISpawnPosibillity, IDamageAble
{
    public UnitData unit;
    public List<SkillParent> skillData;
    [Range(0f, 1f)] public float attackObjectShowTime;
    public float setInitAttackSpeed; // 초기화될 공격속도
    public float currentAttackSpeed; // 현재 공격까지 남은시간
    [SerializeField] protected Animator ani = null;
    public SpriteRenderer sp;


    /**************Status****************/
    [Header("Status")]
    public bool canAttack;
    public bool isAttack;
    public bool isSkill;
    public float hp;
    public float shild;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
    public float maxHp;
    public float spawnProbabillity { get; set; }
    public StatusEffect statusEffectMuchine;
    public bool SkillSetting;
    [Space(75)]
    /************************************/


    /************Targeting***************/
    public bool isDie;
    public bool canFollow { get; set; }
    public int tauntStatusEffect;

    public GameObject target; // 공격할 대상
    public GameObject targetList; // 적이라면 Player를 담고있는 부모 , Player라면 적에 대한 정보를 담고있는 부모
    /************************************/

    /*************TestCode***************/


    /************************************/
    protected virtual void Awake()
    {

        skillData = new List<SkillParent>();
        statusEffectMuchine = new StatusEffect(this);
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>() ? GetComponent<Animator>() : null;
        canFollow = true;
    }

    protected virtual void Update()
    {
        if (!isDie)
        {
            Die();
            statusEffectMuchine?.Update();
            if (GameManager.Instance.playingAnimation || GameManager.Instance.playingShader)
            {
                ani.SetBool("Move", false);
                return;
            }
            if (!isAttack && !isSkill) {

                currentAttackSpeed -= Time.deltaTime;
                
            }
            if (!isAttack && !isSkill && !GameManager.Instance.gameClear && target != null)
            {
                foreach (SkillParent skill in skillData)
                {
                    skill.UseSkill();
                }
            }
            Flip();
            ani?.SetBool("Move", FollowTarget());

        }

    }

    protected void Respawn()
    {
        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss")) targetList = GameObject.Find("PlayerList");
        if (gameObject.CompareTag("Player")) targetList = GameObject.Find("EnemyList");
        isDie = false;
        isAttack = false;
        hp = maxHp;
        canFollow = true;
    }
    protected virtual void Spawn(float setStatus)
    {
        maxHp = unit.hp + (unit.hp * setStatus * 0.1f);
        mp = unit.mp;
        damage = unit.damage + (unit.damage * setStatus * 0.1f);
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;

        hp = maxHp;
    }
    protected virtual void SummonerSpawn(Summoner summoner, string unitName)
    {
        float attackPrecent = GameManager.Instance.soulsInfo[unitName].curStat.attackStat / 100f;
        float hpPercent = GameManager.Instance.soulsInfo[unitName].curStat.hpStat / 100f;

        float bonusAttack = 0;
        float bonusHp = 0;
        if (SkillManager.Instance.UpgradeSummonUnitSkill)
        {
            SkillData skillData = SkillManager.Instance.GetSkillData("소환수 강화");
            bonusAttack = SkillManager.Instance.skillDatas[skillData] * skillData.initPercent;
            bonusHp = SkillManager.Instance.skillDatas[skillData] * skillData.initPercent;
        }
        bonusHp += (GameManager.Instance.reclicsDatas[1].inItPercent +
                    (GameManager.Instance.reclicsDatas[1].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[1]))
                    / 100f;

        bonusAttack += (GameManager.Instance.reclicsDatas[0].inItPercent +
                    (GameManager.Instance.reclicsDatas[0].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[0]))
                    / 100f;
        maxHp = summoner.maxHp * (hpPercent + bonusHp);
        mp = unit.mp;
        damage = summoner.damage * (attackPrecent + bonusAttack);
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;

        hp = maxHp;
    }
    protected void ChangeStats(Summoner summoner, float attackPrecent, float hpPercen)
    {
        float thisHpPercent = hp / maxHp;

        maxHp = summoner.hp * hpPercen;
        mp = unit.mp;
        damage = summoner.damage * attackPrecent;
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;

        hp = maxHp * thisHpPercent;
    }
    protected virtual void Attack()
    {
        canAttack = !isSkill && !isAttack && target != null && attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;

        if (canAttack)
        {
            isAttack = true;
            ani?.SetBool("Attack", true);
            currentAttackSpeed = setInitAttackSpeed;
            StartCoroutine(WaitForAttackAnimationCorutine());
        }

    }
    /*
    protected virtual void KeepChcek() {
        if(GameManager.Instance.gameClear && target?.name != "NextStage") target = null; 
        
        
        if(gameObject.name == "Player" && VirtualJoyStick.instance.isInput) {
            target = FindTarget(targetList);
            ani.SetBool("Move" , true);
            return;
        } 
    }
    */
    protected void Die()
    {
        if (hp <= 0)
        {

            isDie = true;
            canFollow = false;
            ani?.SetBool("Die", isDie);
            target = null;
            if (gameObject.CompareTag("Enemy") && !GameManager.Instance.gameClear)
            {
                EnemySpawner.Instance.CheckDie();
                DropSoul();
            }

            StartCoroutine(WaitForDieAnimationCorutine());
        }
        if (gameObject.CompareTag("Enemy") && GameManager.Instance.gameClear && hp >= 0)
        {
            hp = 0;
        }
    }
    protected bool FollowTarget()
    {
            if (target != null && !target.GetComponent<IFollowTarget>().canFollow) target = null;

            if (target == null)
            {
                target = FindTarget(targetList);
                return false;
            }

            // 도발 상태이상에 걸린 상태라면 FindTarget을 수행하지 않도록 변경
            if (target?.name != "NextStage" && statusEffectMuchine.GetStatusEffect(new TauntEffect()))
            {
                target = FindTarget(targetList);
            }

            if (target?.name != "NextStage" && Vector2.Distance(target.transform.position, transform.position) < attackRadious || isAttack) return false;
            if (isAttack || isSkill) return false;

        

        transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        return true;
    }
    protected GameObject FindTarget(GameObject TargetList)
    {

        GameObject returnGameObject = null;
        float minDistance = 9999999f;

        foreach (Transform targets in TargetList.transform)
        {

            if (Vector2.Distance(targets.position, transform.position) < minDistance && targets.GetComponent<IFollowTarget>().canFollow)
            {
                minDistance = Vector2.Distance(targets.position, transform.position);
                returnGameObject = targets.gameObject;
            }
        }
        return returnGameObject;
    }

    protected void Flip()
    {
        if (target != null && gameObject.name != "Demon(Clone)") sp.flipX = (target.transform.position - transform.position).normalized.x >= 0 ? false : true;
        else if (target != null) sp.flipX = (target.transform.position - transform.position).normalized.x >= 0 ? true : false;
    }

    private IEnumerator WaitForDieAnimationCorutine()
    {
        if (ani != null) yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("DIE") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        StopAllCoroutines();
        statusEffectMuchine.Exit();
        if (gameObject.CompareTag("Enemy"))
        {
            PoolingManager.Instance.ReturnObject(gameObject.name, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private IEnumerator WaitForAttackAnimationCorutine()
    {
        if (ani != null) yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        //else yield return new WaitForSeconds(0.2f);
        ani?.SetBool("Attack", false);
        isAttack = false;
    }

    private void DropSoul()
    {
        float rand = Random.Range(0f, 1f);

        if (unit.classStruct.dropSoulpercent >= rand)
        {
            GameManager.Instance.dropSoul?.Invoke(unit);
        }
    }

    public void Hit(float Damage)
    {
        if (shild >= Damage) shild -= Damage;
        else if (shild < Damage)
        {
            hp -= (Damage - shild);
            shild = 0;
        }
    }

}
