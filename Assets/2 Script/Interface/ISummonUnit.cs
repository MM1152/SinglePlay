using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISummonUnit
{
    public Summoner summoner{get; set;}
    public int damageMeter{get; set;}
    public void DieSummonUnit(Unit unit){
        Debug.Log(GetType().ToString());
        summoner.changeStatus -= unit.ChangeStats;
        summoner.GetComponent<Resurrection>().DieUnit(unit.unit.name);
    }
    public void AddDamage(int value){
        damageMeter += value;
    }
}
