using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit : MonoBehaviour
{
    //1. 게임 시작시 GameManager에 소울이 장착된 캐릭의 데이터를 가져옴
    //2. 가져온 데이터를 기반으로 소환, 즉, soulinfo 안에 소환될 캐릭의 프리팹이 존재해야 할듯
    GameObject spawnEnemy;
    Vector2 spawnPos;
    Transform parent;
    Animator ani;
    Summoner summoner;
    CreateSummonUnitViewer summonUnitViewer;
    CreateDamageMeter damageMeter;   

    float time;
    bool spawn ;
    int level;
    private void Awake() {
        ani = GetComponent<Animator>();
        summonUnitViewer = FindObjectOfType<CreateSummonUnitViewer>();
        damageMeter = FindObjectOfType<CreateDamageMeter>();
        spawn = false;
    }
    void OnEnable()
    {
        time = 3f;
    }
    public void Setting(GameObject spawnEnemy , Vector2 spawnPos , Transform parent , Summoner summoner){
        this.spawnEnemy = spawnEnemy;
        this.spawnPos = spawnPos;
        this.parent = parent;
        this.summoner = summoner;
        StartCoroutine(WaitForAnimation());
    }
    public void Setting(GameObject spawnEnemy , Vector2 spawnPos , Transform parent , int level){
        this.spawnEnemy = spawnEnemy;
        this.spawnPos = spawnPos;
        this.parent = parent;
        this.level = level;
        StartCoroutine(WaitForSpawnBattleMap());
    }
    private void Update() {
        time -= Time.deltaTime;
        if(time <= 0 && gameObject.activeSelf) {
            Debug.Log(gameObject.name);
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }
    //1.첫번째 소환시 옆에 소환된 유닛창이 생성됨
    //2. 재생성시에 이미 소환된 유닛창을 찾아서 다시 연결시켜줘야함함
    IEnumerator WaitForAnimation(){
        GameObject unit = Instantiate(spawnEnemy , parent);
        unit.gameObject.AddComponent<Summon>().Setting();
        unit.SetActive(false);
        
        unit.transform.position = spawnPos;
        unit.transform.SetParent(parent);
        unit.tag = this.tag;
        unit.GetComponent<ISummonUnit>().summoner = summoner; 
        
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        summonUnitViewer.CreateViewer(unit.GetComponent<Unit>());
        summoner.GetComponent<RePair>().summonUnit.Add(unit.GetComponent<Unit>());
        summoner.GetComponent<SummonUnitDodge>().summonUnit.Add(unit.GetComponent<Unit>());
        unit.SetActive(true);

        summoner.changeStatus += unit.GetComponent<Unit>().ChangeStats;

        if(!spawn) damageMeter.Init(unit.GetComponent<Unit>());
        else damageMeter.Redirect(unit.GetComponent<Unit>());
        
        
        spawn = true;

        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
    IEnumerator WaitForSpawnBattleMap(){
        GameObject unit = Instantiate(spawnEnemy , parent);
        unit.gameObject.AddComponent<Summon>().Setting(); //\\TODO : 레벨 집어넣기 
        unit.SetActive(false);
        
        unit.transform.position = spawnPos;
        unit.transform.SetParent(parent);
        unit.tag = this.tag;
        unit.GetComponent<Unit>().level = level;

        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        summonUnitViewer.CreateViewer(unit.GetComponent<Unit>());
        damageMeter.Init(unit.GetComponent<Unit>());
        unit.SetActive(true);
        Destroy(gameObject);
    }
}
