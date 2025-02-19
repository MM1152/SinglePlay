using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISummonUnit
{
    public Summoner summoner{get; set;}
    public int damageMeter{get; set;}
    public void DieSummonUnit(){
        Debug.Log(GetType().ToString());
        summoner.GetComponent<Resurrection>().DieUnit(GetType().ToString());
    }
    public void AddDamage(int value){
        damageMeter += value;
    }
}
