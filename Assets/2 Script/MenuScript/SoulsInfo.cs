using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void SettingSlider();

[DefaultExecutionOrder(0)]
public class SoulsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity , IClassColor , ISellingAble
{
    //\\TODO : 각 soul , reclics에 최대 레벨업에 필요한 갯수 계산하여 , 넘어간 상태에서 획득시 Soul로 바꿔서 지급   
    [Space(50)]
    [Header("직접 설정")]
    [SerializeField] SoulsTab soulsTab;
    [SerializeField] UnitData unitData;
    [SerializeField] Image soulImage ;
    public SoulsInfo parentSoulsInfo; 
    public Text levelText;
    public Slider slider;
    [SerializeField] GameObject lockObejct;
    public int cost;
    
    public int soulLevel; 
    public int soulCount;
    public int soulMaxCount;
    public float soulInintPercent;
    public float soulLevelUpPercent;


    public float spawnProbabillity { get ; set ; }
    public ClassStruct color { get ; set ; }


    public Sprite image { get ; set ; }
    public ClassStruct classStruct { get ; set ; }
    public string saveDataType { get; set; }
    public int saveDatanum { get; set; }

    public SettingSlider settingslider;
    public bool unLock;
    bool[] applyStat;
    public int battlePower;

    private void Update(){
        Check();
    }
    public void Awake() {
        unitData.classStruct = new ClassStruct(unitData.type);
        soulInintPercent = unitData.classStruct.soulInintPercent;
        soulLevelUpPercent = unitData.classStruct.soulLevelUpPercent;
        applyStat = new bool[unitData.stat.GetLength(0)];

        ChangeBonusStat();
        unitData.curStat.speedStat = unitData.classStruct.soulInintPercent;

        spawnProbabillity = unitData.typenumber;      
        InterfaceSetting();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(unLock) {
            if(parentSoulsInfo == null) soulsTab.SetInfo(this , true , null);
            else soulsTab.SetInfo(parentSoulsInfo , true , null);
                    
                    
            if(GameManager.Instance.isPlayingTutorial) {
                GameManager.Instance.StartTutorial(23);
            }
            
            SoundManager.Instance.Play(SoundManager.SFX.SelectItem);
        }else {
            SoundManager.Instance.Play(SoundManager.SFX.DisOpen);
        }
       
    }
    private void Check(){
        if(soulCount > 0 || soulLevel > 0) {
            lockObejct.SetActive(false);
            unLock = true;
        }
    }
    private void InterfaceSetting(){
        image = unitData.image;
        classStruct = unitData.classStruct;
        saveDataType = "Soul";
        saveDatanum = unitData.typenumber - 1;
        color = unitData.classStruct;
        soulImage.sprite = unitData.image;
    }
    public void Setting(int soulCount , int soulLevel){

        if(soulCount == 0 && soulLevel == 0)  return;
        
        this.soulCount = soulCount;
        this.soulLevel = soulLevel;
        soulMaxCount = (this.soulLevel + 1) * 2;
        
        Check();
        CheckLevel();
        ChangeBonusStat();
        SetCost();
        SetBattlePoint();
        settingslider?.Invoke();

        levelText.text = this.soulLevel + 1 + "";
    }
    public SoulsInfo LevelUp(){
        if(GameDataManger.Instance.GetGameData().soul < cost) return this;
        if(CheckMaxLevel()) return this;

        GameDataManger.Instance.GetGameData().soul -= cost;

        soulLevel++;
        soulCount -= soulMaxCount;
        soulMaxCount = (this.soulLevel + 1) * 2;
        levelText.text = soulLevel + 1 + "";

        CheckLevel();
        ChangeBonusStat();
        ChangeStatus();
        SetCost();
        SetBattlePoint();

        settingslider?.Invoke();
        DailyQuestTab.ClearDailyQuest(QuestType.UpgradeSoul , 1);
        return this;
    }
    
    public void GetSoul(){
        soulCount++;
        Setting(soulCount , soulLevel);
        ChangeStatus();
    }

    public void ChangeBonusStat(){
        unitData.curStat.attackStat = unitData.classStruct.soulInintPercent + (unitData.classStruct.soulLevelUpPercent * soulLevel) + unitData.bonusStat.attackStat;
        unitData.curStat.hpStat = unitData.classStruct.soulInintPercent + (unitData.classStruct.soulLevelUpPercent * soulLevel) + unitData.bonusStat.hpStat;
    }

    public void SettingBonusStat(string type , float value){
        switch(type){
            case "Hp" :
                unitData.bonusStat.hpStat += value;
                break;
            case "Attack" :
                unitData.bonusStat.attackStat += value;
                break;
            case "Speed" :
                unitData.bonusStat.speedStat += value;
                break;
        }
    }

    public void ChangeStatus(){
        GameData data = GameDataManger.Instance.GetGameData();
        data.soulsLevel[unitData.typenumber - 1] = soulLevel;
        data.soulsCount[unitData.typenumber - 1] = soulCount;
        
        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
    }
    void SetBattlePoint(){
        battlePower = (int) (100 * (soulLevel + 1)* unitData.classStruct.battlePointPercent);
    }
    void SetCost(){
        cost = (int)(unitData.classStruct.initCost * ((soulLevel + 1)* unitData.classStruct.levelUpCost));

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
    public void CheckLevel(){
        for(int i = 0; i < unitData.stat.GetLength(0); i++) {
            string[] split = unitData.stat[i].Split(" "); //0 : 레벨 , 1 : 무슨종류의 스탯인지 , 2 : 적용할 스탯의 수치
            if(int.Parse(split[0]) <= soulLevel && !applyStat[i]) {
                applyStat[i] = true;
                SettingBonusStat(split[1] , float.Parse(split[2]));
            }
        }
    }
    bool CheckMaxLevel(){
        if(soulLevel >= 12) return true;
        else return false;
    }
    public UnitData GetUnitData(){
        return unitData;
    }
    public SoulsInfo GetSoulsInfo(){
        return this;
    }
}