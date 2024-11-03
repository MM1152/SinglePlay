using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ShortRangeScipt : Unit, IDamageAble
{
    GameObject attackprefeb;
    [SerializeField] ShortRangeAttack shortRangeAttack;


    private void OnEnable()
    {
        Respawn();
        Init(GameManager.Instance.gameLevel * 0.1f);
    }

    protected override void Init(float setStatus)
    {
        base.Init(setStatus);
        isDie = false;
        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb, transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
    }
    protected override void Init(Summoner summoner , float precent)
    {
        base.Init(summoner ,precent);
        isDie = false;
        attackprefeb = Resources.Load<GameObject>("Attack");
        shortRangeAttack = Instantiate(attackprefeb, transform).GetComponent<ShortRangeAttack>();
        shortRangeAttack.unit = unit;
        shortRangeAttack.gameObject.SetActive(false);
    }
    
    protected override void KeepChcek()
    {
        base.KeepChcek();
        Attack();
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
    // Update is called once per frame
    private void Update()
    {
        if (!isDie)
        {
            KeepChcek();
        }
    }
    protected override void Attack()
    {
        base.Attack();

        if (isAttack && target != null)
        {
            shortRangeAttack.target = target.transform.position;
            shortRangeAttack.gameObject.SetActive(true);
        }
    }

}
