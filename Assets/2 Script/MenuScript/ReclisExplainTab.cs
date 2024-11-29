using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReclisExplainTab : MonoBehaviour
{
    
    private ReclicsInfo _reclicsInfo;
    public ReclicsInfo reclicsInfo {
        set { 
            _reclicsInfo = value;
            levelText.text = value.GetReclicsLevel() + "";
            reclicsSliderText.text = value.GetReclicsCount() + "\t" + value.GetReclicsMaxCount() + "";
            slider.maxValue = value.GetReclicsMaxCount();
            slider.value = value.GetReclicsCount();
            


            ReclicsData data = _reclicsInfo.GetReclicsData();
            reclicsImage.sprite = data.image;
            explainText.text = data.reclicsExplain;
            percentText.text = data.inItPercent + (data.levelUpPercent * value.GetReclicsLevel() - 1) + " ";
            percentText.text += "<color=green> (+ "+data.levelUpPercent +")</color> %";
            
        }
    }
    [SerializeField] Text reclicsSliderText;
    [SerializeField] Image reclicsImage;
    [SerializeField] Text explainText;
    [SerializeField] Text percentText;
    [SerializeField] Button levelUpButton;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;

    private void Awake() {
        levelUpButton.onClick.AddListener(ReclicsLevelUp);

    }
    private void Update() {
       if(_reclicsInfo.GetReclicsCount() >= _reclicsInfo.GetReclicsMaxCount()) {
            levelUpButton.interactable = true;
       }
       else {
            levelUpButton.interactable = false;
       }
    }
    private void ReclicsLevelUp(){
        reclicsInfo = _reclicsInfo.LevelUp();
    }
    public void SetReclicsData(ReclicsInfo data){
        reclicsInfo = data;
    }
}
