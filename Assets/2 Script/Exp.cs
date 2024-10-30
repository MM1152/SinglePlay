using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    private Slider slider;
    private Level level;
    // Start is called before the first frame update
    void Awake()
    {
        level = GameObject.FindObjectOfType<Level>();
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 0;
    }

    public void SetExpValue(float value){
        slider.value += value;
        if(slider.value >= slider.maxValue) {
            slider.value = 0;
            level.LevelUp();
            slider.maxValue = slider.maxValue * 1.2f;
        }
    }

}
