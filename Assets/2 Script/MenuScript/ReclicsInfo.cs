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
    public int cost;
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
    public string saveDataType { get; set ; }
    public int saveDatanum { get ; set ; }

    public Action setSlider;

    private int maxLevel;
    public bool onClick;
    private void Update(){
        Check();
    }
    private void Awake() {
        reclicsImage.sprite = reclicsData.image;
        reclicsData.classStruct = new ClassStruct(reclicsData.itemclass);
        color = reclicsData.classStruct;
        spawnProbabillity = reclicsData.reclicsType;

        image = reclicsData.image;
        classStruct = reclicsData.classStruct;
        saveDataType = "Reclics";
        saveDatanum = reclicsData.reclicsType - 1;
        maxLevel = 12;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(onClick) {
            SoundManager.Instance.Play(SoundManager.SFX.SelectItem);
            if(parentReclicsInfo != null) {
                reclicsTab.setInfo(parentReclicsInfo); 
                return;
            }
            reclicsTab.setInfo(this); 
        }else {
            SoundManager.Instance.Play(SoundManager.SFX.DisOpen);
        }
    }

    public void PickUp() {
        _reclicsCount++;
        levelText.text = _reclicsLevel + 1 + "";
        Setting(_reclicsCount , _reclicsLevel);
        ChangeStatus();
    }
     
    public void Setting(int reclicsLevel , int reclicsCount){
        if(reclicsLevel == 0 && reclicsCount == 0) return;
        
        _reclicsLevel = reclicsLevel;
        _reclicsCount = reclicsCount;
        _reclicsMaxCount = _reclicsLevel == 0 ? _reclicsMaxCount : (_reclicsLevel + 1) * 2;
        SetCost();
        Check();
        levelText.text = _reclicsLevel + 1 + "";
    }
    
    public ReclicsInfo LevelUp(){
        if(GameDataManger.Instance.GetGameData().soul < cost) return this;
        if(CheckMaxLevel()) return this;

        _reclicsLevel++;
        _reclicsCount -= _reclicsMaxCount;
        _reclicsMaxCount =  _reclicsMaxCount = _reclicsLevel == 0 ? _reclicsMaxCount : (_reclicsLevel + 1) * 2;
        levelText.text = _reclicsLevel + 1 + "";

        setSlider?.Invoke();
        SetCost();
        ChangeStatus(cost);
        
        DailyQuestTab.ClearDailyQuest(QuestType.UpgradeReclic , 1);
        return this;
    }

    public void ChangeStatus(int cost = 0){
        GameData data = GameDataManger.Instance.GetGameData();
        data.soul -= cost;
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
    void SetCost(){
        cost = (int)(reclicsData.classStruct.initCost * ((_reclicsLevel + 1)* reclicsData.classStruct.levelUpCost));

        int copyCost = cost;
        int length = 0;
        while(copyCost != 0) {
            length++;
            copyCost /= 10;
        }

        if(length - 2 >= 0) {
            cost = cost - cost % (int)Math.Pow(10 , length - 2); 
        }
    }
    bool CheckMaxLevel(){
        if(_reclicsLevel >= 12) return true;
        else return false;
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
