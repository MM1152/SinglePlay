
using System.Collections;
using UnityEngine;

public class ShortRangeScipt : Unit , ISummonUnit
{
    GameObject attackprefeb;
    ShortRangeAttack shortRangeAttack;
        
    public Summoner summoner { get ; set ; }
    public int damageMeter { get ; set ; }
    
    protected bool attackPattenChange;  // 어택이 기본 ShortRangeScript의 어택방식을 따르지 않을때 선언

    protected void OnEnable()
    {
        Respawn();
        if(summoner == null)  Spawn(GameManager.Instance.currentStage);
        else SummonerSpawn(summoner);
    }
    
    protected override void Awake() {
        base.Awake();

        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb, transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        if (!isDie)
        {
            Attack();
            base.Update();
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (isAttack && target != null && !attackPattenChange)
        {
            shortRangeAttack.target = target.transform.position;
            StartCoroutine(ShowAttack());
        }
    }
    IEnumerator ShowAttack(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= attackObjectShowTime);

        shortRangeAttack.gameObject.SetActive(true);
    }    
}
