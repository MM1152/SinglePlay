using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Event : MonoBehaviour , IPointerClickHandler , IPointerDownHandler
{
    Action PointerClick = null;
    Action PointerDown = null;

    public void SetClickAction(Action action){
        PointerClick -= action;
        PointerClick += action;
    }

    public void SetDownAction(Action action){
        PointerDown -= action;
        PointerDown += action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke();
    }
}
