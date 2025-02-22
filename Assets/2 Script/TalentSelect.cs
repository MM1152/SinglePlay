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
    TalentSelect parentSkill;
    Button button;
    Image backGroundImage;
    public int skillLevel;
    public float skillCoolDown;
    bool _unLock;
    bool unLock {
        get { return _unLock; }
        set {
            _unLock = value;
            if(_unLock) backGroundImage.color = Color.white;
            else backGroundImage.color = Color.gray;
            /*
            if(_unLock) button.interactable = true;
            else button.interactable = false;
            */
        }
    }

    bool _isSelect;
    public bool isSelect
    {
        get { return _isSelect; }
        set
        {
            _isSelect = value;
            if (_isSelect && unLock) outline.effectColor = new Color(1, 0, 0, 1);
            else outline.effectColor = new Color(1, 0, 0, 0);
        }
    }
    private void Awake()
    {
        if(transform.parent.GetComponent<TalentSelect>()) parentSkill = transform.parent.GetComponent<TalentSelect>();
        

        skillLevel = 0;
        skillCoolDown = skilldata.coolTime;
        outline = GetComponent<Outline>();
        button = GetComponent<Button>();
        backGroundImage = GetComponent<Image>();

        unLock = parentSkill == null ? true : false;
        skillLevelText.text = skillLevel + " / " + skilldata.maxSkillLevel;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            isSelect = true;
            explnationText.text = skilldata.skillExplain;
        }
        else isSelect = false;

        if(parentSkill != null && !unLock) {
            if(parentSkill.skillLevel == parentSkill.skilldata.maxSkillLevel) unLock = true; 
        }
    }

    public void UpgrdeTalent()
    {
        if(isSelect && Input.touchCount > 0) {
            if(unLock && skillLevel != skilldata.maxSkillLevel && Input.GetTouch(0).tapCount >= 2 && SkillManager.Instance.statPoint > 0) {
                skillLevelText.text = ++skillLevel + " / " + skilldata.maxSkillLevel;
                SkillManager.Instance.UnLockSkill(skilldata);
                RewardManager.Instance.SetSummonerStat.Invoke("None" , 0);
                //ReachMaxSkillLevel(skillLevel);
            }
        }
    }

    public void ReachMaxSkillLevel(int skillLevel){
        if(skillLevel == skilldata.maxSkillLevel) {
            button.interactable = false;
        }
    }
}
