using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    public Unit player;
    public Text text;
    Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
        slider.maxValue = player.unit.hp;
        slider.value = slider.maxValue;
        if(transform.GetComponentInChildren<Text>() != null) text = transform.GetComponentInChildren<Text>(); 
    }

    private void Update() {
        slider.value = player.hp;
        if(text != null) text.text = slider.value.ToString();
    }
}
