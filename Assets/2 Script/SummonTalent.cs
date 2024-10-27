using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummonTalent : MonoBehaviour
{
    public Unit summonUnit;
    public TalentSelect talent;
    private Button bnt;
    private void Awake() {
        talent = GetComponent<TalentSelect>();
        bnt = GetComponent<Button>();
        bnt.onClick.AddListener(() => AddSummonUnit());
    }

    public void AddSummonUnit(){
        if(talent.isSelect && Input.touchCount > 0) {
            if(Input.GetTouch(0).tapCount >= 2) {
                if(talent.skillLevel == 1) SkillManager.Instance.AddSummonsUnit(summonUnit);
            }
        }
    }
}
