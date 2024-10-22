using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeScipt : Unit , IDamageAble
{
    GameObject attackprefeb;
    ShortRangeAttack shortRangeAttack;

    public void Awake() {
        base.Awake();
        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb , transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
        Debug.Log($"hp {hp} , mp {mp} , damage {damage}");
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
    // Update is called once per frame
    protected void Update()
    {
        FollowTarget();
        base.Update();
        Attack();
    }

    protected void Attack(){
        bool isAttack = !DontAttack && target != null && unit.attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;

        if(isAttack) {
            currentAttackSpeed = setInitAttackSpeed;
            
            shortRangeAttack.target = target.transform.position;
            shortRangeAttack.gameObject.SetActive(true);
        }
    }
}
