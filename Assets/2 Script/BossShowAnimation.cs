using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossShowAnimation : MonoBehaviour
{
    [SerializeField] Text BossName;
    [SerializeField] Image ShowBossImage;
    [SerializeField] Image SliderBossImage;
    [SerializeField] Slider slider;
    
    Animator ani;
    private void Awake() {
        ani = ShowBossImage.GetComponent<Animator>();
    }
    public Sprite setBossImage {
        set { 
            ShowBossImage.sprite = value;
            SliderBossImage.sprite = value;
        }
    }

    public string setBossName {
        set {
            BossName.text = value;
        }
    }

    public Unit setBossData {
        set {
            slider.GetComponent<Hpbar>().target = value;
            slider.gameObject.SetActive(true);
        }
    }

    public void SetAnimation(bool value){
        transform.GetChild(0).gameObject.SetActive(value);
        transform.GetChild(1).gameObject.SetActive(value);
        
    }

    public bool IsPlayAnimation(){
        return ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
    }
}
