using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MenuScene : MonoBehaviour
{
    public static MenuScene menuScene { get ; private set; }
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject rankInfoButton;
    [SerializeField] GameObject rankTableButton;
    Dictionary<string, GameObject> ui_map = new Dictionary<string, GameObject>();

    bool init;

    Queue<ResourceRequest> async_Tasks = new Queue<ResourceRequest>();
    Coroutine coroutine;
    void Awake() { Init(); }

    public void Init() 
    {
        if(init) {
            return;
        }
        
        init = true;
        menuScene = this;
        canvas = GameObject.Find("MenuCanvas").gameObject;
        Transform path = canvas.transform.Find("BackGround");
        rankInfoButton = path.Find("PlayerBattleTab").Find("RankInfo").gameObject;
        rankTableButton = path.Find("PlayerBattleTab").Find("Rank").gameObject;

        async_Tasks.Enqueue(BaseUI.instance.AsyncLoadUi("UserFightReward", (value) =>
        {
            GameObject go = Instantiate(value, canvas.transform);
            go.SetActive(false);
            UI_UserFightReward ui = go.GetOrAddComponent<UI_UserFightReward>();
            ui.Init(GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore);
            Debug.Log(ui.GetType().ToString());
            ui_map.Add(ui.GetType().ToString() , ui.gameObject);
            
        }));


        async_Tasks.Enqueue(BaseUI.instance.AsyncLoadUi("RankInfo", (value) =>
        {
            GameObject go = Instantiate(value, canvas.transform);
            go.SetActive(false);

            UI_Event block = go.transform.Find("BlockTab").gameObject.GetOrAddComponent<UI_Event>();
            UI_Event rankButton = rankInfoButton.GetOrAddComponent<UI_Event>();

            block.SetClickAction(() => ClickToCloseEvent(go.gameObject));
            rankButton.SetClickAction(() => ClickToOpenEvent(go.gameObject));

        }));

        async_Tasks.Enqueue(BaseUI.instance.AsyncLoadUi("RankTable", (value) =>
        {
            GameObject go = Instantiate(value, canvas.transform);
            go.SetActive(false);

            go.GetOrAddComponent<UI_RankTable>().Init();
            UI_Event block = go.transform.Find("BlockTab").gameObject.GetOrAddComponent<UI_Event>();
            UI_Event rank = rankTableButton.GetOrAddComponent<UI_Event>();

            block.SetClickAction(() => ClickToCloseEvent(go.gameObject));
            rank.SetClickAction(() => ClickToOpenEvent(go.gameObject));
        }));
        
    }

    public IEnumerator GetUi(Type type , Action<GameObject> callback)  {
        while(async_Tasks.Count > 0) {
            if(async_Tasks.Peek().isDone) {
                async_Tasks.Dequeue();
            }
            yield return null;
        }
        Debug.Log(type.ToString());
        if(ui_map.ContainsKey(type.ToString())) {
            callback?.Invoke(ui_map[type.ToString()]);
        }
        else {
            Debug.LogError("Fail To get Ui");
        }
        
    }

    #region Event 
    void ClickToCloseEvent(GameObject go)
    {
        go.SetActive(false);
    }

    void ClickToOpenEvent(GameObject go)
    {
        go.SetActive(true);
    }
    #endregion
}
