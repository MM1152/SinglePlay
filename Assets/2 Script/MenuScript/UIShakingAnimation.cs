using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShakingAnimation : MonoBehaviour
{
    Image image;
    RectTransform rect;
    EquipSouls equipSouls;

    bool playingAnimation;
    private void Awake() {
        equipSouls = GetComponent<EquipSouls>();
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }
    private void Update() {
        if(EquipSouls.isEquip && !playingAnimation){
            playingAnimation = true;
            StartCoroutine(WaitForEquipCorutine());
        }
        
    }
    IEnumerator WaitForEquipCorutine() {
        float rotation = 0f;
        bool turnleft = false;
        float count = 3f;

        while(EquipSouls.isEquip) {
            Debug.Log("play corutine");
            if(rotation >= 5f) turnleft = true;
            else if(rotation <= -5f) turnleft = false;

            if(turnleft) count = -1f;
            else count = 1f;

            rotation += count;
            rect.rotation = Quaternion.Euler(0 , 0 , rotation);

            yield return new WaitForSeconds(0.01f);
        }
        rect.rotation = Quaternion.Euler(0,0,0);
        playingAnimation = false;
    }
}
