using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{  

    [SerializeField] Unit[] Enemys;
    [SerializeField] float SpawnTimer;
    [SerializeField] int maxEnemyNumber;
    [SerializeField] int currentEnemyNumber;

    List<float> spawnProbabillity = new List<float>();
    float avg;
    MergeSort sort;
    private void Awake(){
        Enemys = Resources.LoadAll<Unit>("Enemys/");
        
        sort = new MergeSort(Enemys);
        Enemys = sort.get();

        SetSpawnProbabillity();
        RespawnEnemy();
    }
    void RespawnEnemy(){
        for(int i = currentEnemyNumber; i <= maxEnemyNumber; i++){
            float spawnEnemy = Random.Range(0f , 1f);
            float sum = 0;
            for(int j = 0; j < spawnProbabillity.Count; j++) {
                sum += spawnProbabillity[j];
                if(sum >= spawnEnemy) {
                    Debug.Log($"Spawn To {Enemys[j]} {spawnEnemy}");
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
}
