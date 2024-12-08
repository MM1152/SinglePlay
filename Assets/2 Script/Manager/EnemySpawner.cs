using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }
    
    
    [Header("참조")]
    [Space(30)]
    [Header("적 / 보스 / 리스폰 위치 설정")]
    [SerializeField] Unit[] Enemys;
    public Unit[] Boss;
    [SerializeField] Transform[] spawnPos;
    [Space(30)]
    [Header("적 리스폰 설정")]
    [SerializeField] float spawnTimer;
    [SerializeField] int maxEnemyNumber;
    [SerializeField] int currentEnemyNumber;

    bool isBossSpawn;

    float currentSpawnTimer;
    float avg;


    public List<float> spawnProbabillity = new List<float>();

    Exp exp;
    Transform spawntrans;
    MergeSort<Unit> sort;
    private void Awake()
    {
        currentSpawnTimer = spawnTimer;
        Instance = this;

        exp = GameObject.FindObjectOfType<Exp>();
        spawntrans = GameObject.Find("EnemyList").transform;

        Enemys = Resources.LoadAll<Unit>(GameManager.Instance.mapName + "Enemy");
        SettingUnitProbabillity();


        sort = new MergeSort<Unit>(Enemys);
        Enemys = sort.get();

        SetSpawnProbabillity();
    }

    private void Update()
    {
        if (GameManager.Instance.gameLevel % 10 != 0)
        {
            if (!GameManager.Instance.gameClear)
            {
                if (currentSpawnTimer <= 0)
                {
                    currentSpawnTimer = spawnTimer;
                    RespawnEnemy();
                }
                currentSpawnTimer -= Time.deltaTime;
            }
            else if (GameManager.Instance.gameClear)
            {
                currentEnemyNumber = 0;
                currentSpawnTimer = spawnTimer;
            }
        }
        else {
            if(!isBossSpawn){
                Debug.Log("Spawn Boss");
                Unit boss = Instantiate(Boss[GameManager.Instance.gameLevel / 10 - 1] , spawntrans);
                boss.transform.position = spawnPos[1].transform.position; 
                isBossSpawn = true;
            }
            
        }

    }

    void RespawnEnemy()
    {
        for (int i = currentEnemyNumber; i < maxEnemyNumber; i++)
        {
            currentEnemyNumber++;
            float spawnEnemy = Random.Range(0f, 1f);
            float sum = 0;
            for (int j = 0; j < spawnProbabillity.Count; j++)
            {
                GameObject Enemy;
                sum += spawnProbabillity[j];
                if (sum >= spawnEnemy)
                {
                    //:fix 적 캐릭터 생성위치 변경 필요 , 캐릭터가 맵 끝쪽에 있을때 맵안쪽에 소환할 수 있도록 변경

                    Enemy = PoolingManager.Instance.ShowObject(Enemys[j].gameObject.name + "(Clone)", Enemys[j].gameObject);
                    int spawnindex = Random.Range(0, 4);
                    Enemy.transform.position = spawnPos[spawnindex].position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                    Enemy.transform.SetParent(spawntrans);
                    Enemy.SetActive(true);
                    break;
                }

            }
        }

    }

    void SetSpawnProbabillity()
    {
        for (int i = 0; i < Enemys.Length; i++)
        {
            avg += Enemys[i].unit.spawnProbabillity;
        }

        for (int i = 0; i < Enemys.Length; i++)
        {
            spawnProbabillity.Add(Enemys[i].unit.spawnProbabillity / avg);
        }
    }

    public void CheckDie()
    {

        exp.SetExpValue(10f);
        currentEnemyNumber--;
        GameManager.Instance.clearMonseter--;
        
    }

    private void SettingUnitProbabillity()
    {
        for (int i = 0; i < Enemys.Length; i++)
        {
            if (Enemys[i].spawnProbabillity == 0)
            {
                Enemys[i].spawnProbabillity = Enemys[i].unit.spawnProbabillity;
            }
        }
    }
}
