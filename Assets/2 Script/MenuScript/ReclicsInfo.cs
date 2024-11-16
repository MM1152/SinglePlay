using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReclicsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity
{
    [SerializeField] ReclicsData reclicsData;
    [SerializeField] GameObject lockObj;
    [SerializeField] Text levelText;

    public float spawnProbabillity { get; set; } // 순서 정렬용
    [SerializeField] private int _reclicsLevel;
    [SerializeField] private int _reclicsCount;
    [SerializeField] private int _reclicsMaxCount = 2;
    

    public Action setSlider;

    bool onClick;
    private void Awake() {
        spawnProbabillity = reclicsData.reclicsType;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick) ReclicsTab.setInfo(this);
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
        _reclicsMaxCount *= reclicsLevel == 0 ? 1 : reclicsLevel;
        levelText.text = _reclicsLevel + "";
        setSlider.Invoke();

        onClick = true;
        lockObj.SetActive(false);
    }
    
    public ReclicsInfo LevelUp(){
        _reclicsLevel++;
        _reclicsCount -= _reclicsMaxCount;
        _reclicsMaxCount *= _reclicsLevel;
        levelText.text = _reclicsLevel + "";
        setSlider.Invoke();

        ChangeStatus();

        return this;
    }

    public void ChangeStatus(){
        GameData data = GameDataManger.Instance.GetGameData();
        data.reclicsLevel[reclicsData.reclicsType - 1] = _reclicsLevel;
        data.reclicsCount[reclicsData.reclicsType - 1] = _reclicsCount;
        GameDataManger.Instance.SaveData();
    }
}
