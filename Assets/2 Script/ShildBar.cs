using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShildBar : MonoBehaviour
{
    public Unit target;
    public Text text;
    Slider slider;

    private void Start() {
        slider = GetComponent<Slider>();
        if(transform.GetComponentInChildren<Text>() != null) text = transform.GetComponentInChildren<Text>(); 
    }

    private void Update() {
        slider.maxValue = target.maxHp;
        slider.value = target.shild;
        if(text != null) text.text = slider.value.ToString();
    }
}
