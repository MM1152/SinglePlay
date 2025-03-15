
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;



[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour, IFollowTarget, ISpawnPosibillity, IDamageAble
{
    public Rigidbody2D rg;
    public CircleCollider2D collider;
    public UnitData unit;
    public List<SkillParent> skillData;
    public ISummonUnit summonUnit;
    [Range(0f, 1f)] public float attackObjectShowTime;
    public float setInitAttackSpeed; // 초기화될 공격속도
    public float currentAttackSpeed; // 현재 공격까지 남은시간
    public GameObject hpBar;
    [HideInInspector] protected Animator ani = null;
    [HideInInspector] public SpriteRenderer sp;

    protected float cliticalPercent;

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
    public float dodge;
    public float critical;
    [Space(75)]
    /************************************/


    /************Targeting***************/
    public bool isDie;
    public bool canFollow { get; set; }
    public int tauntStatusEffect;

    private Transform damageShowPos;
    public Transform hpbarPos;

    public GameObject target; // 공격할 대상
    public GameObject targetList; // 적이라면 Player를 담고있는 부모 , Player라면 적에 대한 정보를 담고있는 부모

    //현재 소환되는 유닛의 레벨에 따라 적용되는 수치
    public float attackPrecent;
    public float hpPercent;

    //서머너의 레벨업 특성으로 찍은 유닛의 추가강화
    public float bonusAttack = 0;
    public float bonusHp = 0;
    public float bonusSpeed = 0;

    public float buffAttack = 0;
    public float buffSpeed = 0;
    //현재 소환수가 쌓은 데미지량
    public int overlapDamage;
    /************************************/

    /*************TestCode***************/


    /************************************/

    protected virtual void Awake()
    {
        overlapDamage = 0;
        cliticalPercent = 1.5f;
        rg = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        rg.gravityScale = 0;
        rg.sleepMode = RigidbodySleepMode2D.NeverSleep;
        damageShowPos = transform.Find("DamageShowPosition");
        hpbarPos =  transform.Find("HpbarPos");
        hpbarPos.transform.position -= new Vector3(0f , 0f , hpbarPos.transform.position.z);

        skillData = new List<SkillParent>();
        if(!GameManager.Instance.setting.showHpButton.isSelect) hpBar = Resources.Load<GameObject>("UI/HpCanvas");
        statusEffectMuchine = new StatusEffect(this);
        sp = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>() ? GetComponent<Animator>() : null;
        canFollow = true;
        
        if(hpBar != null) {
            GameObject hpbarCanvas = Instantiate(this.hpBar , hpbarPos);
            Hpbar hpbar = hpbarCanvas.transform.GetComponentInChildren<Hpbar>();
            hpbarCanvas.transform.position = hpbarPos.position;
            hpbarCanvas.transform.position += Vector3.forward * 1f;
            hpbar.target = this;
        }

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

            if (!isAttack ) {
                currentAttackSpeed -= Time.deltaTime;
                Flip();
            }

            if (!isAttack && !isSkill && !GameManager.Instance.gameClear && target != null && !GameManager.Instance.playingAnimation)
            {
                foreach (SkillParent skill in skillData)
                {
                    skill.UseSkill();
                }
            }
            
            ani?.SetBool("Move", FollowTarget());

        }

    }
    protected void Respawn()
    {
        if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss")) targetList = GameObject.Find("PlayerList");
        if (gameObject.CompareTag("Player")) targetList = GameObject.Find("EnemyList");
        collider.enabled = true;
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

    protected virtual void SummonerSpawn(Summoner summoner)
    { 
        Debug.Log($"Unit SummonerSpawn : {gameObject.name}");
        attackPrecent = GameManager.Instance.soulsInfo[unit.name].curStat.attackStat / 100f;
        hpPercent = GameManager.Instance.soulsInfo[unit.name].curStat.hpStat / 100f;

        maxHp = summoner.maxHp * (hpPercent);
        mp = unit.mp;
        damage = summoner.damage * (attackPrecent);
        speed = unit.speed;
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;
        critical = summoner.critical;
    
        hp = maxHp;
        summonUnit = GetComponent<ISummonUnit>();
    }

    public void ChangeStats()
    {
        Debug.Log($"Unit Change Stats : {gameObject.name}");
        if (SkillManager.Instance.UpgradeSummonUnitSkill)
        {
            SkillData skillData = SkillManager.Instance.GetSkillData("소환수 강화");
            bonusAttack = SkillManager.Instance.skillDatas[skillData] * skillData.initPercent;
            bonusHp = SkillManager.Instance.skillDatas[skillData] * skillData.initPercent;
        }
        if (SkillManager.Instance.UpgradeSummonUnitSpeed)
        {
            SkillData skillData = SkillManager.Instance.GetSkillData("소환수 이동속도 증가");
            bonusSpeed = SkillManager.Instance.skillDatas[skillData] * skillData.initPercent;
        }
        float thisHpPercent = hp / maxHp;

        maxHp = summonUnit.summoner.hp * (hpPercent + bonusHp);
        mp = unit.mp;
        damage = summonUnit.summoner.damage * (attackPrecent + bonusAttack + buffAttack);
        speed = unit.speed * (1 + bonusSpeed + buffSpeed); 
        attackRadious = unit.attackRadious;
        setInitAttackSpeed = unit.attackSpeed;
        critical = summonUnit.summoner.critical;

        hp = maxHp * thisHpPercent;
    }
    
    protected virtual void Attack()
    {
        canAttack = !isSkill && !isAttack && target != null && attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;

        if (canAttack)
        {
            isAttack = true;
            if(setInitAttackSpeed >= 1) ani.SetFloat("AttackSpeed" , 1f);
            else ani.SetFloat("AttackSpeed" , 1f / setInitAttackSpeed);
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
            collider.enabled = false;
            isDie = true;
            canFollow = false;
            ani?.SetBool("Die", isDie);
            target = null;
            if (gameObject.CompareTag("Enemy") || gameObject.CompareTag("Boss") && !GameManager.Instance.gameClear)
            {
                if(gameObject.CompareTag("Boss")) DailyQuestTab.ClearDailyQuest(QuestType.ClearBoss , 1);
                
                EnemySpawner.Instance.CheckDie();
                DropSoul();
            }else if(summonUnit != null){
                summonUnit.DieSummonUnit(this);
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

            if (targets.GetComponent<IFollowTarget>().canFollow && Vector2.Distance(targets.position, transform.position) < minDistance )
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
        float rand = UnityEngine.Random.Range(0f, 1f);

        if (unit.classStruct.dropSoulpercent >= rand)
        {
            GameManager.Instance.dropSoul?.Invoke(unit);
        }
    }

    public void Hit(float Damage, Unit applyunit, float Critical = 0, AttackType attackType = AttackType.None)
    { 
        //\\TODO 여기서 스킬데미지증가 유물에 관해서 데미지 증가 로직 적용시켜주면 될거같음.
        DamageText damage = PoolingManager.Instance.ShowDamage().GetComponent<DamageText>();
        
        if(applyunit != null) {
            bool isclitical = UnityEngine.Random.Range(0f , 1f) <= applyunit.critical ? true : false;
            if(isclitical) {
                Damage = Damage * applyunit.cliticalPercent;
                attackType = AttackType.CriticalAttack ;
            }
        }
        
        float rand = UnityEngine.Random.Range(0f , 1f);
        if(rand <= dodge) {
            damage.Setting(AttackType.Dodge);
            damage.damage = 0;
            damage.target = damageShowPos;
            SoundManager.Instance.Play(SoundManager.SFX.Dodge);
            return;
        }
        SoundManager.Instance.Play(SoundManager.SFX.Hit);
        damage.Setting(attackType);
        damage.damage = (int) Damage;
        damage.target = damageShowPos;

        
        if(applyunit.GetComponent<ISummonUnit>().summoner != null) {
            applyunit.overlapDamage += (int) Damage;
            SettingMobDamageMeter.maxDamage += (int) Damage;
        }
        
        if (shild >= Damage) shild -= Damage;
        else if (shild < Damage)
        {
            hp -= (Damage - shild);
            shild = 0;
        }
    }

}
