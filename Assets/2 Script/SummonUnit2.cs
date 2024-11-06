using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit2 : LongRangeScript , ISummonUnit

{
    public Summoner summoner { get ; set ; }

    private void OnEnable() {
        if (summoner) base.Init(summoner , 0.25f * SkillManager.Instance.skillDatas["해골 마법사 소환"]);
        Respawn();
    }
    // Start is called before the first frame update
    void Awake()
    {
        ++ISummonUnit.unitCount;
        targetList = GameObject.Find("EnemyList").gameObject;
    }

    private void Start() {
        base.Init(summoner , 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDie) {
            Attack();
        }
    }

    private void OnDisable() {
        --ISummonUnit.unitCount;
    }
}
