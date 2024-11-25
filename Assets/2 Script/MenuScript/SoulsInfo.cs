using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public delegate void SettingSlider();

[DefaultExecutionOrder(0)]
public class SoulsInfo : MonoBehaviour , IPointerClickHandler , ISpawnPosibillity , IClassColor
{
    [SerializeField] UnitData unitData;
    [Space(50)]
    [Header("직접 설정")]
    [SerializeField] Image soulImage;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;
    [SerializeField] GameObject lockObejct;

    public int soulLevel {private set; get;}
    public int soulCount {private set; get;}
    public int soulMaxCount {private set; get;}

    public float spawnProbabillity { get ; set ; }
    public ItemClass itemClass { get ; set ; }

    public SettingSlider settingslider;
    private void Awake() {
        spawnProbabillity = unitData.typenumber;      
        Init();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    private void Init(){
        itemClass = unitData.type;
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

    }

    
    
    public SoulsInfo GetSoulsInfo(){
        return this;
    }
}
