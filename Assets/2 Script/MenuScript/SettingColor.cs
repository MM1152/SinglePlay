using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingColor : MonoBehaviour
{
    Image backGroundImageColor;
    ItemClass item;
    Color CommonColor = Color.gray;
    Color UnCommonColor = Color.blue;
    Color UniqueColor = new Color(0.8257728f , 0 , 0.8301887f);
    Color LegendColor = Color.yellow;

    void Start()
    {
        item = GetComponent<IClassColor>().itemClass;
        backGroundImageColor = GetComponent<Image>();
        
        switch(item) {
            case ItemClass.COMMON :
                backGroundImageColor.color = CommonColor;
                break;
            case ItemClass.UNCOMMON :
                backGroundImageColor.color = UnCommonColor;
                break;
            case ItemClass.UNIQUE :
                backGroundImageColor.color = UniqueColor;
                break;
            case ItemClass.LEGENDARY :
                backGroundImageColor.color = LegendColor;
                break;
        }
    }
}
