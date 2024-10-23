using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentSelect : MonoBehaviour
{
    [SerializeField] Text explnationText;
    [SerializeField] Text skillLevelText;
    [SerializeField] SkillData skilldata;

    Outline outline;

    int skillLevel;
    float skillCoolDown;
    bool _isSelect;
    bool waitTime = true;
    string initSKillText;
    public bool isSelect
    {
        get { return _isSelect; }
        set
        {
            _isSelect = value;
            if (_isSelect) outline.effectColor = new Color(1, 0, 0, 1);
            else outline.effectColor = new Color(1, 0, 0, 0);
        }
    }
    private void Awake()
    {
        initSKillText = skillLevelText.text;
        skillLevel = skilldata.skillLevel;
        skillCoolDown = skilldata.coolTime;
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            isSelect = true;
            explnationText.text = skilldata.skillExplain;
        }
        else isSelect = false;

    }

    public void UpgrdeTalent()
    {
        if(isSelect && Input.touchCount > 0 && waitTime) {
            if(Input.GetTouch(0).tapCount >= 2) {
                skillLevel++;
                skillLevelText.text = skillLevel + initSKillText;
            }
        }
    }
}
