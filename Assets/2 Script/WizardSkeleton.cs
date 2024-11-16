using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSkeleton : LongRangeScript , ISummonUnit

{
    public Summoner summoner { get ; set ; }

    private void OnEnable() {
        if (summoner) base.Init(summoner , 0.25f * SkillManager.Instance.skillDatas["해골 마법사 소환"]);
        ++ISummonUnit.unitCount;
        Respawn();
    }
    private void Start() {
        base.Init(summoner , 0.25f);
        targetList = GameObject.Find("EnemyList").gameObject;
    }

    private void OnDisable() {
        --ISummonUnit.unitCount;
    }
}
