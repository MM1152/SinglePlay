using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using Unity.VisualScripting;

[Serializable] 
public class GameData{
    public int soul;
    public int gem;
    public List<bool> unLockMap = new List<bool>();
    public List<int> reclicsLevel = new List<int>();
    public List<int> reclicsCount = new List<int>();
}


public class GameDataManger : MonoBehaviour
{   //\\TODO : 서버든 오프라인으로 저장해놓은 데이터들 불러와서 일괄적으로 적용시켜줘야함
    //  유물 데이터 , 맵 데이터 , 재화 정보 , 수집한 영혼 정보 등.
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
           
            for(int i = 0; i < ReclicsManager.Instance.reclicsDatas.Length; i++) {
                LoadData.reclicsLevel.Add(0);
                LoadData.reclicsCount.Add(0);
            }
            LoadData.gem = 0;
            LoadData.soul = 0;

            string JsonData = JsonUtility.ToJson(LoadData);

            File.WriteAllText(filePath , JsonData);
        }

        //파일 로드시 유물데이터의 갯수와 내가 저장한 데이터의 길이가 다를때 실행된다.
        //EX ) 유물데이터 추가 등
        if(ReclicsManager.Instance.reclicsDatas.Length != LoadData.reclicsCount.Count) {
            for(int i = 0 ; i < ReclicsManager.Instance.reclicsDatas.Length - LoadData.reclicsCount.Count; i++){
                LoadData.reclicsCount.Add(0);
                LoadData.reclicsLevel.Add(0);
            }
        }

        string data = File.ReadAllText(filePath);
        LoadData = JsonUtility.FromJson<GameData>(data);
        dataDownLoad = true;
        return LoadData;
    }   

    public void SaveData(){
        string jsonData = JsonUtility.ToJson(data);

        if(File.Exists(filePath)) File.WriteAllText(filePath , jsonData);
        
        Debug.Log(filePath);
    }
    public GameData GetGameData(){
        return data;
    }

}
