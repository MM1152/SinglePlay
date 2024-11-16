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
        
        slider.maxValue = reclics.GetReclicsMaxCount();
        slider.value = reclics.GetReclicsCount();
        countText.text = "" + reclics.GetReclicsCount() + "\t" + reclics.GetReclicsMaxCount(); 
    }
}
