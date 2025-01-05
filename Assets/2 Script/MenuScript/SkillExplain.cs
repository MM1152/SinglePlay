using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillExplain : MonoBehaviour

{
    public SoulsSkillData skillData;
    public string skillText;
    public GameObject lockImage;
    private void OnEnable() {
        skillText = skillData.skillExplainText;
        ChangeWord("skillInitPercent");
        ChangeWord("skillCoolTime");
    }
    public void SetSkillExplain(GameObject explainTab , Text text , RectTransform rect , Text skillName , Image skillImage){
        explainTab.transform.SetParent(transform);
        rect.anchoredPosition = Vector2.zero + new Vector2(0f , 100f);
        text.text = skillText;
        skillName.text = skillData.skillName;
        skillImage.sprite = skillData.skillImages;
        explainTab.SetActive(true);
    }
    private void ChangeWord(string word){ 
        int first = skillData.skillExplainText.IndexOf(word);
        if(first != -1){
            skillText = skillText.Replace(word , skillData.GetType().GetField(word).GetValue(skillData).ToString());
        } 
    }
}
