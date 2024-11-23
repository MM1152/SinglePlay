using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingClassToColor : MonoBehaviour
{
    Image backGroundImageColor;
    ItemClass item;
    Color CommonColor = Color.gray;
    Color UnCommonColor = Color.blue;
    Color UniqueColor = new Color(0.8257728f , 0 , 0.8301887f);
    Color LegendColor = Color.yellow;
    private void Awake() {
        item = GetComponent<IClassColor>().itemClass;
        backGroundImageColor = GetComponent<Image>();
    }
    void Start()
    {
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
