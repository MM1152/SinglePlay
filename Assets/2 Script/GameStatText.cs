using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStatText : MonoBehaviour
{
    [SerializeField] Text clearMonsetertext;
    Slider slider;

    int maxClearCount;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start(){
        maxClearCount = GameManager.Instance.clearMonseter;
        slider.maxValue = maxClearCount;
    }
    void Update()
    {
        slider.value = GameManager.Instance.clearMonseter;  
        clearMonsetertext.text = GameManager.Instance.clearMonseter + " / " + maxClearCount;
    }
}
