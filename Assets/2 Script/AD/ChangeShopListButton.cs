using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShopListButton : MonoBehaviour
{
    [SerializeField] private Text countText;
    
    public int maxCount;
    private int _count;
    public int count {
        get {
            return _count;
        }
        set {
            _count = value;
            countText.text = "<color=yellow>"+ count + " / " + maxCount + "</color>";
            CheckCount();
        }
    }
    private Button button;
    private ChangeShopListButtonAd callAd;
    [SerializeField] private MakeSellingList makeSellingList;
    void Awake()
    {        
        TryGetComponent<ChangeShopListButtonAd>(out callAd);
        makeSellingList = GameObject.FindObjectOfType<MakeSellingList>();
        button = GetComponent<Button>();
        count = maxCount;
        

        if(callAd == null) {
            button.onClick.AddListener(() => {
                //\\TODO : 재화에 맞춰 상점 초기화 시켜주기
                if(GameDataManger.Instance.GetGameData().soul >= 100) {
                    count--;

                    GameDataManger.Instance.GetGameData().soul -= 100;
                    GameDataManger.Instance.GetGameData().shopListChangeCount[0] = count;
                    makeSellingList.SettingShopList();
                    GameDataManger.Instance.SaveData();
                }
                else return;
            });
        }
        

    }
    void Start() {
        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() => {
            Debug.Log("Setting ChangeAbleCount in shop" + GameDataManger.Instance.GetGameData().shopListChangeCount[0]);
            if(callAd == null) count = GameDataManger.Instance.GetGameData().shopListChangeCount[0];
            else count = GameDataManger.Instance.GetGameData().shopListChangeCount[1];
            
            
            CheckCount();
        }));
    }
    void CheckCount(){
        if(count == -1) return;

        if(count == 0) {
            button.interactable = false;
        }
        else {
            button.interactable = true;
        }
    }
}
