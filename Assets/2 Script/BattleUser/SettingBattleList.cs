using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        [SerializeField] Tier tier;

        [SerializeField] Button rankButton;
        [SerializeField] Button rankInfoButton;
        [SerializeField] Button AD_playCountButton; // 유저 전투 기능 횟수 추가 버튼
        [SerializeField] Button rerollBattleListButton;
        Dictionary<ItemClass , List<UnitData>> unitDatas = new Dictionary<ItemClass, List<UnitData>>();

        
        void Start()
        {
            GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(CreateBattleList));
            foreach(string soul in GameManager.Instance.allSoulInfo.Keys) {
                
                ItemClass itemClass = GameManager.Instance.allSoulInfo[soul].type;
                UnitData data = GameManager.Instance.allSoulInfo[soul];

                if(!unitDatas.ContainsKey(itemClass)) unitDatas.Add(itemClass , new List<UnitData>());
                unitDatas[itemClass].Add(data);
                
            }
            playAbleCount.text = GameDataManger.Instance.GetGameData().playAbleCount + "/" + 5;

            tier.Init(GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore);

            AD_playCountButton.transform.GetComponentInChildren<Text>().text = GameDataManger.Instance.GetGameData().Ad_playAbleCount + "/" + 3;
            AD_playCountButton.onClick.AddListener(() =>{ 
                if(GameDataManger.Instance.GetGameData().playAbleCount > 5 && GameDataManger.Instance.GetGameData().Ad_playAbleCount > 0) {
                    GoogleAdMobs.instance.ShowRewardedAd(() => {
                        GameDataManger.Instance.GetGameData().playAbleCount++;
                        GameDataManger.Instance.GetGameData().Ad_playAbleCount--;
                        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
                        playAbleCount.text = GameDataManger.Instance.GetGameData().playAbleCount + "/" + 5;
                    });
                }
            });

            rerollBattleListButton.transform.GetComponentInChildren<Text>().text = GameDataManger.Instance.GetGameData().userBattleListReRoll + "/" + 3;
            rerollBattleListButton.onClick.AddListener(() => {
                if(GameDataManger.Instance.GetGameData().userBattleListReRoll > 0) {
                    GameDataManger.Instance.GetGameData().userBattleListReRoll--;
                    GameDataManger.Instance.GetGameData().settingBattleUserData = false;
                    GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);

                    CreateBattleList();
                }
            });
        }
        void OnEnable()
        {
            if(!GameManager.Instance.isPlayingTutorial) {
                filtering.Open();
            }
        }
        

        #region 배틀리스트 생성

        void LoadBattleList() {
            
            List<BattleUserData> battleUserData = GameDataManger.Instance.GetGameData().battleUserDatas;
            string json = JsonUtility.ToJson(GameDataManger.Instance.GetGameData());
            Debug.Log(json);
            for(int i = 0 ; i < battleUserData.Count; i ++) {
                BattleList list = Instantiate(BaseUI.instance.SynchLoadUi("BattleList"), spawnPos).GetComponent<BattleList>();
                list.Init(true);
                list.Setting(battleUserData[i]);
            }
            
        } 

        void CreateBattleList()
        {
            if(GameDataManger.Instance.GetGameData().settingBattleUserData) {
                LoadBattleList();
                return;
            }

            foreach(Transform child in spawnPos) {
                Destroy(child.gameObject);
            }
            
            BattleDatas userData = GameDataManger.Instance.GetBattleData();
            List<int> pickCount = new List<int>();
            GameDataManger.Instance.GetGameData().battleUserDatas = new List<BattleUserData>();
            
            for(int i = 0; i < 3; i++) {
                int limit = 0;
                BattleList list = Instantiate(BaseUI.instance.SynchLoadUi("BattleList"), spawnPos).GetComponent<BattleList>();
                list.Init(true , CreateBattleList);
                if(userData.user.Count - 1 > i && limit < 5){
                    int userRandomIndex = UnityEngine.Random.Range(0 , userData.user.Count);
                    
                    while((pickCount.Contains(userRandomIndex) 
                    || GameDataManger.Instance.battlUserIndex == userRandomIndex 
                    || userData.user[userRandomIndex].mobList.Count == 0
                    || math.abs(userData.user[userRandomIndex].battleScore - userData.user[GameDataManger.Instance.battlUserIndex].battleScore) >= 100) 
                    && limit < 5) {
                        userRandomIndex = UnityEngine.Random.Range(0 , userData.user.Count);    
                        limit++;
                    }
                    Debug.Log(userRandomIndex);
                    if(limit >= 5) {
                        list.Setting(CreateDummyData());
                        continue;
                    }

                    pickCount.Add(userRandomIndex);
                    list.Setting(userData.user[userRandomIndex]);
                    GameDataManger.Instance.GetGameData().battleUserDatas.Add(userData.user[userRandomIndex]);
                }
                else {
                    list.Setting(CreateDummyData());
                }
            }
            GameDataManger.Instance.GetGameData().settingBattleUserData = true;
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
        }
        
        BattleUserData CreateDummyData(){
            int score = GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore;
            BattleUserData dummyData = new BattleUserData();

            if(score >= 0 && score < 100) {
                RandomPick(ItemClass.COMMON , 3 , dummyData , 1 , 5);
            }
            else if(score >= 100 && score < 300) {
                RandomPick(ItemClass.COMMON , 3 , dummyData , 8 , 14);
                RandomPick(ItemClass.UNCOMMON , 2 , dummyData , 5 , 11);
            }
            else if(score >= 300 && score < 500) {
                RandomPick(ItemClass.UNCOMMON , 2 , dummyData , 9 , 14);
                RandomPick(ItemClass.RARE , 2 , dummyData , 5 , 10);
                RandomPick(ItemClass.LEGENDARY , 1 , dummyData , 2 , 6);
            }
            

            GameDataManger.Instance.GetGameData().battleUserDatas.Add(dummyData);
            return dummyData;
        }

        void RandomPick(ItemClass itemClass , int count , BattleUserData dummyData , int mobLevelMin , int mobLevelMax){
            List<int> pickednumber = new List<int>();
            for(int i = 0 ; i < count; i++) {
                if(pickednumber.Count >= unitDatas[itemClass].Count) break;

                int rand = UnityEngine.Random.Range(0 , unitDatas[itemClass].Count);

                while(pickednumber.Contains(rand)) {
                    rand = UnityEngine.Random.Range(0 , unitDatas[itemClass].Count);
                }

                dummyData.mobList.Add(new MobData(unitDatas[itemClass][rand].name , unitDatas[itemClass][rand].typenumber - 1, UnityEngine.Random.Range(mobLevelMin, mobLevelMax)));
                dummyData.userName = "dummy";
                dummyData.battleScore = UnityEngine.Random.Range(GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore - 100
                 , GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore + 100);
                pickednumber.Add(rand);
            }
        }
        #endregion
    }
}

