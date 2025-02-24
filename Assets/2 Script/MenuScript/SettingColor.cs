using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingColor : MonoBehaviour
{
    Image backGroundImageColor;


    void Start()
    {
        
        backGroundImageColor = GetComponent<Image>();

        backGroundImageColor.color = GetComponent<IClassColor>().color.thisItemColor;
    }
}
