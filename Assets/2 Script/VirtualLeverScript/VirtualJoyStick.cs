using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System.Runtime.CompilerServices;

public class VirtualJoyStick : MonoBehaviour 
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

    public void ControllJoyStickLever(Vector2 position) {
        var inputDir = position - (Vector2) rectTransform.position;
        var clampDir = inputDir.magnitude > leverRange ? inputDir.normalized * leverRange : inputDir;

        lever.anchoredPosition = clampDir;
        inputVector = clampDir.normalized;
    } 
    
    private void Update() {
        if(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            ControllJoyStickLever(Input.GetTouch(0).position);
        }
        if(isInput) controllObject.Move(inputVector);
    }
}
