using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpSliderEffect : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    int target; // 0 Reclics , 1 Soul
    int targetLength;

    [SerializeField] private Transform backGround;
    [SerializeField] private RectTransform[] pickUp;
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject showSpawnPosibility;
    ReclicsPickUp reclicsPickUp;
    SoulPickUp soulPickUp;

    float sliderPos;

    private List<Image> images = new List<Image>();
    private void Awake()
    {
        target = 0;
        targetLength = pickUp.Length;
        reclicsPickUp = GetComponent<ReclicsPickUp>();
        soulPickUp = GetComponent<SoulPickUp>();
        foreach (Transform child in backGround)
        {
            images.Add(child.GetComponent<Image>());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        sliderPos = eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.position.x - sliderPos < 0)
        {
            if (targetLength - 1 > target)
            {
                target++;
                StartCoroutine(WaitForMove(target - 1, target, true));
                images[target - 1].color = Color.white;
                images[target].color = Color.gray;
            }
        }
        else
        {
            if (0 < target)
            {
                target--;
                StartCoroutine(WaitForMove(target + 1, target, false));
                images[target + 1].color = Color.white;
                images[target].color = Color.gray;
            }
        }

    }

    IEnumerator WaitForMove(int outObject, int inObject, bool moveLeft)
    {
        showSpawnPosibility.SetActive(false);
        if (moveLeft)
        {
            for (float i = 0f; i <= 1400f; i += 100)
            {
                pickUp[outObject].anchoredPosition = new Vector3(-i, -87f, 0f);
                pickUp[inObject].anchoredPosition = new Vector3(1400f - i, -87f, 0f);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (float i = 0f; i <= 1400f; i += 100)
            {
                pickUp[outObject].anchoredPosition = new Vector3(i, -87f, 0f);
                pickUp[inObject].anchoredPosition = new Vector3(-1400f + i, -87f, 0f);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (eventData.pointerCurrentRaycast.gameObject == icon)
        {
            int count = 4;
            if (target == 0)
            {
                foreach(string itemclass in Enum.GetNames(typeof(ItemClass))){
                    if(reclicsPickUp.showPosibilityData.ContainsKey(itemclass)) {
                        showSpawnPosibility.transform.GetChild(count--).GetChild(0).GetComponent<Text>().text = string.Format("{0:F2} %" , reclicsPickUp.showPosibilityData[itemclass] * 100f );
                    }
                    else {
                        showSpawnPosibility.transform.GetChild(count--).GetChild(0).GetComponent<Text>().text = "0 %";
                    }
                }
            }
            if(target == 1) 
            {
                foreach(string itemclass in Enum.GetNames(typeof(ItemClass))){
                    if(soulPickUp.showPosibilityData.ContainsKey(itemclass)) {
                        showSpawnPosibility.transform.GetChild(count--).GetChild(0).GetComponent<Text>().text = string.Format("{0:F2} %" , soulPickUp.showPosibilityData[itemclass] * 100f );
                    }
                    else{
                        showSpawnPosibility.transform.GetChild(count--).GetChild(0).GetComponent<Text>().text = "0 %";
                    } 
                }
            }

            showSpawnPosibility.SetActive(true);

        }
        else
        {
            showSpawnPosibility.SetActive(false);
        }
    }

}
