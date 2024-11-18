using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSetting : MonoBehaviour
{
    [SerializeField] GameObject mapListParent;
    [SerializeField] List<SelectMap> mapList = new List<SelectMap>();
    void Awake()
    {
        for(int i = 0 ; i < mapListParent.transform.childCount; i++) {
            mapList.Add(mapListParent.transform.GetChild(i).GetComponent<SelectMap>());
        }
        
    }
    private void Start() {
        StartCoroutine(WaitForDownLoadData());
    }

    public void Setting(List<bool> mapData){
        for(int i = 0 ; i < mapData.Count; i++) {
            if(!mapData[i]) return;

            mapList[i].unLock = true;
        }
    }

    IEnumerator WaitForDownLoadData(){
        Debug.Log("WaitData");
        yield return new WaitUntil(() => GameDataManger.Instance.dataDownLoad);
        Debug.Log("DataDownLoadFin");
        Setting(GameDataManger.Instance.GetGameData().unLockMap);
    }
}
