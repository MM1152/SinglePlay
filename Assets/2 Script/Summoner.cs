using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : LongRangeScript, IDamageAble
{
    [SerializeField] float skillCoolDown;
    [SerializeField] float skillCurrentTime;
    private void Awake()
    {
        base.Init(1);
        skillCoolDown = 2f;
        skillCurrentTime = skillCoolDown;
    }
    private void Update()
    {
        if (!isDie)
        {
            base.KeepChcek();
            if (!VirtualJoyStick.instance.isInput) FollowTarget();
        }
        if (SkillManager.Instance.SummonSkill) SummonSkill();
        
    }

    public void Move(Vector3 movePos)
    {
        transform.position += movePos * Time.deltaTime * unit.speed;
    }

    public void SummonSkill()
    {
        if (skillCurrentTime <= 0 && ISummonUnit.unitCount < 2)
        {
            GameObject unit = SkillManager.Instance.GetSummonGameObjet();
            unit = PoolingManager.Instance.ShowObject(unit.name+"(Clone)", unit);
            unit.transform.SetParent(transform.parent);
            unit.GetComponent<ISummonUnit>().summoner = this;
            unit.tag = gameObject.tag;
            unit.transform.position = transform.position + Vector3.right;
            
            skillCurrentTime = skillCoolDown;
        }
        else
        {
            skillCurrentTime -= Time.deltaTime;
        }
    }
}
