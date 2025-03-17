using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleUser
{
    public class SettingBattleList : MonoBehaviour
    {
        [SerializeField] GameObject battleList;
        [SerializeField] Transform spawnPos;
        [SerializeField] Filtering filtering;
        
        public enum Tier {
            None , Bronze , Silver , Gold , Pletinum , Diamond
        }
        Queue<BattleUserData> battleUserQueue = new Queue<BattleUserData>();

        void Start()
        {
            GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(CreateBattleList));
        }
        
        void OnEnable()
        {
            filtering.Open();
        }

        void CreateBattleList()
        {
            BattleDatas userData = GameDataManger.Instance.GetBattleData();
            for(int i = 0; i < 3; i++) {
                BattleList list = Instantiate(battleList, spawnPos).GetComponent<BattleList>();

                if(userData.battleUserDatas.Count > i){
                    List<MobData> mobData = userData.battleUserDatas[i].mobList;
                    for(int j = 0; j < mobData.Count; j++) {
                        list.Setting(i , mobData[j]);
                    }
                }
            }
        }
    }
}

