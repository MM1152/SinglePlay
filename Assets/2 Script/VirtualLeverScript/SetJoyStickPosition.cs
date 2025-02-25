using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetJoyStickPosition : MonoBehaviour
{
    [SerializeField] VirtualJoyStick joyStick;

    void Start()
    {
        joyStick.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.touchCount >= 1)
        {
            RaycastHit2D ray = Physics2D.Raycast(Input.GetTouch(0).position, Camera.main.transform.forward);
            if (ray.collider != null)
            {
                if (ray.collider.gameObject == this.gameObject && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    joyStick.gameObject.SetActive(true);
                    joyStick.isInput = true;
                    joyStick.transform.position = Input.GetTouch(0).position;
                }

            }
        }
        if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            joyStick.gameObject.SetActive(false);
            joyStick.isInput = false;
        }
    }
}
