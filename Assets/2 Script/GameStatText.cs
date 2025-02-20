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
        if(GameManager.Instance.currentStage % 10 == 0) {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
        else {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        slider.value = GameManager.Instance.clearMonseter;  
        clearMonsetertext.text = GameManager.Instance.clearMonseter + " / " + maxClearCount;
    }
}
