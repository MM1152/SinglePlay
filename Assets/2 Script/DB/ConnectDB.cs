using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;


[Serializable]
public class BattleDatas {
    public List<BattleUserData> battleUserDatas = new List<BattleUserData>();
}

[Serializable]
public class BattleUserData{
    public string userName;
    public List<MobData> mobList  = new List<MobData>();
    public int battleScore;
}

[Serializable]
public class MobData {
    public MobData (string name , int typeNumber , int level){
        this.name = name;
        this.typeNumber = typeNumber;
        this.level = level;
    }
    public string name;
    public int typeNumber;
    public int level;
}


public class ConnectDB : MonoBehaviour
{
    // Start is called before the first frame update
    FirebaseApp app;
    DatabaseReference m_Reference;
    public bool connection;

    public BattleDatas battleuserData;

    public void Init()
    {
        try {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                m_Reference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
            connection = true;
            });

        } catch (Exception ex) {
            Debug.LogError(ex);
        }
    }

    IEnumerator SettingFin(Action action){
        yield return new WaitUntil(() => connection);
        action.Invoke();
    }


    public void CheckVersion(Action<string> getVersion){
        if(!connection) {
            StartCoroutine(SettingFin(() => CheckVersion(getVersion)));
            return;
        }
        try {
            FirebaseDatabase.DefaultInstance.GetReference("Version").GetValueAsync().ContinueWithOnMainThread(task => {
                if(task.IsFaulted) {
                    Debug.LogError("Fail To Get Version");
                    getVersion(""); 
                }
                else if(task.IsCompleted) {
                    DataSnapshot snapshot = task.Result; 
                    Debug.Log(snapshot);
                    getVersion(snapshot.Value.ToString());    
                }
            });
        }catch (Exception ex) {
            Debug.LogError(ex);
            getVersion("");
        }
        
    }
    public void CheckBattleUserData(Action getData = null){
        if(!connection) {
            StartCoroutine(SettingFin(() => CheckBattleUserData(getData)));
            return;
        }
        try {
            var task = FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWithOnMainThread(task => {
                if(task.IsFaulted) {
                    Debug.Log("Fail To get BattleUserData");
                }
                else if(task.IsCompleted) {
                    DataSnapshot snapshot = task.Result; 
                
                    foreach(var user in snapshot.Children) {
                        battleuserData.battleUserDatas.Add(JsonUtility.FromJson<BattleUserData>(user.GetRawJsonValue()));
                    }
                    
                    getData();
                }
            });
            

        }catch (Exception ex) {
            Debug.LogError(ex);
        }
    }

    public void WriteData(string text){
        
        m_Reference.Child("Users").Child(GameDataManger.Instance.GetGameData().userName).Child("userName").SetValueAsync(text);

        BattleUserData mob = new BattleUserData();
        mob.userName = text;

        string json = JsonUtility.ToJson(mob);
        
        m_Reference.Child("Users").Child(text).SetRawJsonValueAsync(json);
    }

    public void SettingBattleData(BattleUserData battleUserData){
        string json = JsonUtility.ToJson(battleUserData);
        m_Reference.Child("Users").Child(GameDataManger.Instance.GetGameData().userName).SetRawJsonValueAsync(json);
    }
}
