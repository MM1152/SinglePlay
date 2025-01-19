using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[DefaultExecutionOrder(0)]
public class ReclicsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity , IClassColor , ISellingAble
{
    public ReclicsInfo parentReclicsInfo;
    [SerializeField] ReclicsTab reclicsTab;
    [SerializeField] ReclicsData reclicsData;
    [SerializeField] GameObject lockObj;
    [SerializeField] Text levelText;
    [SerializeField] Image reclicsImage;
    public float spawnProbabillity { get; set; } // 순서 정렬용
    public ClassStruct color { get; set; }


    [SerializeField] private int _reclicsLevel;
    [SerializeField] private int _reclicsCount;
    [SerializeField] private int _reclicsMaxCount = 2;
    

    public Sprite image { get ; set ; }
    public ClassStruct classStruct { get ; set; }

    public Action setSlider;

    public bool onClick;
    private void Update(){
        Check();
    }
    private void Awake() {
        reclicsImage.sprite = reclicsData.image;
        reclicsData.classStruct = new ClassStruct(reclicsData.itemclass);
        color = reclicsData.classStruct;
        spawnProbabillity = reclicsData.reclicsType;
        reclicsTab = GameObject.FindObjectOfType<ReclicsTab>();
        image = reclicsData.image;
        classStruct = reclicsData.classStruct;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick) {
            if(parentReclicsInfo != null) {
                reclicsTab.setInfo(parentReclicsInfo); 
                return;
            }
            reclicsTab.setInfo(this); 
        }
    }

    public void PickUp() {
        _reclicsCount++;
        levelText.text = _reclicsLevel + 1 + "";
        
        ChangeStatus();
    }
     
    public void Setting(int reclicsLevel , int reclicsCount){
        if(reclicsLevel == 0 && reclicsCount == 0) return;
        
        _reclicsLevel = reclicsLevel;
        _reclicsCount = reclicsCount;
        _reclicsMaxCount = _reclicsLevel == 0 ? _reclicsMaxCount : (int) math.pow((_reclicsLevel + 1) , 2);
        levelText.text = _reclicsLevel + 1 + "";
    }
    
    public ReclicsInfo LevelUp(){
        _reclicsLevel++;
        _reclicsCount -= _reclicsMaxCount;
        _reclicsMaxCount =  _reclicsMaxCount = _reclicsLevel == 0 ? _reclicsMaxCount : (int) math.pow((_reclicsLevel + 1) , 2);
        levelText.text = _reclicsLevel + 1 + "";

        setSlider?.Invoke();
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
            lockObj.SetActive(false);
            setSlider?.Invoke();
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
