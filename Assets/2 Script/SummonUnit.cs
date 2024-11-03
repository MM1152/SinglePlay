using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit : ShortRangeScipt , ISummonUnit , IDamageAble
{
    //\\TODO 타켓이 NULL 일때 플레이어 옆에 있도록 만들어야됌
    //\\TODO 이걸 SummonUnit을 상속받은 다른 스크립트를 여러개만들까.... 고민되네
    public Summoner summoner { get ; set ; }
    private void OnEnable() {
        ++ISummonUnit.unitCount;
        if(summoner != null) ChangeStats(summoner , 0.2f * SkillManager.Instance.skillDatas["해골 소환"]);
        Respawn();
    }   

    private void Awake() { // 소환물은 프리팹이라 인스펙터창에서 드래그로 적용을 못시켜줘서 targetList 설정필요
        targetList = GameObject.Find("EnemyList").gameObject;
       
    }
    private void Start() {
        base.Init(summoner , 0.2f);
           summoner.function += SpawnMapSummonUnit;
    }

    private void Update()
    {
        if(!isDie) {
            base.KeepChcek();
        }
    }

    private void OnDisable() {
        --ISummonUnit.unitCount;
    }

    void SpawnMapSummonUnit(){
        transform.position = summoner.transform.position + (Vector3)Random.insideUnitCircle;
    }
}
