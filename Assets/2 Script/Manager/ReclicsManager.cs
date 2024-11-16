using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ReclicsManager : MonoBehaviour
{
    public static ReclicsManager Instance {get ; private set;}
    public ReclicsInfo[] reclicsDatas;
    // Start is called before the first frame update
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } 
        else {
            Destroy(this);
        }
        
    }
    void Start()
    {
        reclicsDatas = GameObject.FindObjectsOfType<ReclicsInfo>();
        MergeSort<ReclicsInfo> mergeSort = new MergeSort<ReclicsInfo>(reclicsDatas);
        reclicsDatas = mergeSort.get();
        Array.Reverse(reclicsDatas);
        //\\TODO 외부 데이터가 존재하면 외부데이터 불러와 사용 가능하도록 구현해줘야됌

        SetReclicsInfo(GameDataManger.Instance.GetGameData());
    }

    void SetReclicsInfo(GameData data){
        for(int i = 0; i < reclicsDatas.Length; i++){
            Debug.Log(data.reclicsCount[i] + "");
            reclicsDatas[i].Setting(data.reclicsLevel[i] , data.reclicsCount[i]);
        }
    }
}
