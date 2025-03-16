using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BattleUser
{
    public class SettingBattleList : MonoBehaviour
    {
        [SerializeField] GameObject battleList;
        [SerializeField] GameObject BattleMob;
        [SerializeField] Transform spawnPos;
        [SerializeField] Filtering filtering;
        
        void Awake()
        {
            for (int i = 0; i < 3; i++)
            {
                CreateBattleList();
            }
        }
        void OnEnable()
        {
            filtering.Open();
        }
        void CreateBattleList()
        {
            Instantiate(battleList, spawnPos);
        }
    }
}

