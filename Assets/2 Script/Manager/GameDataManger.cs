using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.UI;

[Serializable] 
public class GameData{


    public int soul;
    public int gem;
    public List<bool> unLockMap = new List<bool>();

    /// <summary>
    /// 1: Attack , 2: Hp , 3: SummonUnitHp , 4: AttackSpeed  , 5: MoveSpeed , 6: BonusTalent , 7: BonusGoods , 8: IncreaesDamage ,
    /// 9: IncreaesHp, 10: CoolTime, 11: SkillDamage
    /// </summary>
    public List<int> reclicsLevel = new List<int>();
    public List<int> reclicsCount = new List<int>();
    public List<int> soulsLevel = new List<int>();
    public List<int> soulsCount = new List<int>();
    public List<int> soulsEquip = new List<int>();
}


public class GameDataManger : MonoBehaviour
{   
    public static GameDataManger Instance { get ; private set;}

    [SerializeField] GameData data = null;
    private string filePath;
    public bool dataDownLoad;
    private void Awake() {
        
        filePath  = Application.persistentDataPath + "/data.txt";
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
            LoadData.gem = 0;
            LoadData.soul = 0;

            string JsonData = JsonUtility.ToJson(LoadData);

            File.WriteAllText(filePath , JsonData);
        }

        string data = File.ReadAllText(filePath);
        LoadData = JsonUtility.FromJson<GameData>(data);

        //데이터 로드시 데이터의 갯수와 내가 저장한 데이터의 길이가 다를때 실행된다.
        //EX ) 데이터 추가 등
        if(SoulsManager.Instance.soulsInfos.Length != 0 && SoulsManager.Instance.soulsInfos.Length != LoadData.soulsCount.Count) {
            for(int i = 0 ; i <= SoulsManager.Instance.soulsInfos.Length - LoadData.soulsCount.Count; i++) {
                LoadData.soulsCount.Add(0);
                LoadData.soulsLevel.Add(0);
            }
            this.data = LoadData;
            SaveData();
        }

        if(ReclicsManager.Instance.reclicsDatas.Length != 0 && ReclicsManager.Instance.reclicsDatas.Length != LoadData.reclicsCount.Count) {
            for(int i = 0 ; i <= ReclicsManager.Instance.reclicsDatas.Length - LoadData.reclicsCount.Count; i++){
                LoadData.reclicsCount.Add(0);
                LoadData.reclicsLevel.Add(0);
            }
            this.data = LoadData;
            SaveData();
        }

        if(LoadData.soulsEquip.Count != 5) {
            for(int i = 0 ; i < 5; i++) {
                LoadData.soulsEquip.Add(0);
            }
            this.data = LoadData;
            SaveData();
        }
        

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
    
}
