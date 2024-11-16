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
            ReclicsData data = _reclicsInfo.GetReclicsData();
            reclicsImage.sprite = data.image;
            explainText.text = data.reclicsExplain;
            percentText.text = data.inItPercent + " ";
            
            levelText.text = value.GetReclicsLevel() + "";
            slider.maxValue = value.GetReclicsMaxCount();
            slider.value = value.GetReclicsCount();
            percentText.text += "<color=green> (+ "+data.levelUpPercent +")</color> %";
        }
    }
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
