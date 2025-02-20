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
    [SerializeField] Image mobImage;
    
    Summoner summoner;
    Unit unit;
    public void Awake()
    {
        summoner = GameObject.FindObjectOfType<Summoner>();
        summoner.changeStatus += ChangeStatus;
    }

    public void Setting(Unit unit){
        this.unit = unit;
        if(unit == null) {
            Debug.LogError("unit null error");
            return;
        }
        mobImage.sprite = unit.unit.image;
        ChangeStatus(null);
    }

    public void ChangeStatus(Summoner summoner){
        maxHpText.text = $"{unit.maxHp} (+{unit.hpPercent + unit.bonusHp} % )";
        damageText.text = $"{unit.damage} (+{unit.attackPrecent + unit.bonusAttack} % )";
        cliticalText.text = $"{unit.clitical} (+{0} % )";
        speedText.text = $"{unit.speed} (+{unit.bonusSpeed} % )";
        
    }
}
