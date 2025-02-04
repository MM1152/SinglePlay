using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SoulsExplainTab : MonoBehaviour
{
    private EquipSouls equipSouls;
    private SoulsInfo _UnitData;
    public SoulsInfo UnitData
    {
        get
        {
            return _UnitData;
        }
        set
        {
            _UnitData = value;

            slider.maxValue = value.soulMaxCount;
            slider.value = value.soulCount;
            levelText.text = value.soulLevel + 1 + "";
            SliderText.text = value.soulCount + " / " + value.soulMaxCount;
            cost.text = "<color=blue>" + value.cost + "</color>";

            UnitData data = value.GetUnitData();

            hppercentText.text = "<color=red>" + data.curStat.hpStat + "%</color>";
            damagepercentText.text = "<color=yellow>" + data.curStat.attackStat + "% </color>";
            Image.sprite = data.image;

            
            initHpText.text = "<color=red>" + data.hp + "</color>" ;
            initDamageText.text = "<color=yellow>" + data.damage + "</color>" ;

            SetExplain(data);
            SetSkill(data);
        }
    }

    List<GameObject> skillImagesObject = new List<GameObject>();

    [SerializeField] Text SliderText;
    [SerializeField] Image Image;
    [SerializeField] Text cost;
    [SerializeField] Text hppercentText;
    [SerializeField] Text damagepercentText;
    [SerializeField] Button levelUpButton;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;
    [SerializeField] Button equipButton;
    [SerializeField] Button unEquipButton;

    /// <summary>
    /// Skill보여주는 프리팹
    /// </summary>
    [SerializeField] GameObject SkillImage;
    [SerializeField] Transform skillImageParent;
    [SerializeField] Text initHpText;
    [SerializeField] Text initDamageText;


    [SerializeField] private GameObject skillExplainTab;
    [SerializeField] private Text skillExplainText;
    [SerializeField] private Text skillName;
    [SerializeField] private Image skillImage;
    [SerializeField] private Transform levelPerAdditionalParent;
    private RectTransform rect;

    public Action SetEquip;
    public Action SetUnEquip;
    private void Awake()
    {
        levelUpButton.onClick.AddListener(LevelUp);
        equipButton.onClick.AddListener(() =>
        {
            EquipSouls.isEquip = true;
            SetEquip();
            this.gameObject.SetActive(false);
        });
        unEquipButton.onClick.AddListener(() =>
        {
            equipSouls.SetSoulInfo(null);
            GameManager.Instance.soulsInfo.Remove(UnitData.GetUnitData().name);
            this.gameObject.SetActive(false);
        });

        rect = skillExplainTab.GetComponent<RectTransform>();
    }
    private void OnDisable() {
        skillExplainTab.SetActive(false);
    }
    public void SettingSoulExplainTab(SoulsInfo soulsInfo, bool open_To_SoulTab, EquipSouls equip)
    {
        UnitData = soulsInfo;
        this.equipSouls = equip;
        equipButton.gameObject.SetActive(open_To_SoulTab);
        unEquipButton.gameObject.SetActive(!open_To_SoulTab);
    }
    void LevelUp()
    {
        if (UnitData.soulCount >= UnitData.soulMaxCount)
        {
            UnitData = UnitData.LevelUp();
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(Input.GetTouch(0).position, Camera.main.transform.forward))
            {
                SkillExplain skillExplain;
                if (hit.collider.TryGetComponent<SkillExplain>(out skillExplain))
                {
                    skillExplain.SetSkillExplain(skillExplainTab, skillExplainText, rect , skillName , skillImage);
                    skillExplainTab.transform.SetParent(transform);
                }
                else
                {
                    skillExplainTab.SetActive(false);
                }
            }
        }
    }
    private void SetSkill(UnitData data)
    {
        for (int i = 0; i < skillImagesObject.Count; i++)
        {
            skillImagesObject[i].SetActive(false);
        }

        if (data.soulsSkillData.Length > 0)
        {
            if (skillImagesObject.Count >= data.soulsSkillData.Length)
            {
                for (int i = 0; i < data.soulsSkillData.Length; i++)
                {
                    skillImagesObject[i].GetComponent<Image>().sprite = data.soulsSkillData[i].skillData.skillImages;
                    skillImagesObject[i].GetComponent<SkillExplain>().skillData = data.soulsSkillData[i].skillData;
                    skillImagesObject[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < skillImagesObject.Count; i++)
                {
                    skillImagesObject[i].GetComponent<Image>().sprite = data.soulsSkillData[i].skillData.skillImages;
                    skillImagesObject[i].GetComponent<SkillExplain>().skillData = data.soulsSkillData[i].skillData;
                    skillImagesObject[i].SetActive(true);
                }

                for (int i = skillImagesObject.Count; i < data.soulsSkillData.Length; i++)
                {
                    GameObject prefebSkill = Instantiate(SkillImage, skillImageParent);
                    prefebSkill.GetComponent<Image>().sprite = data.soulsSkillData[i].skillData.skillImages;
                    prefebSkill.GetComponent<SkillExplain>().skillData = data.soulsSkillData[i].skillData;
                    skillImagesObject.Add(prefebSkill);
                }
            }

            for(int i = 0 ; i < data.soulsSkillData.Length; i++) {
                if(data.soulsSkillData[i].level <= UnitData.soulLevel + 1) {
                    skillImagesObject[i].GetComponent<SkillExplain>().lockImage.SetActive(false);
                }
                else {
                    skillImagesObject[i].GetComponent<SkillExplain>().lockImage.SetActive(true);
                }
            }
        }

    }
    private void SetExplain(UnitData data){
        string[] splitText = data.explainText.Split("\n");
        
        for(int i = 0 ; i < splitText.Length; i++) {
            ReferenceLevelPerAdditional reference = levelPerAdditionalParent.GetChild(i).GetComponent<ReferenceLevelPerAdditional>();
            string[] splitLevel = splitText[i].Split(":");

            reference.levelText.text = splitLevel[0];
            reference.additionalText.text = splitLevel[1];
            
            if(_UnitData.soulLevel + 1 >= (i + 1) * 3) {
                reference.lockObejct.SetActive(false);
            }
            else {
                reference.lockObejct.SetActive(true);
            }
        }
    }
}
