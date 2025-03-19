using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleUser
{
    public class SettingBattleList : MonoBehaviour
    {
        [SerializeField] GameObject battleList;
        [SerializeField] Transform spawnPos;
        [SerializeField] Filtering filtering;
        [SerializeField] Text playAbleCount;
        [SerializeField] Sprite[] tierImages;

        int tierScore;
        int palyCount;
        Dictionary<ItemClass , List<UnitData>> unitDatas = new Dictionary<ItemClass, List<UnitData>>();

        public enum Tier {
            UnRank , Bronze , Silver , Gold , Pletinum , Diamond
        }


        void Start()
        {
            GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(CreateBattleList));
            foreach(string soul in GameManager.Instance.allSoulInfo.Keys) {
                
                ItemClass itemClass = GameManager.Instance.allSoulInfo[soul].type;
                UnitData data = GameManager.Instance.allSoulInfo[soul];

                if(!unitDatas.ContainsKey(itemClass)) unitDatas.Add(itemClass , new List<UnitData>());
                unitDatas[itemClass].Add(data);
                
            }
        }
        
        void OnEnable()
        {
            filtering.Open();
        }

        void CreateBattleList()
        {
            BattleDatas userData = GameDataManger.Instance.GetBattleData();
            int limit = 0;
            List<int> pickCount = new List<int>();
            for(int i = 0; i < 3; i++) {
                BattleList list = Instantiate(battleList, spawnPos).GetComponent<BattleList>();
                
                if(userData.battleUserDatas.Count - 1 > i && limit < 5){
                    int userRandomIndex = UnityEngine.Random.Range(0 , userData.battleUserDatas.Count);

                    while((pickCount.Contains(userRandomIndex) || GameDataManger.Instance.battlUserIndex == userRandomIndex) && limit < 5) {
                        userRandomIndex = UnityEngine.Random.Range(0 , userData.battleUserDatas.Count);    
                        limit++;
                    }
                   
                    pickCount.Add(userRandomIndex);
                    list.Setting(userData.battleUserDatas[userRandomIndex]);
                }
                else {
                    list.Setting(CreateDummyData());
                }

            }
        }

        BattleUserData CreateDummyData(){
            int score = GameDataManger.Instance.GetBattleData().battleUserDatas[GameDataManger.Instance.battlUserIndex].battleScore;
            BattleUserData dummyData = new BattleUserData();
            if(score >= 0 || score < 500) {
                RandomPick(ItemClass.COMMON , 3 , dummyData);
                return dummyData;
            }

            return null;
        }

        void RandomPick(ItemClass itemClass , int count , BattleUserData dummyData){
            List<int> pickednumber = new List<int>();
            for(int i = 0 ; i < count; i++) {
                if(pickednumber.Count >= unitDatas[itemClass].Count) break;

                int rand = UnityEngine.Random.Range(0 , unitDatas[itemClass].Count);

                while(pickednumber.Contains(rand)) {
                    rand = UnityEngine.Random.Range(0 , unitDatas[itemClass].Count);
                }

                dummyData.mobList.Add(new MobData(unitDatas[itemClass][rand].name , unitDatas[itemClass][rand].typenumber - 1, UnityEngine.Random.Range(1, 5)));
                dummyData.userName = "dummy";
                pickednumber.Add(rand);
            }

            
        }
    }
}

