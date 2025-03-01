using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour , IPointerClickHandler
{   
    public string mapName;
    public float obtainablegoodds;
    public int maxStage;
    [TextArea] public string infomationText;
    bool onClick;

    Image image;
    GameObject lockGameObj;
    [SerializeField] GameObject infomationTab;
    MapInfomationTab mapInfomationTab;
    Color InitColor = new Color(0f , 0f , 0f , 0.7f);
    Color UnLockColor = new Color(0.3f , 0.3f , 0.3f , 1f);
    public bool unLock {
        set {
            if(value) {
                if(mapName == "null") return;
                
                image.color = UnLockColor;
                lockGameObj.SetActive(false);
                onClick = true;
            }
            else image.color = InitColor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick) {
            if(mapName == "null") return;
            mapInfomationTab.name = mapName;
            mapInfomationTab.infomation = infomationText;
            mapInfomationTab.obtainablegoods = obtainablegoodds;
            mapInfomationTab.maxStage = maxStage;
            mapInfomationTab.mapindex = transform.GetSiblingIndex() + 1;
            infomationTab.SetActive(true);

            if(GameManager.Instance.isPlayingTutorial) {
                GameManager.Instance.StartTutorial(2);
            }
        }
    }

    private void Awake() {
        mapInfomationTab = infomationTab.GetComponent<MapInfomationTab>();
        image = GetComponent<Image>();
        lockGameObj = transform.Find("MapImage").transform.Find("Lock").gameObject;
        unLock = false;
    }
}
