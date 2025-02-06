using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReclicsSlider : MonoBehaviour
{
    Slider slider;
    ReclicsInfo reclics;
    [SerializeField] Text countText; 
    [SerializeField] Image levelUpImage;
    // Start is called before the first frame update`
    void Awake()
    {
        slider = GetComponent<Slider>();
        reclics = transform.parent.GetComponent<ReclicsInfo>();
        reclics.setSlider += Setting;
        gameObject.SetActive(false);
    }

    public void Setting(){
        gameObject.SetActive(true);

        if(reclics.GetReclicsLevel() >= 12) {
            countText.text = "MAX";
            slider.value = slider.maxValue;
            levelUpImage.gameObject.SetActive(false);
            return;
        }

        slider.maxValue = reclics.GetReclicsMaxCount();
        slider.value = reclics.GetReclicsCount();
        
        if(reclics.GetReclicsMaxCount() <= reclics.GetReclicsCount()) levelUpImage.gameObject.SetActive(true);
        else levelUpImage.gameObject.SetActive(false);

        countText.text = "" + reclics.GetReclicsCount() + " / " + reclics.GetReclicsMaxCount(); 
    }
}
