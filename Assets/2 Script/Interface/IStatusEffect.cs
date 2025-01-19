using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffect
{
    Dictionary<string, IStatusEffect> statusEffect = new Dictionary<string, IStatusEffect>();
    public Unit unit;

    public StatusEffect(Unit unit)
    {
        this.unit = unit;
    }

    public void SetStatusEffect(IStatusEffect value)
    {

        foreach (string key in statusEffect.Keys)
        {
            if (key == value.ToString())
            {
                statusEffect[key].Init(unit);
                return;
            }
        }

        statusEffect.Add(value.ToString(), value);
        statusEffect[value.ToString()].Init(unit);

    }
    public void SetStatusEffect(IStatusEffect value , Unit applyUnit)
    {

        foreach (string key in statusEffect.Keys)
        {
            if (key == value.ToString())
            {
                statusEffect[key].Init(unit , applyUnit);
                return;
            }
        }

        statusEffect.Add(value.ToString(), value);
        statusEffect[value.ToString()].Init(unit , applyUnit);

    }
    public void Update()
    {
        foreach (string key in statusEffect.Keys)
        {
            if (statusEffect[key].overlapCount > 0) statusEffect[key]?.Run();
        }
    }

    public void Exit()
    {
        foreach (string key in statusEffect.Keys)
        {
            statusEffect[key]?.Exit();
        }
    }

    public bool GetStatusEffect(IStatusEffect value)
    {
        if (statusEffect.ContainsKey(value.ToString()) && statusEffect[value.ToString()].overlapCount != 0) return false;
        else return true;
    }
}
public class AttackPowerBuffEffect : IStatusEffect
{
    float duration = 20f;
    float upgradeDamage;
    GameObject attackBuffObj;
    public static Queue<GameObject> pools = new Queue<GameObject>();
    public int overlapCount { get; set; }
    public float currentDuration { get; set; }
    Unit unit;

    public void Exit()
    {
        unit.damage -= upgradeDamage;
        overlapCount = 0;
        attackBuffObj.SetActive(false);
        pools.Enqueue(attackBuffObj);
    }

    public void Init(Unit unit, Unit tauntUnit = null)
    {
        this.unit = unit;
        upgradeDamage = unit.damage * 0.3f;
        this.unit.damage += upgradeDamage;

        if (pools.Count != 0)
            {
                attackBuffObj = pools.Dequeue();
            }
            else
            {
                attackBuffObj = new GameObject("AttackBuffEffect" , typeof(SpriteRenderer));
                attackBuffObj.transform.localScale = new Vector3(3f, 3f , 3f);
                attackBuffObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>("UseSkillFolder/AttackBuff").GetComponent<SpriteRenderer>().sprite;
            }
        attackBuffObj.transform.position = Vector2.zero;
        attackBuffObj.SetActive(true);
        currentDuration = duration;
        overlapCount++;
    }

    public void Run()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration > 0)
        {
            attackBuffObj.transform.position = unit.transform.position + Vector3.up;
        }
        if(currentDuration <= 0) Exit();
    }
}
public class TauntEffect : IStatusEffect
{
    float duration = 5f;
    bool isExit;
    public static Queue<GameObject> pools = new Queue<GameObject>();
    public float currentDuration { get; set; }
    public int overlapCount { get; set; }
    GameObject tauntobj;
    Unit unit;
    Unit usetauntUnit;

    public void Init(Unit unit , Unit tauntUnit = null)
    {
        this.unit = unit;
        isExit = false;
        this.usetauntUnit = tauntUnit;
        if (overlapCount <= 0)
        {
            if (pools.Count != 0)
            {
                tauntobj = pools.Dequeue();
            }
            else
            {
                tauntobj = new GameObject();
                tauntobj.AddComponent<SpriteRenderer>();
                tauntobj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UseSkillFolder/TauntImage");
            }
            tauntobj.transform.position = Vector2.zero;
            tauntobj.SetActive(true);
        }

        overlapCount++;
        currentDuration = 10f;
        
    }

    public void Run()
    {
        currentDuration -= Time.deltaTime;
        if(usetauntUnit != null && usetauntUnit.isDie) Exit();
        if (currentDuration > 0)
        {
            tauntobj.transform.position = unit.transform.position;
        }
        else if (currentDuration <= 0)
        {
            Exit();
        }
    }

    public void Exit()
    {
        if (!isExit)
        {
            isExit = true;
            overlapCount = 0;
            pools.Enqueue(tauntobj);
            tauntobj.SetActive(false);
        }


    }

}


public interface IStatusEffect
{
    public int overlapCount { get; set; }
    public float currentDuration { get; set; }
    public void Init(Unit unit , Unit tauntUnit = null);
    public void Run();
    public void Exit();
}