using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour
{
    [SerializeField] Image sellingImage;
    [SerializeField] Image goodsTypeImage;
    [SerializeField] Text goodsCostText;

    [SerializeField] Sprite[] goodsImages; // 0 : soul , 1 : gem
    public void Setting(ISellingAble sellingData , bool sellingGem){
        sellingImage.sprite = sellingData.image;

        if(sellingGem) {
            goodsTypeImage.sprite = goodsImages[1];
            goodsCostText.text = sellingData.classStruct.gemCost.ToString();
        } 
        else {
            goodsTypeImage.sprite = goodsImages[0];
            goodsCostText.text = sellingData.classStruct.soulCost.ToString();
        }
    }
}
