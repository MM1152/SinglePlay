using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingColor : MonoBehaviour
{
    Image backGroundImageColor;

    Color CommonColor = Color.gray;
    Color UnCommonColor = Color.blue;
    Color UniqueColor = new Color(0.8257728f , 0 , 0.8301887f);
    Color LegendColor = Color.yellow;

    void Start()
    {
        
        backGroundImageColor = GetComponent<Image>();

        backGroundImageColor.color = GetComponent<IClassColor>().color.thisItemColor;
    }
}
