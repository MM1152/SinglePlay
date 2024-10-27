using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShortRangeScipt : Unit , IDamageAble
{
    GameObject attackprefeb;
    ShortRangeAttack shortRangeAttack;
    

    public void OnEnable(){
        isDie = false;
    }
    public void Awake() {
        base.Init();
        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb , transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
    // Update is called once per frame
    protected void Update()
    {
        FollowTarget();
        base.KeepChcek();
        Attack();
    }

    protected void Attack(){

        bool isAttack = !DontAttack && target != null && unit.attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;

        if(isAttack) {
            ani.SetTrigger("Attack");
            currentAttackSpeed = setInitAttackSpeed;
            
            shortRangeAttack.target = target.transform.position;
            shortRangeAttack.gameObject.SetActive(true);
        }
    }
}
