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
    bool isTutorial;
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
        
        if(!isTutorial && GameManager.Instance.isPlayingTutorial) {
            isTutorial = true;
            level.LevelUp();
            GameManager.Instance.StartTutorial(7);
        }
        slider.value += value * bonus;

        if(slider.value >= slider.maxValue) {
            slider.value = 0;
            level.LevelUp();
            slider.maxValue = slider.maxValue * 1.2f;
        }
    }

}
