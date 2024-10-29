using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hpbar : MonoBehaviour
{
    [SerializeField] Summoner player;
    Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
        slider.maxValue = player.unit.hp;
        slider.value = slider.maxValue;
    }

    private void Update() {
        slider.value = player.getHp();
    }
}
