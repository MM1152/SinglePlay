using System;
using UnityEditorInternal;
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

    public void LoadUi(string filePath , Action<GameObject> getUiData = null){
        ResourceRequest load = Resources.LoadAsync<GameObject>("UI/" + filePath);
        load.completed += (task) => {
            if(task.isDone) {
                GameObject go = (GameObject) load.asset;
                getUiData?.Invoke(go);
            }
        };
    }

    public T GetAddUiData<T>(GameObject go) where T : Component{
        T componet = go.GetComponent<T>();

        if(componet == null) {
            componet = go.AddComponent<T>();
        }

        return componet;
    }

}
