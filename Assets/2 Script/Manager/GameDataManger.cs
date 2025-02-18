using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.ExceptionServices;
using UnityEngine.AI;

[Serializable] 
public class GameData{
    public int soul;
    public int gem;
    public List<bool> unLockMap = new List<bool>();

    /// <summary>
    /// 1: Attack , 2: Hp , 3: SummonUnitHp , 4: AttackSpeed  , 5: MoveSpeed , 6: BonusTalent , 7: BonusGoods , 8: IncreaesDamage ,
    /// 9: IncreaesHp, 10: CoolTime, 11: SkillDamage, 12 IncreasedExp, 13 Dodge, 14 DrainLife
    /// </summary>
    public List<int> reclicsLevel = new List<int>();
    public List<int> reclicsCount = new List<int>();
    public List<int> soulsLevel = new List<int>();
    public List<int> soulsCount = new List<int>();
    public List<int> soulsEquip = new List<int>();

    /// <summary>
    /// 0 : 재화로 변경 , 1 : 광고로 변경
    /// </summary>
    public List<int> shopListChangeCount = new List<int>(); 
    public List<string> sellingItemListType = new List<string>();
    public List<int> sellingItemListNum = new List<int>();
    public List<bool> sellingGem = new List<bool>();
    public List<bool> soldOutItem = new List<bool>();
    public bool settingShopList;
    public string dateTime;
    public List<bool> dailyGift = new List<bool>();
    public bool getGift;
    public List<int> openCount = new List<int>(); // RandomPick Up 횟수  0 : 유물, 1 : 소울 
    public List<bool> isBoxOpen = new List<bool>();
    public List<DailyQuestData> questData = new List<DailyQuestData>();
}

[Serializable]
public class DailyQuestData {
    public DailyQuestData(string type) {
        
        this.type = type;
        this.count = 0;
        this.isClear = false;   

        if(type == "Login") count = 1;
    }

    public void Setting(){
        this.count = 0;
        this.isClear = false;  

        if(type == "Login") count = 1;
    }
    public string type;
    public int count;
    public bool isClear;
}

public class GameDataManger : MonoBehaviour
{   
    public static GameDataManger Instance { get ; private set;}
    [SerializeField] GameData data = null;
    public Action<int ,int> goodsSetting;
    private string filePath;
    public bool dataDownLoad;
    
    private void Awake() {
        
        filePath  = Application.persistentDataPath + "/data.json";
        Debug.Log(filePath);
        
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
        
    }
    private void Start() {
        data = LoadData();
        DailyQuestTab.dailyQuestTab.Setting();
    }
    public GameData LoadData(){
        GameData LoadData = new GameData();

        //저장된 파일이 없을때 실행
        if(!File.Exists(filePath)) {
            LoadData.unLockMap.Add(true);

            for(int i = 0 ; i < SoulsManager.Instance.soulsInfos.Length; i++) {
                LoadData.soulsLevel.Add(0);
                LoadData.soulsCount.Add(0);
            }
            
            for(int i = 0; i < ReclicsManager.Instance.reclicsDatas.Length; i++) {
                LoadData.reclicsLevel.Add(0);
                LoadData.reclicsCount.Add(0);
            }

            for(int i = 0; i < 5; i++) {
                LoadData.soulsEquip.Add(0);
            }

            for(int i = 0 ; i < 8; i++) {
                LoadData.sellingItemListType.Add("");
                LoadData.sellingItemListNum.Add(-1);
                LoadData.sellingGem.Add(false);
                LoadData.soldOutItem.Add(false);
            }

            for(int i = 0; i < 28; i++) {
                LoadData.dailyGift.Add(false);
            }

            foreach(string type in Enum.GetNames(typeof(QuestType))) {
                LoadData.questData.Add(new DailyQuestData(type));
            }

            for(int i = 0; i < 3; i++) {
                LoadData.isBoxOpen.Add(false);
            }
            LoadData.settingShopList = false;

            LoadData.openCount.Add(0);
            LoadData.openCount.Add(1);

            LoadData.shopListChangeCount.Add(3);
            LoadData.shopListChangeCount.Add(2);

            LoadData.gem = 0;
            LoadData.soul = 0;

            LoadData.dateTime = GetTime.currentTime;
            LoadData.getGift = false;
            string JsonData = JsonUtility.ToJson(LoadData);

            File.WriteAllText(filePath , JsonData);
        }

        string data = File.ReadAllText(filePath);
        LoadData = JsonUtility.FromJson<GameData>(data);
        
        CheckChangeData(LoadData);
        CheckCompareDateTime(LoadData);
        if(SceneManager.GetActiveScene().buildIndex == 1) goodsSetting?.Invoke(LoadData.soul , LoadData.gem);
        dataDownLoad = true;
        return LoadData;
    }   
    //fix : 데이터 로드이후 게임 접근시 유물 slider maxValue 설정 오류
    public void SaveData(){
        string jsonData = JsonUtility.ToJson(data);

        if(File.Exists(filePath)) File.WriteAllText(filePath , jsonData);
        
        dataDownLoad = false;
        this.data = LoadData();
        Debug.Log(filePath);
    }
    public GameData GetGameData(){
        return data;
    }

    public static IEnumerator WaitForDownLoadData(Action GetGameData = null){
        yield return new WaitUntil(() => Instance.dataDownLoad);
        GetGameData?.Invoke();
    }
    //데이터 로드시 데이터의 갯수와 내가 저장한 데이터의 길이가 다를때 실행된다.
    //EX ) 데이터 추가 등
    void CheckChangeData(GameData LoadData){

        bool changeData = false;
        if(LoadData.dateTime == null) {
            LoadData.dateTime = GetTime.currentTime;
            this.data = LoadData;
            changeData = true;
        }

        if(SoulsManager.Instance.soulsInfos.Length != 0 && SoulsManager.Instance.soulsInfos.Length != LoadData.soulsCount.Count) {
            for(int i = 0 ; i <= SoulsManager.Instance.soulsInfos.Length - LoadData.soulsCount.Count; i++) {
                LoadData.soulsCount.Add(0);
                LoadData.soulsLevel.Add(0);
            }
            this.data = LoadData;
            changeData = true;
        }

        if(ReclicsManager.Instance.reclicsDatas.Length != 0 && ReclicsManager.Instance.reclicsDatas.Length != LoadData.reclicsCount.Count) {
            for(int i = 0 ; i <= ReclicsManager.Instance.reclicsDatas.Length - LoadData.reclicsCount.Count; i++){
                LoadData.reclicsCount.Add(0);
                LoadData.reclicsLevel.Add(0);
            }
            this.data = LoadData;
            changeData = true;
        }

        if(LoadData.soldOutItem.Count == 0) {
            for(int i = 0; i < 8; i++) {
                LoadData.soldOutItem.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }

        if(LoadData.dailyGift.Count == 0) {
            for(int i = 0; i < 28; i++) {
                LoadData.dailyGift.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }

        if(LoadData.openCount.Count == 0) {
            LoadData.openCount.Add(0);
            LoadData.openCount.Add(0);
            this.data = LoadData;
            changeData = true;
        }

        if(LoadData.questData.Count == 0) {
            foreach(string type in Enum.GetNames(typeof(QuestType))) {
                LoadData.questData.Add(new DailyQuestData(type));
            }
            this.data = LoadData;
            changeData = true;
        }

        if(LoadData.isBoxOpen.Count == 0) {
            for(int i = 0; i < 3; i++) {
                LoadData.isBoxOpen.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }
        if(changeData) SaveData();

    }
    int CheckCompareDateTime(GameData LoadData){
        DateTime pastDateTime = Convert.ToDateTime(LoadData.dateTime);
        DateTime currentDateTime = Convert.ToDateTime(GetTime.currentTime);
        int isPast = DateTime.Compare(pastDateTime , currentDateTime);
        if(isPast < 0) {
            if(pastDateTime.Month < currentDateTime.Month) {
                for(int i = 0; i < 28; i++) {
                    LoadData.dailyGift[i] = false;
                }
            }
            LoadData.dateTime = currentDateTime.ToString("yyyy-MM-dd");
            LoadData.settingShopList = false;    
            LoadData.shopListChangeCount[0] = 3;
            LoadData.shopListChangeCount[1] = 2;

            LoadData.getGift = false;

            for(int i = 0; i < LoadData.questData.Count; i++) {
                LoadData.questData[i].Setting();
            }

            data = LoadData;
            SaveData();
        }
        else if(isPast > 0) {
            LoadData.dateTime = currentDateTime.ToString("yyyy-MM-dd");
            data = LoadData;
            SaveData();
        }
        
        return isPast;
    }

    public void GetSoul(int value){
        data.soul += value;
        SaveData();
    }
}
