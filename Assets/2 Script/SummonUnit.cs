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
    private void Awake() {
        ani = GetComponent<Animator>();
        summonUnitViewer = GameObject.FindObjectOfType<CreateSummonUnitViewer>();
    }
    public void Setting(GameObject spawnEnemy , Vector2 spawnPos , Transform parent , Summoner summoner){
        this.spawnEnemy = spawnEnemy;
        this.spawnPos = spawnPos;
        this.parent = parent;
        this.summoner = summoner;
        StartCoroutine(WaitForAnimation());
    }
    IEnumerator WaitForAnimation(){
        GameObject unit = Instantiate(spawnEnemy , parent);
        unit.gameObject.AddComponent<Summon>();
        unit.SetActive(false);
        
        unit.transform.position = spawnPos;
        unit.transform.SetParent(parent);
        unit.tag = this.tag;
        unit.GetComponent<ISummonUnit>().summoner = summoner; 
        
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        
        unit.SetActive(true);
        summoner.changeStatus += unit.GetComponent<Unit>().ChangeStats;
        summonUnitViewer.CreateViewer(unit.GetComponent<Unit>());
        gameObject.SetActive(false);
    }

}
