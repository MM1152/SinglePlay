using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainTab : MonoBehaviour
{
    private ReclicsInfo _reclicsInfo;
    public ReclicsInfo reclicsInfo {
        set { 
            _reclicsInfo = value;

            levelText.text = value.GetReclicsLevel() + "";
            slider.maxValue = value.GetReclicsMaxCount();
            slider.value = value.GetReclicsCount();
            reclicsSliderText.text = value.GetReclicsCount() + "\t" + value.GetReclicsMaxCount() + "";


            ReclicsData data = _reclicsInfo.GetReclicsData();
            reclicsImage.sprite = data.image;
            explainText.text = data.reclicsExplain;
            percentText.text = data.inItPercent + (data.levelUpPercent * value.GetReclicsLevel()) + " ";
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
