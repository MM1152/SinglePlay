using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class VirtualJoyStick : MonoBehaviour , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    public static VirtualJoyStick instance {get ; private set; }
    
    [SerializeField] private RectTransform lever;
    [SerializeField] private Summoner controllObject;
    private RectTransform rectTransform;
    private Vector2 inputVector;
    public bool isInput;

    [SerializeField , Range(10f , 150f)]
    private float leverRange; // Lever가 바깥으로 나갈수있는 제한값
    private void Awake() {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        ControllJoyStickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControllJoyStickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    public void ControllJoyStickLever(PointerEventData eventData) {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampDir = inputDir.magnitude > leverRange ? inputDir.normalized * leverRange : inputDir;
        lever.anchoredPosition = clampDir;
        inputVector = clampDir.normalized;
    } 
    
    private void Update() {
        if(isInput) controllObject.Move(inputVector);
    }
}
