using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoulsExplainTab : MonoBehaviour
{
    
    private SoulsInfo _UnitData;
    public SoulsInfo UnitData {
        get {
            return _UnitData;
        }
        set {
            _UnitData = value;

            slider.maxValue = value.soulMaxCount;
            slider.value = value.soulCount;
            levelText.text = value.soulLevel + "";
            SliderText.text = value.soulCount + " / " + value.soulMaxCount;

            hppercentText.text = "<color=red>"+value.soulInintPercent +"%</color>";
            damagepercentText.text = "<color=yellow>" + value.soulInintPercent + "% </color>";

            UnitData data = value.GetUnitData();
            
            Image.sprite = data.image;
            explainText.text = data.explainText;
            
            //스킬 이미지 꺼놓기
            for(int i = 0 ; i < skillImagesObject.Count; i++) {
                skillImagesObject[i].SetActive(false);
            }

            if(data.soulsSkillData.Length > 0) {
                if(skillImagesObject.Count >= data.soulsSkillData.Length) {
                    for(int i = 0 ; i < data.soulsSkillData.Length; i++) {
                        skillImagesObject[i].GetComponent<Image>().sprite = data.soulsSkillData[i].skillImages;
                        skillImagesObject[i].GetComponent<SkillExplain>().skillData = data.soulsSkillData[i];
                        skillImagesObject[i].SetActive(true);
                    }
                }
                else {
                    for(int i = 0; i < skillImagesObject.Count; i++) {
                        skillImagesObject[i].GetComponent<Image>().sprite = data.soulsSkillData[i].skillImages;
                        skillImagesObject[i].GetComponent<SkillExplain>().skillData = data.soulsSkillData[i];
                        skillImagesObject[i].SetActive(true);
                    }

                    for(int i = skillImagesObject.Count; i < data.soulsSkillData.Length; i++){
                        GameObject prefebSkill = Instantiate(SkillImage , skillImageParent);
                        prefebSkill.GetComponent<Image>().sprite = data.soulsSkillData[i].skillImages;
                        prefebSkill.GetComponent<SkillExplain>().skillData = data.soulsSkillData[i];
                        skillImagesObject.Add(prefebSkill);
                    }
                }
            }
            //\\TODO 스킬 이미지가 존재한다면 스킬이미지 넣어주고 스킬 클릭하면 어떤 효과인지 확인 가능하게 해줘야함
        }
    }

    List<GameObject> skillImagesObject = new List<GameObject>();

    [SerializeField] Text SliderText;
    [SerializeField] Image Image;
    [SerializeField] Text explainText;
    [SerializeField] Text hppercentText;
    [SerializeField] Text damagepercentText;
    [SerializeField] Button levelUpButton;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;
    [SerializeField] Button equipButton;
    /// <summary>
    /// Skill보여주는 프리팹
    /// </summary>
    [SerializeField] GameObject SkillImage;
    [SerializeField] Transform skillImageParent;
    public GameObject skillExplain;
    public Action SetEquip;
    private void Awake() {
        levelUpButton.onClick.AddListener(LevelUp);
        equipButton.onClick.AddListener(() => {
            EquipSouls.isEquip = true;
            SetEquip();
            this.gameObject.SetActive(false);
        });
    }
    public void SettingSoulExplainTab(SoulsInfo soulsInfo){
        UnitData = soulsInfo;
    }
    void LevelUp(){
        if(UnitData.soulCount >= UnitData.soulMaxCount) {
            UnitData = UnitData.LevelUp();
        }
    }
    private void Update() {
        
    }
}
