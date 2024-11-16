using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongRangeScript : Unit, IDamageAble
{
    int spawnProjecTileCount = 0;
    int attackCount = 0;
    [SerializeField] GameObject projectile;
    [Range(0f, 1f)] public float attackObjectShowTime;
    private void OnEnable()
    {
        Respawn();
        Init(GameManager.Instance.gameLevel);
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
        if (canAttack)
        {
            StartCoroutine(WaitForAttackAnimation());
        }
    }
    // 설정된 attackObjectShowTime 시간에 맞춰 원거리투사체 발사
    IEnumerator WaitForAttackAnimation()
    {
        if(ani != null)yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= attackObjectShowTime);
        else yield return null;

        GameObject attackObj = PoolingManager.Instance.ShowObject(projectile.name+"(Clone)", projectile);
        ProjecTile projecTile = attackObj.GetComponent<ProjecTile>();

        projecTile.tag = gameObject.tag;
        projecTile.target = target.transform;
        projecTile.unitData = this;
        attackObj.transform.position = this.gameObject.transform.position;

        projecTile.SetDirecetion();
        attackObj.SetActive(true);
    }
}
