using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillExplain : MonoBehaviour , IPointerClickHandler 

{
    [SerializeField] GameObject ExplainTab;
    private RectTransform explainTabRect;
    private Text text;
    private bool isOpen;
    public SoulsSkillData skillData;
    public string enemy1;
    public string enemy2;
    
    private void OnEnable() {
        enemy1 = $"공격시 {skillData.skillInitPercent}% 확률로 가까운 적 3명을 공격합니다.";
        enemy2 = $"{skillData.skillCoolTime} 주변 50px 의 적을 도발합니다.";
        isOpen = false;
        explainTabRect.anchoredPosition = new Vector2(50f , -50f);
    }
    private void OnDisable() {
        
    }
    public void Awake(){
        ExplainTab = transform.parent.parent.GetComponent<SoulsExplainTab>().skillExplain;
        explainTabRect = ExplainTab.GetComponent<RectTransform>();
        text = ExplainTab.transform.GetChild(0).GetComponent<Text>();
    }
    private void Update() {
        ExplainTab.SetActive(isOpen);
        if(Input.touchCount > 0) {
            RaycastHit2D hit;
            if(hit = Physics2D.Raycast(Input.GetTouch(0).position , Camera.main.transform.forward)) {
                if(hit.collider.gameObject != gameObject){
                    isOpen = false;
                    ExplainTab.transform.SetParent(transform.parent.parent);   
                }
            }
            else {
                ExplainTab.transform.SetParent(transform);
            }
        }    
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isOpen = true;
        text.text = enemy1;
    }
    //\\TODO 다른데 클릭하면 없애줘야됌
}
