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


            UnitData data = value.GetUnitData();

            hppercentText.text = "<color=red>" + data.curStat.hpStat + "%</color>";
            damagepercentText.text = "<color=yellow>" + data.curStat.attackStat + "% </color>";
            Image.sprite = data.image;
            explainText.text = data.explainText;

            SetSkill(data);
            SetCost(data);
        }
    }

    List<GameObject> skillImagesObject = new List<GameObject>();

    [SerializeField] Text SliderText;
    [SerializeField] Image Image;
    [SerializeField] Text cost;
    [SerializeField] Text explainText;
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


    [SerializeField] private GameObject skillExplainTab;
    private Text skillExplainText;
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

        skillExplainText = skillExplainTab.transform.GetChild(0).GetComponent<Text>();
        rect = skillExplainTab.GetComponent<RectTransform>();
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
                    skillExplain.SetSkillExplain(skillExplainTab, skillExplainText, rect);
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
                if(data.soulsSkillData[i].level <= UnitData.soulLevel) {
                    skillImagesObject[i].GetComponent<SkillExplain>().lockImage.SetActive(false);
                }
                
            }
        }

    }
    private void SetCost(UnitData data)
    {
        int costValue = (int)(data.classStruct.initCost * ((UnitData.soulLevel + 1)* data.classStruct.levelUpCost));
        
        Debug.Log(costValue);
        // 길이 구하고 길이 -2 까지의 밑의 값은 0으로 바꿔줄꺼임
        int copyCost = costValue;
        int length = 0;
        while(copyCost != 0) {
            length++;
            copyCost /= 10;
        }

        if(length - 2 >= 0) {
            costValue = costValue - costValue % (int)Math.Pow(10 , length - 2); 
        }

        cost.text = "<color=blue>" + costValue + "</color>";
    }
}
