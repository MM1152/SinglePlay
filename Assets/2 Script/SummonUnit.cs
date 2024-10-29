using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit : ShortRangeScipt , ISummonUnit , IDamageAble
{
    
    public Summoner summoner { get ; set ; }
    public int count;
    private void OnEnable() {
        count = ++ISummonUnit.unitCount;
        Respawn();
    }

    private void Awake() { // 소환물은 프리팹이라 인스펙터창에서 드래그로 적용을 못시켜줘서 targetList 설정필요
        targetList = GameObject.Find("EnemyList").gameObject;
    }
    private void Start() {
        base.Init(summoner , 0.2f);
    }

    private void Update()
    {
        if(!isDie) {
            base.KeepChcek();
        }
    }

    private void OnDisable() {
        count = --ISummonUnit.unitCount;
    }

}
