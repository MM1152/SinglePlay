using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShopListButton : MonoBehaviour
{
    [SerializeField] private Text countText;
    
    public int maxCount;
    private int count;
    private Button button;
    private CallAd callAd;
    private MakeSellingList makeSellingList;
    void Awake()
    {        
        TryGetComponent<CallAd>(out callAd);
        makeSellingList = GameObject.FindObjectOfType<MakeSellingList>();
        count = maxCount;
        button = GetComponent<Button>();

        button.onClick.AddListener(() => {
            count--;
            countText.text = "<color=yellow>"+ count + " / " + maxCount + "</color>";
            CheckCount();
        });
        if(callAd == null) {
            button.onClick.AddListener(() => {
                //\\TODO : 재화에 맞춰 상점 초기화 시켜주기
                makeSellingList.SettingShopList();
            });
        }
        
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
