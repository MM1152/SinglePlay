using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBattleMob : MonoBehaviour
{
    [SerializeField] SummonUnit summonUnit;
    [SerializeField] CameraMove cameraMove;
    

    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform playerSpawnPos;

    [SerializeField] Transform otherPlayerSpawn;
    [SerializeField] Transform otherPlayerSpawnPos;
    
    void Start()
    {
        cameraMove = FindObjectOfType<CameraMove>();
        StartCoroutine(SpawnEffect());
    }

    IEnumerator SpawnEffect(){
        int count = 0;

        foreach(string key in GameManager.Instance.battlesInfo.Keys){
            SummonUnit spawn = Instantiate(summonUnit , playerSpawn).GetComponent<SummonUnit>();

            cameraMove.Setting(spawn.transform);
            spawn.tag = "Player";
            spawn.transform.position =  (Vector2) playerSpawnPos.position + (Vector2.up * count * -1.5f);
            int unitLevel = GameDataManger.Instance.GetGameData().soulsLevel[GameManager.Instance.battlesInfo[key].typenumber - 1];
            spawn.Setting(GameManager.Instance.battlesInfo[key].SummonPrefeb , (Vector2) playerSpawnPos.position + (Vector2.up * count * -1.5f) , playerSpawn , unitLevel);
            count++;
            yield return new WaitForSeconds(1f);
        }
        //TODO : 적소환하는 로직 구현

        BattleUserData battleUserData = GameManager.Instance.otherBattleUserData;

        for(int i = 0 ; i < battleUserData.mobList.Count; i++) {
            SummonUnit spawn = Instantiate(summonUnit , otherPlayerSpawn).GetComponent<SummonUnit>();
            cameraMove.Setting(spawn.transform);
            spawn.tag = "Enemy";
            spawn.transform.position =  (Vector2) otherPlayerSpawnPos.position + (Vector2.up * count * -1.5f);

            Vector2 spawnPos = (Vector2) otherPlayerSpawnPos.position + (Vector2.up * count * -1.5f) ;
            spawn.Setting(GameManager.Instance.battlesInfo[battleUserData.mobList[i].name].SummonPrefeb , spawnPos , otherPlayerSpawn , battleUserData.mobList[i].level);
            count++;
            yield return new WaitForSeconds(1f);
        }
    } 
}
