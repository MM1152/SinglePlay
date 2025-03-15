using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase;
using Firebase.Database;
using System;

[Serializable]
public class BattleUserData{
    public string userName;
    public List<MobData> mobList  = new List<MobData>();
}

[Serializable]
public class MobData {
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

    public void CheckUserName(Action<string> getName){
        if(!connection) {
            StartCoroutine(SettingFin(() => CheckUserName(getName)));
            return;
        }
        try {
            FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWithOnMainThread(task => {
                if(task.IsFaulted) {
                    Debug.LogError("Fail To Get Name");
                    getName(""); 
                }
                else if(task.IsCompleted) {
                    DataSnapshot snapshot = task.Result; 
                    Debug.Log(snapshot);
                    getName(snapshot.Value.ToString());    
                }
            });
        }catch (Exception ex) {
            Debug.LogError(ex);
            getName("");
        }
    }

    public void WriteData(string text){
        string json = JsonUtility.ToJson(text);
        m_Reference.Child("Users").Child(GameDataManger.Instance.GetGameData().userName).SetValueAsync(json);
    }
}
