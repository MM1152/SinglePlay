
using UnityEngine;

public class ShortRangeScipt : Unit, IDamageAble
{
    GameObject attackprefeb;
    ShortRangeAttack shortRangeAttack;
    protected bool attackPattenChange;

    private void OnEnable()
    {
        Respawn();
        Init(1f + (GameManager.Instance.gameLevel * 0.1f));
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
    
    public void Hit(float Damage)
    {
        hp -= Damage;
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
