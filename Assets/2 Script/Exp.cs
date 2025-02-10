using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    private Slider slider;
    private Level level;
    private float bonus = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        if(GameDataManger.Instance.GetGameData().reclicsCount[11] > 0 || GameDataManger.Instance.GetGameData().reclicsLevel[11] > 0) {
            bonus += GameManager.Instance.reclicsDatas[11].inItPercent;
            bonus += GameDataManger.Instance.GetGameData().reclicsLevel[11] * GameManager.Instance.reclicsDatas[11].levelUpPercent;
            bonus /= 100f;
            bonus += 1f;
        }
        level = GameObject.FindObjectOfType<Level>();
        slider = GetComponent<Slider>();
        slider.maxValue = 100;
        slider.value = 0;
    }

    public void SetExpValue(float value){
    
        Debug.Log("Get Exp " + "(" + value + ")" + "(" + bonus + ")");
        
        slider.value += value * bonus;

        if(slider.value >= slider.maxValue) {
            slider.value = 0;
            level.LevelUp();
            slider.maxValue = slider.maxValue * 1.2f;
        }
    }

}
