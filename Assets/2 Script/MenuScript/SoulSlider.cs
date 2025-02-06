using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulSlider : MonoBehaviour
{
    Slider slider;
    SoulsInfo soul;
    [SerializeField] Text countText; 
    [SerializeField] Image levelUpImage;
    // Start is called before the first frame update`
    void Awake()
    {
        slider = GetComponent<Slider>();
        soul = transform.parent.GetComponent<SoulsInfo>();
        soul.settingslider += Setting;
        gameObject.SetActive(false);

    }

    public void Setting(){
        gameObject.SetActive(true);
        if(soul.soulLevel >= 12) {
            countText.text = "MAX";
            slider.value = slider.maxValue;
            return;
        }
        slider.maxValue = soul.soulMaxCount;
        slider.value = soul.soulCount;

        if(soul.soulMaxCount <= soul.soulCount) levelUpImage.gameObject.SetActive(true);
        else levelUpImage.gameObject.SetActive(false);
        
        countText.text = "" + soul.soulCount + " / " + soul.soulMaxCount; 
    }
}
