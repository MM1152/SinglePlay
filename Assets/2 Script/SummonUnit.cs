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
    bool spawn ;
    private void Awake() {
        ani = GetComponent<Animator>();
        summonUnitViewer = GameObject.FindObjectOfType<CreateSummonUnitViewer>();
        spawn = false;
    }
    public void Setting(GameObject spawnEnemy , Vector2 spawnPos , Transform parent , Summoner summoner){
        this.spawnEnemy = spawnEnemy;
        this.spawnPos = spawnPos;
        this.parent = parent;
        this.summoner = summoner;
        StartCoroutine(WaitForAnimation());
    }
    //1.첫번째 소환시 옆에 소환된 유닛창이 생성됨
    //2. 재생성시에 이미 소환된 유닛창을 찾아서 다시 연결시켜줘야함함
    IEnumerator WaitForAnimation(){
        GameObject unit = Instantiate(spawnEnemy , parent);
        unit.gameObject.AddComponent<Summon>();
        unit.SetActive(false);
        
        unit.transform.position = spawnPos;
        unit.transform.SetParent(parent);
        unit.tag = this.tag;
        unit.GetComponent<ISummonUnit>().summoner = summoner; 
        
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        summonUnitViewer.CreateViewer(unit.GetComponent<Unit>());
        unit.SetActive(true);
        summoner.changeStatus += unit.GetComponent<Unit>().ChangeStats;
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        
    }

}
