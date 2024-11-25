using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsExplainTab : MonoBehaviour
{
    private  SoulsInfo _UnitData;
    public SoulsInfo UnitData {
        get {
            return _UnitData;
        }
        set {
            _UnitData = value;


        }
    }

    [SerializeField] Text SliderText;
    [SerializeField] Image Image;
    [SerializeField] Text explainText;
    [SerializeField] Text percentText;
    [SerializeField] Button levelUpButton;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;

    public static void SettingSoulExplainTab(){
        
    }
}
