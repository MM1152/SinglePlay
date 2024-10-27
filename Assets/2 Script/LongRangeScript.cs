using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeScript : Unit, IDamageAble
{
    int spawnProjecTileCount = 0;
    private void OnEnable() {
        Respawn();
    }
    private void Awake()
    {
        Init(GameManager.Instance.gmaeLevel);
    }
    private void Update()
    {
        if (!isDie)
        {
            KeepChcek();
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
            GameObject attackObj = PoolingManager.Instance.ShowObject("Projectile");
            ProjecTile projecTile = attackObj.GetComponent<ProjecTile>();

            projecTile.tag = gameObject.tag;
            projecTile.target = target.transform;
            projecTile.unitData = unit;
            attackObj.transform.position = this.gameObject.transform.position;

            projecTile.SetDirecetion();
            attackObj.SetActive(true);
            if(gameObject.name == "Summon1(Clone)") {
            Debug.Log($"Attack {gameObject.name}");
        }
        }
        
        
    }
    protected override void Init(float setStatus) {
        base.Init(setStatus);
    }
    protected override void KeepChcek()
    {
        base.KeepChcek();
        Attack();
    }
}
