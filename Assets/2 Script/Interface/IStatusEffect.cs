using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
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
                statusEffect[key].Init(unit , 0 , 0);
                return;
            }
        }

        statusEffect.Add(value.ToString(), value);
        statusEffect[value.ToString()].Init(unit , 0 , 0);

    }
    /// <param name="value">적용시킬 상태이상/버프</param>
    /// <param name="applyUnit">적용시킨 유닛</param>
    /// <param name="durationTime">지속시간 1초 단위</param>
    /// <param name="percent">적용시킬 퍼센트 0.1단위</param>
    public void SetStatusEffect(IStatusEffect value , Unit applyUnit , float durationTime , float percent)
    {

        foreach (string key in statusEffect.Keys)
        {
            if (key == value.ToString())
            {
                statusEffect[key].Init(unit , durationTime, percent , applyUnit);
                return;
            }
        }
        statusEffect.Add(value.ToString(), value);
        statusEffect[value.ToString()].Init(unit , durationTime , percent , applyUnit);
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
    //\\TODO : 전달하는 유닛의 스킬 퍼센트에 맞게 바꿔줘야함.
    float duration = 20f;
    float upgradeDamage;
    GameObject attackBuffObj;
    public static Queue<GameObject> pools = new Queue<GameObject>();
    public int overlapCount { get; set; }
    public float currentDuration { get; set; }
    Unit unit;


    public void Exit()
    {
        unit.buffAttack -= upgradeDamage;
        upgradeDamage = 0;
        overlapCount = 0;
        attackBuffObj.SetActive(false);
        pools.Enqueue(attackBuffObj);
        unit.ChangeStats();
    }

    public void Init(Unit unit, float settingDuration , float percent ,Unit tauntUnit = null)
    {
        if(duration == 0 && percent == 0) {
            currentDuration = duration;
            upgradeDamage += 0.3f;
        }
        else {
            currentDuration = settingDuration;
            upgradeDamage += percent;
        }
        Debug.Log("Setting CurrentDuration" + currentDuration);
        this.unit = unit;
        //\\TODO : 전달하는 유닛의 스킬 퍼센트에 맞게 바꿔줘야함.
        this.unit.buffAttack = upgradeDamage;
        unit.ChangeStats();
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
    Text overlapCountText;
    Unit unit;
    Unit usetauntUnit;

    public void Init(Unit unit ,float duration , float percent , Unit tauntUnit = null)
    {
        if(duration == 0) currentDuration = this.duration;
        else currentDuration = duration;

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
        
    }

    public void Run()
    {
        currentDuration -= Time.deltaTime;
        if(usetauntUnit != null && usetauntUnit.isDie) Exit();
        if (currentDuration > 0)
        {
            tauntobj.transform.position = unit.transform.position + Vector3.up;
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
public class BurnEffect : IStatusEffect
{
    public int overlapCount { get ; set ; }
    public float currentDuration { get ; set ; }
    GameObject burnEffect;

    float repeatDamageTime;
    float duration = 5f;
    float percent;
    Unit unit;
    Unit applyUnit;
    public void Exit()
    {
        overlapCount = 0;
        repeatDamageTime = 0f;
        burnEffect.SetActive(false);
    }

    public void Init(Unit unit,float duration , float percent, Unit tauntUnit = null)
    {
        if(duration == 0 && percent == 0) {
            currentDuration = this.duration;
            this.percent = 0.1f;
        }
        else {
            currentDuration = duration;
            this.percent = percent;
        }

        if(burnEffect == null) {
            burnEffect = new GameObject("Burn");
            burnEffect.transform.localScale = Vector3.one * 4f;
            burnEffect.AddComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>("UseSkillFolder/BurnEffect").GetComponent<SpriteRenderer>().sprite;
        }

        if(overlapCount <= 0) burnEffect.SetActive(true);

        this.unit = unit;
        applyUnit = tauntUnit;
                
        if(overlapCount < 5) overlapCount++;
    }

    public void Run()
    {
        repeatDamageTime += Time.deltaTime;
        currentDuration -= Time.deltaTime;
        burnEffect.transform.position = unit.transform.position + Vector3.up;
        if(repeatDamageTime >= 1f) {
            repeatDamageTime = 0f;
            RepeatDamage();
        }
        if(currentDuration <= 0) {
            Exit();
        }
    }
    void RepeatDamage(){
        //\\TODO : 화상데미지는 주황색으로 표시?
        unit.Hit(applyUnit.damage * (percent * overlapCount) , applyUnit , 0 , AttackType.Burn);
    }
}
public class SpeedBuffEffect : IStatusEffect
{
    public int overlapCount { get ; set ; }
    public float currentDuration { get ; set ; }
    float duration;
    float upgradePercent;
    Unit unit;
    GameObject speedBuffObject;

    public void Exit()
    {
        Debug.Log("SpeedUP Exit");
        unit.buffSpeed -= upgradePercent;
        upgradePercent = 0;
        speedBuffObject.SetActive(false);
        overlapCount = 0;
        unit.ChangeStats();
    }

    public void Init(Unit unit, float settingDuration , float percent,Unit tauntUnit = null)
    {
        if(duration == 0 && percent == 0) {
            currentDuration = duration;
            upgradePercent += 0.1f;
        }
        else {
            currentDuration = settingDuration;
            upgradePercent += percent;
        }

        if(speedBuffObject == null) {
            speedBuffObject = new GameObject("Speedup");
            speedBuffObject.transform.localScale = Vector3.one * 4f;
            speedBuffObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>("UseSkillFolder/SpeedUpEffect").GetComponent<SpriteRenderer>().sprite;
        }
        
        
        this.unit = unit;
        if(overlapCount <= 0) speedBuffObject.SetActive(true);

        unit.buffSpeed = upgradePercent;
        unit.ChangeStats();
        overlapCount++;
    }

    public void Run()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration > 0)
        {
            speedBuffObject.transform.position = unit.transform.position + Vector3.up;
        }
        if(currentDuration <= 0) Exit();
    }
}
public class ElectricEffect : IStatusEffect
{
    public int overlapCount { get ; set ; }
    public float currentDuration { get ; set ; }
    float duration = 7f;
    float slowValue;
    Unit unit;
    GameObject effectObject;
    public void Exit()
    {
        Debug.Log("Electric Exit");
        effectObject.SetActive(false);
        overlapCount = 0;
        unit.speed += slowValue;
    }

    public void Init(Unit unit, float duration , float percent, Unit tauntUnit = null)
    {
        if(duration == 0 && percent == 0) {
            currentDuration = this.duration;
            percent = 0.2f;
        }
        else {
            currentDuration = duration;
        }

        if(effectObject == null) {
            effectObject = new GameObject("Electric");
            effectObject.transform.localScale = Vector3.one * 4f;
            effectObject.AddComponent<SpriteRenderer>().sprite = Resources.Load<GameObject>("UseSkillFolder/Electric").GetComponent<SpriteRenderer>().sprite;
        }
        
        Debug.Log("Electric Init");
        this.unit = unit;
        slowValue = unit.speed * percent;
        if(overlapCount <= 0) effectObject.SetActive(true);

        unit.speed -= slowValue;
       
        overlapCount++;
    }

    public void Run()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration > 0)
        {
            effectObject.transform.position = unit.transform.position + Vector3.up;
        }
        if(currentDuration <= 0) Exit();
    }
}
public interface IStatusEffect
{
    public int overlapCount { get; set; }
    public float currentDuration { get; set; }
    public void Init(Unit unit , float duration , float percent , Unit tauntUnit = null);
    public void Run();
    public void Exit();
}