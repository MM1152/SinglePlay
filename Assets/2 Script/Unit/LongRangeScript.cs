using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LongRangeScript : Unit , ISummonUnit
{
    public GameObject projectile;
    public Summoner summoner { get; set; }
    public int damageMeter { get ; set ; }
   
    protected void OnEnable()
    {
        Respawn();
        if(summoner == null) Spawn(GameManager.Instance.currentStage * GameManager.Instance.mapindex + 1);
        else SummonerSpawn(summoner);
    }
    private void Update()
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
        if (canAttack)
        {
            StartCoroutine(WaitForAttackAnimation(target));
        }
    }
    // 설정된 attackObjectShowTime 시간에 맞춰 원거리투사체 발사
    public IEnumerator WaitForAttackAnimation(GameObject target)
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
