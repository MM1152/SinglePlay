using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour , IPointerClickHandler
{   
    public string mapName;
    bool onClick;

    Image image;
    GameObject lockGameObj;
    [SerializeField] GameObject infomationTab;
    MapInfomationTab mapInfomationTab;
    Color InitColor = new Color(0f , 0f , 0f , 0.7f);
    Color UnLockColor = new Color(1f , 1f , 1f , 1f);
    public bool unLock {
        set {
            if(value) {
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
            mapInfomationTab.name = mapName;
            infomationTab.SetActive(true);
        }
    }

    private void Awake() {
        mapInfomationTab = infomationTab.GetComponent<MapInfomationTab>();
        image = GetComponent<Image>();
        lockGameObj = transform.GetChild(0).gameObject;
        unLock = false;
    }
}
