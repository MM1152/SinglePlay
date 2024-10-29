using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit2 : LongRangeScript , ISummonUnit , IDamageAble

{
    public Summoner summoner { get ; set ; }

    private void OnEnable() {
        Respawn();
        
    }
    // Start is called before the first frame update
    void Awake()
    {
        ++ISummonUnit.unitCount;
        targetList = GameObject.Find("EnemyList").gameObject;
    }

    private void Start() {
        base.Init(summoner , 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        base.KeepChcek();
    }

    private void OnDisable() {
        --ISummonUnit.unitCount;
    }
}
