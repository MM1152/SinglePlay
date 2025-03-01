using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickToNext : MonoBehaviour , IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(Input.touchCount);
        Debug.Log(Input.GetTouch(0).phase);
        
        if(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
            int sibling = transform.GetSiblingIndex();
            if(sibling == 26) {
                GameManager.Instance.isPlayingTutorial = false;
                GameDataManger.Instance.GetGameData().tutorial = true;
                GameDataManger.Instance.SaveData();
                gameObject.SetActive(false);
                return;
            }
            GameManager.Instance.StartTutorial(sibling + 1);
        }
    }
}
