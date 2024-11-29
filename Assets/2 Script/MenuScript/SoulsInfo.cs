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

    private void Awake() {
        unitData.classStruct = new ClassStruct(unitData.type);
        soulInintPercent = unitData.classStruct.soulInintPercent;
        soulLevelUpPercent = unitData.classStruct.soulLevelUpPercent;

        spawnProbabillity = unitData.typenumber;      
        Init();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //\\TODO lock 일때는 확인 X 풀려있어야 확인 가능하도록 해줘야함
        soulsTab.settingSoul(this);
    }
    private void Init(){
        color = unitData.classStruct;
        soulImage.sprite = unitData.image;
    }
    public void Setting(int soulCount , int soulLevel){
        if(soulCount == 0 && soulLevel == 0) return;

        this.soulCount = soulCount;
        this.soulLevel = soulLevel;
        soulMaxCount = soulLevel * 2;
        settingslider?.Invoke();

        levelText.text = soulLevel + "";
        lockObejct.SetActive(false);
        unLock = true;
    }
    public SoulsInfo LevelUp(){
        soulLevel++;
        soulCount -= soulMaxCount;
        soulMaxCount = soulLevel * 2;
        levelText.text = soulLevel + "";
        ChangeStatus();
        return this;
    }
    public void ChangeStatus(){
        GameData data = GameDataManger.Instance.GetGameData();
        data.soulsLevel[unitData.typenumber - 1] = soulLevel;
        data.soulsCount[unitData.typenumber - 1] = soulCount;
        GameDataManger.Instance.SaveData();
    }

    public UnitData GetUnitData(){
        return unitData;
    }
    
    public SoulsInfo GetSoulsInfo(){
        return this;
    }
}
