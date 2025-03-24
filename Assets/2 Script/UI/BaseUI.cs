using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BaseUI
{
    
    static BaseUI _instance;
    public static BaseUI instance { get { return Init(); } }

    static BaseUI Init(){
        if(_instance == null) {
            _instance = new BaseUI();
        }

        return _instance;
    }

    public ResourceRequest AsyncLoadUi(string filePath , Action<GameObject> getUiData = null){
        ResourceRequest load = Resources.LoadAsync<GameObject>("UI/" + filePath);
        
        load.completed += (task) => {
            if(task.isDone) {
                GameObject go = (GameObject) load.asset;
                getUiData?.Invoke(go);
            }
        };

        return load;
    }

    public GameObject SynchLoadUi(string filePath){
        GameObject load = Resources.Load<GameObject>("UI/" + filePath);
        return load;
    }

}
