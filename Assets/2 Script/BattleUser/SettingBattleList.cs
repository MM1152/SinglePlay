using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingBattleList : MonoBehaviour
{
    [SerializeField] GameObject battleList;
    [SerializeField] GameObject BattleMob;
    [SerializeField] Transform spawnPos;
    // Start is called before the first frame update
    void Awake()
    {
        for(int i = 0; i < 3; i++) {
            CreateBattleList();
        }
    }

    void CreateBattleList(){
        Instantiate(battleList , spawnPos);
    }

}
