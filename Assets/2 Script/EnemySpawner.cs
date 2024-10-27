using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{  
    public static EnemySpawner Instance { get; private set;}

    [SerializeField] Unit[] Enemys;
    [SerializeField] float spawnTimer;
    float currentSpawnTimer;
    [SerializeField] int maxEnemyNumber;
    [SerializeField] int currentEnemyNumber;

    List<float> spawnProbabillity = new List<float>();

    private Transform spawntrans;
    float avg;
    Summoner player;
    MergeSort sort;
    private void Awake(){
        Instance = this;

        Enemys = Resources.LoadAll<Unit>("Enemys/");
        player = GameObject.FindObjectOfType<Summoner>();
        sort = new MergeSort(Enemys);
        Enemys = sort.get();
        spawntrans = GameObject.Find("EnemyList").transform;
        SetSpawnProbabillity();
    }
    
    private void Update(){
        if(currentSpawnTimer <= 0) {
            currentSpawnTimer = spawnTimer;
            RespawnEnemy();
        }
        currentSpawnTimer -= Time.deltaTime;
    }

    void RespawnEnemy(){
        for(int i = currentEnemyNumber; i < maxEnemyNumber; i++){
            currentEnemyNumber++;
            float spawnEnemy = Random.Range(0f , 1f);
            float sum = 0;
            for(int j = 0; j < spawnProbabillity.Count; j++) {
                GameObject Enemy;
                sum += spawnProbabillity[j];
                if(sum >= spawnEnemy) {
                    Enemy = PoolingManager.Instance.ShowObject(Enemys[j].gameObject.name +"(Clone)" , Enemys[j].gameObject);
                    Enemy.transform.position = player.transform.position + new Vector3(Random.Range(-5f , 5f) , Random.Range(-5f , 5f));
                    Enemy.transform.SetParent(spawntrans);
                    Enemy.SetActive(true);  
                    break;
                }
                
            }
        }
        
    }    

    void SetSpawnProbabillity(){
        for(int i = 0; i < Enemys.Length; i++) {
            avg += Enemys[i].unit.spawnProbabillity;
        }

        for(int i = 0; i < Enemys.Length; i++) {
            spawnProbabillity.Add(Enemys[i].unit.spawnProbabillity / avg);
        }
    }

    public void CheckDie(){
        currentEnemyNumber--;
    }
}
