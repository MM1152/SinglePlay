using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulsTab : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] UnitData image;

    [Space(50)]
    [Header("직접 설정")]
    [SerializeField] Image soulImage;
    [SerializeField] Text levelText;
    [SerializeField] Slider slider;
    [SerializeField] GameObject lockObejct;
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
