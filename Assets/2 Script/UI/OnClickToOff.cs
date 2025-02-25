using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickToOff : MonoBehaviour
{    
    void Update()
    {
        if(Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
            gameObject.SetActive(false);
        }
    }
}
