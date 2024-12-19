
using UnityEngine;

public class ShortRangeScipt : Unit
{
    //\\TODO MUSHROOM 스크립스 생성후 머시룸에 붙여줘서 사용하도록 , ISummonUnit 상속받아 소환가능한 객체로 만들기
    GameObject attackprefeb;
    ShortRangeAttack shortRangeAttack;
    protected bool attackPattenChange;

    protected void OnEnable()
    {
        Respawn();
    }
    
    protected override void Awake() {
        base.Awake();
        attackPattenChange = false;
        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb, transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
    }
    protected override void Update()
    {
        if (!isDie)
        {
            base.Update();
            Attack();
        }
    }

    protected override void Attack()
    {
        base.Attack();

        if (isAttack && target != null && !attackPattenChange)
        {
            shortRangeAttack.target = target.transform.position;
            shortRangeAttack.gameObject.SetActive(true);
        }
    }
    
}
