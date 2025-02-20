using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMobDamageMeter : MonoBehaviour
{
    [SerializeField] Text maxHpText;
    [SerializeField] Text damageText;
    [SerializeField] Text cliticalText;
    [SerializeField] Text speedText;
    [SerializeField] Text damagePercentText;
    [SerializeField] Image mobImage;
    [SerializeField] Image damageFillImage;
    Summoner summoner;
    public string unitname;
    Unit unit;
    public static int maxDamage;
    int overlapDamage;
    public void Awake()
    {
        summoner = GameObject.FindObjectOfType<Summoner>();
        summoner.changeStatus += ChangeStatus;
    }
    
    public void Setting(Unit unit){
        this.unit = unit;
        unitname = unit.unit.name;
        if(unit == null) {
            Debug.LogError("unit null error");
            return;
        }
        Debug.Log(unit.overlapDamage);
        unit.overlapDamage += overlapDamage;
        
        Debug.Log(overlapDamage);
        mobImage.sprite = unit.unit.image;
        ChangeStatus(null);
    }

    public void Update()
    {
        if(maxDamage != 0) {
            damageFillImage.fillAmount = (float)unit.overlapDamage /  (float)maxDamage;
            
            damagePercentText.text = string.Format("{0} ({1:0.00}%)" , unit.overlapDamage , damageFillImage.fillAmount * 100);
            overlapDamage = unit.overlapDamage;
        }

    }

    public void ChangeStatus(Summoner summoner){
        maxHpText.text = $"{unit.maxHp} (+{(unit.hpPercent + unit.bonusHp) * 100f}%)";
        damageText.text = $"{unit.damage} (+{(unit.attackPrecent + unit.bonusAttack) * 100f}%)";
        cliticalText.text = $"{unit.clitical} (+{0}%)";
        speedText.text = $"{unit.speed} (+{(unit.bonusSpeed) * 100f}%)";
    }

    public Unit GetUnitData(){
        return unit;
    }
}
