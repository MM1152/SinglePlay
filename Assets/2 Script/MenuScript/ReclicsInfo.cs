using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReclicsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity
{
    [HideInInspector] public ReclicsInfo parentReclicsInfo;
    [SerializeField] ReclicsData reclicsData;
    [SerializeField] GameObject lockObj;
    [SerializeField] Text levelText;

    public float spawnProbabillity { get; set; } // 순서 정렬용
    [SerializeField] private int _reclicsLevel;
    [SerializeField] private int _reclicsCount;
    [SerializeField] private int _reclicsMaxCount = 2;
    

    public Action setSlider;

    bool onClick;
    private void Update(){
        Check();
    }
    private void Awake() {
        spawnProbabillity = reclicsData.reclicsType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick) {
            if(parentReclicsInfo != null) {
                ReclicsTab.setInfo(parentReclicsInfo); 
                return;
            }
            ReclicsTab.setInfo(this); 
        }
    }

    public void PickUp() {
        _reclicsCount++;
        ChangeStatus();
    }
     
    public void Setting(int reclicsLevel , int reclicsCount){
        if(reclicsLevel == 0 && reclicsCount == 0) {
            return;
        }
        _reclicsLevel = reclicsLevel;
        _reclicsCount = reclicsCount;
        _reclicsMaxCount = reclicsLevel * 2;
        levelText.text = _reclicsLevel + "";
    }
    
    public ReclicsInfo LevelUp(){
        _reclicsLevel++;
        _reclicsCount -= _reclicsMaxCount;
        _reclicsMaxCount = _reclicsLevel * 2;
        levelText.text = _reclicsLevel + "";

        ChangeStatus();

        return this;
    }

    public void ChangeStatus(){
        GameData data = GameDataManger.Instance.GetGameData();
        data.reclicsLevel[reclicsData.reclicsType - 1] = _reclicsLevel;
        data.reclicsCount[reclicsData.reclicsType - 1] = _reclicsCount;
        GameDataManger.Instance.SaveData();
    }
    
    private void Check(){
        if(_reclicsCount > 0 || _reclicsLevel > 0) {
            onClick = true;
            setSlider?.Invoke();
            lockObj.SetActive(false);
        }
    }
    public int GetReclicsLevel(){
        return _reclicsLevel;
    }

    public int GetReclicsCount(){
        return _reclicsCount;
    }

    public int GetReclicsMaxCount(){
        return _reclicsMaxCount;
    }

    public ReclicsData GetReclicsData(){
        return reclicsData;
    }
}
