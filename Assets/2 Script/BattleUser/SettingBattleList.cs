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

        Dictionary<ItemClass , List<UnitData>> unitDatas = new Dictionary<ItemClass, List<UnitData>>();

        void Init() {
            BaseUI.instance.LoadUi("RankInfo" , (value) => {
                GameObject go = Instantiate(value , transform); 
                go.SetActive(false);

                UI_Event block = go.transform.Find("BlockTab").gameObject.GetOrAddComponent<UI_Event>();
                UI_Event rankButton = transform.Find("RankInfo").GetOrAddComponent<UI_Event>();
                UI_Event rank = transform.Find("Rank").GetOrAddComponent<UI_Event>();

                block.SetClickAction(() => ClickToCloseEvent(go.gameObject));
                rankButton.SetClickAction(() => ClickToOpenEvent(go.gameObject));
                
            });
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
            playAbleCount.text = GameDataManger.Instance.GetGameData().playAbleCount + "/" + 5;

            tier.Init(GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore);
            Init();
        }
        void OnEnable()
        {
            if(!GameManager.Instance.isPlayingTutorial) {
                filtering.Open();
            }
        }
        
        #region Event 
        void ClickToCloseEvent(GameObject go){
            go.SetActive(false);
        }

        void ClickToOpenEvent(GameObject go) {
            go.SetActive(true);
        }
        #endregion
        #region 배틀리스트 생성

        void LoadBattleList() {
            
            List<BattleUserData> battleUserData = GameDataManger.Instance.GetGameData().battleUserDatas;
            string json = JsonUtility.ToJson(GameDataManger.Instance.GetGameData());
            Debug.Log(json);
            for(int i = 0 ; i < battleUserData.Count; i ++) {
                BattleList list = Instantiate(battleList, spawnPos).GetComponent<BattleList>();
                list.Setting(battleUserData[i]);
            }
            
        } 

        void CreateBattleList()
        {
            if(GameDataManger.Instance.GetGameData().settingBattleUserData) {
                LoadBattleList();
                return;
            }

            BattleDatas userData = GameDataManger.Instance.GetBattleData();
            List<int> pickCount = new List<int>();
            BattleUserData battleUserData = new BattleUserData();
            
            for(int i = 0; i < 3; i++) {
                int limit = 0;
                BattleList list = Instantiate(battleList, spawnPos).GetComponent<BattleList>();
                
                if(userData.user.Count - 1 > i && limit < 5){
                    int userRandomIndex = UnityEngine.Random.Range(0 , userData.user.Count);

                    while((pickCount.Contains(userRandomIndex) 
                    || GameDataManger.Instance.battlUserIndex == userRandomIndex 
                    || userData.user[userRandomIndex].mobList.Count == 0) 
                    && limit < 5) {
                        userRandomIndex = UnityEngine.Random.Range(0 , userData.user.Count);    
                        limit++;
                    }

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
            if(score >= 0 || score < 500) {
                RandomPick(ItemClass.COMMON , 3 , dummyData);
            }

            GameDataManger.Instance.GetGameData().battleUserDatas.Add(dummyData);
            return dummyData;
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
        #endregion
    }
}

