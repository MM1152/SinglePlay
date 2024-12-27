using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulSlider : MonoBehaviour
{
    Slider slider;
    SoulsInfo soul;
    [SerializeField] Text countText; 
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
        
        slider.maxValue = soul.soulMaxCount;
        slider.value = soul.soulCount;
        countText.text = "" + soul.soulCount + "/" + soul.soulMaxCount; 
    }
}
