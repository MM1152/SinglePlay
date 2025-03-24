using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Diagnostics;


[Serializable]
public class BattleDatas {
    public List<BattleUserData> user = new List<BattleUserData>();
}

[Serializable]
public class BattleUserData : ISpawnPosibillity{
    public string userName;
    public List<MobData> mobList  = new List<MobData>();
    public int battleScore;

    public float spawnProbabillity { get ; set ; }
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
    public enum ReadType {
        None , Users , Version , BattleScore
    }
    public enum WriteType {
        None , BattleScore , Users
    }
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
            UnityEngine.Debug.LogError(ex);
        }
    }

    IEnumerator SettingFin(Action action){
        yield return new WaitUntil(() => connection);
        action.Invoke();
    }


    public T ReadData<T>(ReadType type , Action<T> callback = null , Action<List<T>> callback1 = null){
        if(!connection) {
            StartCoroutine(SettingFin(() => ReadData<T>(type , callback , callback1)));
            return default;
        } 

        DatabaseReference _reference = FirebaseDatabase.DefaultInstance.GetReference(type.ToString());
        
        _reference.GetValueAsync().ContinueWithOnMainThread(task => {
            if(task.IsCompleted) {
                DataSnapshot snapshot = task.Result;

                if(type == ReadType.Version) callback?.Invoke((T)snapshot.Value);
                
                if(type == ReadType.Users){
                    List<T> returnValue = new List<T>();

                    foreach(var user in snapshot.Children) {
                        returnValue.Add(JsonUtility.FromJson<T>(user.GetRawJsonValue()));  
                    }
                    
                    callback1?.Invoke(returnValue);
                }
            }
        });
        return default;
    }

    public void WriteUserData(){
        BattleUserData mob = new BattleUserData();
        mob.userName = GameDataManger.Instance.GetGameData().userName;

        string json = JsonUtility.ToJson(mob);
        
        m_Reference.Child("Users").Child(mob.userName).SetRawJsonValueAsync(json);
    }

    public void WriteBattleData(BattleUserData battleUserData){
        string json = JsonUtility.ToJson(battleUserData);

        m_Reference.Child("Users").Child(GameDataManger.Instance.GetGameData().userName).SetRawJsonValueAsync(json);
    }

    public void WriteBattleScore(){
        BattleUserData mob = GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex];
        UnityEngine.Debug.Log(m_Reference.Child("Users").Child(mob.userName).GetValueAsync());
        m_Reference.Child("Users").Child(mob.userName).UpdateChildrenAsync(new Dictionary<string, object>() {{"battleScore" , mob.battleScore}});
    }
}
