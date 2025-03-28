using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

[Serializable]
public class GameData
{
    public string userName;
    public int soul;
    public int gem;
    public int playAbleCount;
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
    public List<int> battleEquip = new List<int>();

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

    public List<BattleUserData> battleUserDatas = new List<BattleUserData>();
    public bool settingBattleUserData;
    public int userBattleListReRoll;
    public int Ad_playAbleCount;
    
    public bool tutorial;
}

[Serializable]
public class DailyQuestData
{
    public DailyQuestData(string type)
    {

        this.type = type;
        this.count = 0;
        this.isClear = false;

        if (type == "Login") count = 1;
    }

    public void Setting()
    {
        this.count = 0;
        this.isClear = false;

        if (type == "Login") count = 1;
    }
    public string type;
    public int count;
    public bool isClear;
}

[Serializable]
public class CouponInfo
{
    public CouponInfo(string id, string type, int value)
    {
        couponId = id;
        giftType = type;
        this.value = value;
    }
    public string couponId;
    public bool isAcquire = false;
    // soul , gem ,
    public string giftType;
    public int value;
}

[Serializable]
public class CouponData
{
    public List<CouponInfo> couponInfo = new List<CouponInfo>();
}
public class GameDataManger : MonoBehaviour
{
    public static GameDataManger Instance { get; private set; }
    public bool isTest; // 데이터 암호화 유무 
    [SerializeField] GameData data = null;
    [SerializeField] CouponData couponData = null;
    [SerializeField] BattleDatas battleData = null;
    UI_UserFightReward ui_UserFightReward;
    [HideInInspector] public int battlUserIndex;
    [HideInInspector] public Action<int, int> goodsSetting;
    private string filePath;
    private string couponFilePath;
    private string battleFilePath;
    private bool isCheckDate;
    [HideInInspector] public bool dataDownLoad;
    
    public enum SaveType {
        GameData , CouponData , BattleData
    }

    private void Awake()
    {
        #if UNITY_EDITOR 
            isTest = true;
        #elif UNITY_ANDROID 
            isTest = false;
        #endif

        filePath = Application.persistentDataPath + "/data.json";
        couponFilePath = Application.persistentDataPath + "/coupon.json";
        battleFilePath = Application.persistentDataPath + "/battle.json";
     
        Debug.Log(filePath);
        Debug.Log(couponFilePath);
        Debug.Log(battleFilePath);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

    }
    private void Start()
    {
        
        battleData = LoadBattleData();
        data = LoadData();
        couponData = LoadCouponData();


        DailyQuestTab.dailyQuestTab.Setting();
        if(!data.tutorial && !GameManager.Instance.isPlayingTutorial) {
            GameManager.Instance.StartTutorial(0);
            GameManager.Instance.isPlayingTutorial = true;
        } 
    }
    #region BattleData
    public BattleDatas LoadBattleData(){
        BattleDatas battleDatas = new BattleDatas();
        if(!File.Exists(battleFilePath)){
            string JsonData = JsonUtility.ToJson(GameManager.Instance.connectDB.battleuserData);
            File.WriteAllText(battleFilePath , JsonData);
        }

        string battleData = File.ReadAllText(battleFilePath);
        if(Key.IsEncrypted(battleData)) battleData = Key.Decrypt(battleData);
        
        battleDatas = JsonUtility.FromJson<BattleDatas>(battleData);

        return battleDatas;
    }

    public BattleDatas GetBattleData() {
        return battleData;
    }
    #endregion

    
    #region GameData
    public GameData LoadData()
    {
        GameData LoadData = new GameData();

        //저장된 파일이 없을때 실행
        if (!File.Exists(filePath))
        {
            LoadData.unLockMap.Add(true);
            for (int i = 0; i < SoulsManager.Instance.soulsInfos.Length; i++)
            {
                LoadData.soulsLevel.Add(0);
                LoadData.soulsCount.Add(0);
            }

            for (int i = 0; i < ReclicsManager.Instance.reclicsDatas.Length; i++)
            {
                LoadData.reclicsLevel.Add(0);
                LoadData.reclicsCount.Add(0);
            }

            for (int i = 0; i < 5; i++)
            {
                LoadData.soulsEquip.Add(0);
            }

            for (int i = 0; i < 8; i++)
            {
                LoadData.sellingItemListType.Add("");
                LoadData.sellingItemListNum.Add(-1);
                LoadData.sellingGem.Add(false);
                LoadData.soldOutItem.Add(false);
            }

            for (int i = 0; i < 28; i++)
            {
                LoadData.dailyGift.Add(false);
            }

            foreach (string type in Enum.GetNames(typeof(QuestType)))
            {
                LoadData.questData.Add(new DailyQuestData(type));
            }

            for (int i = 0; i < 3; i++)
            {
                LoadData.isBoxOpen.Add(false);
            }

            for( int i = 0 ; i < 5; i++) {
                LoadData.battleEquip.Add(0);
            }
            LoadData.settingShopList = false;

            LoadData.openCount.Add(0);
            LoadData.openCount.Add(1);

            LoadData.shopListChangeCount.Add(3);
            LoadData.shopListChangeCount.Add(2);

            LoadData.gem = 0;
            LoadData.soul = 0;
            LoadData.tutorial = false;

            LoadData.dateTime = GetTime.currentTime;
            LoadData.getGift = false;
            string JsonData = JsonUtility.ToJson(LoadData);

            File.WriteAllText(filePath, JsonData);
        }

        string data = File.ReadAllText(filePath);
        if(Key.IsEncrypted(data)) data = Key.Decrypt(data);
        LoadData = JsonUtility.FromJson<GameData>(data);
        

        if(!isCheckDate) {
            CheckChangeData(LoadData);
            CheckCompareDateTime(LoadData);
            for(int i = 0 ; i < battleData.user.Count; i++) {

                if(battleData.user[i].userName == LoadData.userName) {
                  
                    battlUserIndex = i;
                    break;
                }
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 1) goodsSetting?.Invoke(LoadData.soul, LoadData.gem);
        dataDownLoad = true;
        return LoadData;
    }
    public void SaveData(SaveType saveType)
    {   
        string jsonData;
        switch (saveType) {
            
            case SaveType.GameData :
                jsonData = JsonUtility.ToJson(data);
                if(!isTest) jsonData = Key.Encrypt(jsonData);
                if (File.Exists(filePath)) File.WriteAllText(filePath, jsonData);
                
                if(!isTest) jsonData = Key.Decrypt(jsonData);
                data = JsonUtility.FromJson<GameData>(jsonData);
                break;

            case SaveType.CouponData :
                jsonData = JsonUtility.ToJson(couponData);
                if(!isTest) jsonData = Key.Encrypt(jsonData);
                if (File.Exists(couponFilePath)) File.WriteAllText(couponFilePath, jsonData);

                if(!isTest) jsonData = Key.Decrypt(jsonData);
                couponData = JsonUtility.FromJson<CouponData>(jsonData);
                break;

            case SaveType.BattleData :
                jsonData = JsonUtility.ToJson(battleData);
                if(!isTest) jsonData = Key.Encrypt(jsonData);
                if (File.Exists(battleFilePath)) File.WriteAllText(battleFilePath, jsonData);

                if(!isTest) jsonData = Key.Decrypt(jsonData);
                battleData = JsonUtility.FromJson<BattleDatas>(jsonData);
                break;
            
        }

    }
    public GameData GetGameData()
    {
        return data;
    }
    #endregion
    
    #region CouponData
    public CouponData LoadCouponData()
    {
        CouponData couponData = new CouponData();

        if (!File.Exists(couponFilePath))
        {
            for (int i = 0; i < Coupon.InitCoupons.Length; i++)
            {
                couponData.couponInfo.Add(new CouponInfo(Coupon.InitCoupons[i].couponId, Coupon.InitCoupons[i].giftType, Coupon.InitCoupons[i].value));
            }
            File.WriteAllText(couponFilePath, JsonUtility.ToJson(couponData));
        }

        if (File.Exists(couponFilePath))
        {
            couponData = JsonUtility.FromJson<CouponData>(File.ReadAllText(couponFilePath));

            if (Coupon.InitCoupons.Length != couponData.couponInfo.Count)
            {
                for (int i = couponData.couponInfo.Count; i < Coupon.InitCoupons.Length - couponData.couponInfo.Count; i++)
                {
                    couponData.couponInfo.Add(new CouponInfo(Coupon.InitCoupons[i].couponId, Coupon.InitCoupons[i].giftType, Coupon.InitCoupons[i].value));
                }

                File.WriteAllText(couponFilePath, JsonUtility.ToJson(couponData));
            }
        }
        string coupon = File.ReadAllText(couponFilePath);
        if(Key.IsEncrypted(coupon)) coupon = Key.Decrypt(coupon);
        couponData = JsonUtility.FromJson<CouponData>(coupon);
        return couponData;
    }
    public void SaveCouponData()
    {
        string jsonData = JsonUtility.ToJson(couponData);

        if (File.Exists(couponFilePath)) File.WriteAllText(couponFilePath, jsonData);

        couponData = LoadCouponData();
        Debug.Log(couponFilePath);
    }
    public CouponData GetCouponData()
    {
        return couponData;
    }
    #endregion
    
    public static IEnumerator WaitForDownLoadData(Action GetGameData = null)
    {
        yield return new WaitUntil(() => Instance.dataDownLoad);
        GetGameData?.Invoke();
    }
    //데이터 로드시 데이터의 갯수와 내가 저장한 데이터의 길이가 다를때 실행된다.
    //EX ) 데이터 추가 등
    void CheckChangeData(GameData LoadData)
    {
        
        bool changeData = false;
        if (LoadData.dateTime == null)
        {
            LoadData.dateTime = GetTime.currentTime;
            this.data = LoadData;
            changeData = true;
        }

        if (SoulsManager.Instance.soulsInfos.Length != 0 && SoulsManager.Instance.soulsInfos.Length != LoadData.soulsCount.Count)
        {
            for (int i = 0; i <= SoulsManager.Instance.soulsInfos.Length - LoadData.soulsCount.Count; i++)
            {
                LoadData.soulsCount.Add(0);
                LoadData.soulsLevel.Add(0);
            }
            this.data = LoadData;
            changeData = true;
        }

        if (ReclicsManager.Instance.reclicsDatas.Length != 0 && ReclicsManager.Instance.reclicsDatas.Length != LoadData.reclicsCount.Count)
        {
            for (int i = 0; i <= ReclicsManager.Instance.reclicsDatas.Length - LoadData.reclicsCount.Count; i++)
            {
                LoadData.reclicsCount.Add(0);
                LoadData.reclicsLevel.Add(0);
            }
            this.data = LoadData;
            changeData = true;
        }

        if (LoadData.soldOutItem.Count == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                LoadData.soldOutItem.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }

        if (LoadData.dailyGift.Count == 0)
        {
            for (int i = 0; i < 28; i++)
            {
                LoadData.dailyGift.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }

        if (LoadData.openCount.Count == 0)
        {
            LoadData.openCount.Add(0);
            LoadData.openCount.Add(0);
            this.data = LoadData;
            changeData = true;
        }

        if (LoadData.questData.Count == 0)
        {
            foreach (string type in Enum.GetNames(typeof(QuestType)))
            {
                LoadData.questData.Add(new DailyQuestData(type));
            }
            this.data = LoadData;
            changeData = true;
        }

        if (LoadData.isBoxOpen.Count == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                LoadData.isBoxOpen.Add(false);
            }
            this.data = LoadData;
            changeData = true;
        }
        if (LoadData.battleEquip.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                LoadData.battleEquip.Add(0);
            }
            this.data = LoadData;
            changeData = true;
        }
        if (changeData) SaveData(SaveType.GameData);

    }
    int CheckCompareDateTime(GameData LoadData)
    {
        DateTime pastDateTime = Convert.ToDateTime(LoadData.dateTime);
        DateTime currentDateTime = Convert.ToDateTime(GetTime.currentTime);
        int isPast = DateTime.Compare(pastDateTime, currentDateTime);
        if (isPast < 0)
        {
            Debug.Log("과거 달 : " + pastDateTime.Month);
            Debug.Log("현재 달 : " + currentDateTime.Month);
            if (pastDateTime.Month < currentDateTime.Month)
            {
                for (int i = 0; i < 28; i++)
                {
                    LoadData.dailyGift[i] = false;
                }
            }

            if(currentDateTime.DayOfWeek == DayOfWeek.Monday) {
                MenuScene.menuScene.StartCoroutine(MenuScene.menuScene.GetUi(typeof(UI_UserFightReward) , (component) => {
                    ui_UserFightReward = component.GetComponent<UI_UserFightReward>();
                    ui_UserFightReward.Init();
                }));
            }

            LoadData.dateTime = currentDateTime.ToString("yyyy-MM-dd");
            LoadData.playAbleCount = 5;
            LoadData.settingShopList = false;
            LoadData.shopListChangeCount[0] = 3;
            LoadData.shopListChangeCount[1] = 2;
            LoadData.settingBattleUserData = false;
            LoadData.battleUserDatas.Clear();
            LoadData.getGift = false;
            LoadData.Ad_playAbleCount = 3;
            LoadData.userBattleListReRoll = 3;

            for (int i = 0; i < LoadData.questData.Count; i++)
            {
                LoadData.questData[i].Setting();
            }

            for (int i = 0; i < LoadData.isBoxOpen.Count; i++)
            {
                LoadData.isBoxOpen[i] = false;
            }
            
            data = LoadData;
            


            SaveData(SaveType.GameData);
        }
        else if (isPast > 0)
        {
            LoadData.dateTime = currentDateTime.ToString("yyyy-MM-dd");
            data = LoadData;
            SaveData(SaveType.GameData);
        }
        isCheckDate = true;
                    
        GameManager.Instance.connectDB.ReadData<BattleUserData>(ConnectDB.ReadType.Users , callback1 : (data) => {
            battleData.user = data;
            SaveData(SaveType.BattleData);
        });
        
        return isPast;
    }

    public void GetSoul(int value)
    {
        data.soul += value;
        SaveData(SaveType.GameData);
    }
    public void GetGem(int value)
    {
        data.gem += value;
        SaveData(SaveType.GameData);
    }
}
