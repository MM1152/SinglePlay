
using UnityEngine;

public class SwordSkeleton : ShortRangeScipt , ISummonUnit
{
    public Summoner summoner { get ; set ; }
    
     
    private void OnEnable() {
        ++ISummonUnit.unitCount;
        if(summoner != null) ChangeStats(summoner , 0.2f * SkillManager.Instance.skillDatas["해골 소환"]);
        Respawn();
    }   
    private void Start() {
        Init(summoner , 0.2f);
        summoner.function += SpawnMapSummonUnit;
    }
    protected override void Awake() { // 소환물은 프리팹이라 인스펙터창에서 드래그로 적용을 못시켜줘서 targetList 설정필요
        base.Awake();
        targetList = GameObject.Find("EnemyList").gameObject;
    }
    

/*
    private void Update()
    {
        if(!isDie) {
            base.KeepChcek();
        }
    }
*/
    private void OnDisable() {
        --ISummonUnit.unitCount;
    }

    void SpawnMapSummonUnit(){
        transform.position = summoner.transform.position + (Vector3)Random.insideUnitCircle;
    }
}
