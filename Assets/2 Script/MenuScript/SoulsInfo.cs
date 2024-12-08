using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void SettingSlider();

[DefaultExecutionOrder(0)]
public class SoulsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity , IClassColor
{
    
    [Space(50)]
    [Header("직접 설정")]
    [SerializeField] SoulsTab soulsTab;
    [SerializeField] UnitData unitData;
    [SerializeField] Image soulImage;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;
    [SerializeField] GameObject lockObejct;

    
    public int soulLevel {private set; get;}
    public int soulCount {private set; get;}
    public int soulMaxCount {private set; get;}
    public float soulInintPercent {private set; get;}
    public float soulLevelUpPercent {private set; get;}



    public float spawnProbabillity { get ; set ; }
    public ClassStruct color { get ; set ; }

    public SettingSlider settingslider;
    bool unLock;
    bool[] applyStat;
    private void Awake() {
        unitData.classStruct = new ClassStruct(unitData.type);
        soulInintPercent = unitData.classStruct.soulInintPercent;
        soulLevelUpPercent = unitData.classStruct.soulLevelUpPercent;
        applyStat = new bool[unitData.stat.GetLength(0)];


        ChangeBonusStat();
        unitData.curStat.speedStat = unitData.classStruct.soulInintPercent;

        spawnProbabillity = unitData.typenumber;      
        Init();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(unLock) {
            soulsTab.settingSoul(this , true , null);
        }
    }
    private void Init(){
        color = unitData.classStruct;
        soulImage.sprite = unitData.image;
    }
    public void Setting(int soulCount , int soulLevel){
        if(soulCount == 0 && soulLevel == 0) return;

        this.soulCount = soulCount;
        this.soulLevel = soulLevel;
        soulMaxCount = (this.soulLevel + 1) * 2;
        
        settingslider?.Invoke();
        
        CheckLevel();

        ChangeBonusStat();
        levelText.text = this.soulLevel + 1 + "";
        lockObejct.SetActive(false);
        unLock = true;
    }
    public SoulsInfo LevelUp(){
        soulLevel++;
        soulCount -= soulMaxCount;
        soulMaxCount = soulLevel * 2;
        levelText.text = soulLevel + 1 + "";

        CheckLevel();
        ChangeBonusStat();
        ChangeStatus();

        settingslider?.Invoke();

        return this;
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
        GameDataManger.Instance.SaveData();
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
    public UnitData GetUnitData(){
        return unitData;
    }
    
    public SoulsInfo GetSoulsInfo(){
        return this;
    }
}
