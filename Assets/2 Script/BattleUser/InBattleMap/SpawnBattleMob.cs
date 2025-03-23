using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBattleMob : MonoBehaviour
{
    [SerializeField] SummonUnit summonUnit;

    [SerializeField] CameraMove cameraMove;
    [SerializeField] CheckBattleResult checkBattleResult;

    [SerializeField] Transform playerSpawn;
    [SerializeField] Transform playerSpawnPos;

    [SerializeField] Transform otherPlayerSpawn;
    [SerializeField] Transform otherPlayerSpawnPos;
    
    

    Transform cameraTarget;
    void Start()
    {
        cameraMove = FindObjectOfType<CameraMove>();
        checkBattleResult = GetComponent<CheckBattleResult>();
        StartCoroutine(SpawnEffect());

    }

    public Transform GetNewTarget(){
        for(int i = 0; i < playerSpawn.transform.childCount; i++){
            if(playerSpawn.transform.GetChild(0).GetComponent<Unit>().isDie) continue;
            return playerSpawn.transform.GetChild(0);
        }
        return null;
    }

    IEnumerator SpawnEffect(){
        int count = 0;

        foreach(string key in GameManager.Instance.battlesInfo){
            SummonUnit spawn = Instantiate(summonUnit , playerSpawn).GetComponent<SummonUnit>();

            spawn.tag = "BattlePlayer";
            spawn.transform.position =  (Vector2) playerSpawnPos.position + (Vector2.up * count * -1.5f);
            int unitLevel = GameDataManger.Instance.GetGameData().soulsLevel[GameManager.Instance.allSoulInfo[key].typenumber - 1];
            spawn.Setting(GameManager.Instance.allSoulInfo[key].SummonPrefeb , (Vector2) playerSpawnPos.position + (Vector2.up * count * -1.5f) , playerSpawn , unitLevel);
            count++;
            yield return new WaitForSeconds(1f);
        }

        BattleUserData battleUserData = GameManager.Instance.otherBattleUserData;
        count = 0;
        
        for(int i = 0 ; i < battleUserData.mobList.Count; i++) {
            SummonUnit spawn = Instantiate(summonUnit , otherPlayerSpawn).GetComponent<SummonUnit>();

            spawn.tag = "BattleEnemy";
            spawn.transform.position =  (Vector2) otherPlayerSpawnPos.position + (Vector2.up * count * -1.5f);

            Vector2 spawnPos = (Vector2) otherPlayerSpawnPos.position + (Vector2.up * count * -1.5f) ;
            spawn.Setting(GameManager.Instance.allSoulInfo[battleUserData.mobList[i].name].SummonPrefeb , spawnPos , otherPlayerSpawn , battleUserData.mobList[i].level);
            count++;
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(0.5f);

        cameraMove.Setting(GetNewTarget());
        cameraMove.SettingAction(() => GetNewTarget());

        checkBattleResult.isStart = true;
    } 
}
