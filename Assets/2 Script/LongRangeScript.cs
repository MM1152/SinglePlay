using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeScript : Unit, IDamageAble
{
    private void Awake()
    {
        base.Init();
    }
    private void Update()
    {
        base.KeepChcek();
        Attack();
        FollowTarget();
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }

    protected void Attack()
    {
        bool isAttack = !DontAttack && target != null && unit.attackRadious > Vector2.Distance(target.transform.position, transform.position) && currentAttackSpeed <= 0;

        if (isAttack)
        {
            
            currentAttackSpeed = setInitAttackSpeed;

            GameObject attackObj = PoolingManager.Instance.ShowObject("Projectile");
            ProjecTile projecTile = attackObj.GetComponent<ProjecTile>();

            projecTile.tag = gameObject.tag;
            projecTile.target = target.transform;
            projecTile.unitData = unit;
            attackObj.transform.position = this.gameObject.transform.position;

            projecTile.SetDirecetion();
            attackObj.SetActive(true);
            
        }
    }
}
