using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemList : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] Image sellingImage;
    [SerializeField] Image goodsTypeImage;
    [SerializeField] Text goodsCostText;
    [SerializeField] PossiblePurchase possible;
    [SerializeField] ImPossiblePurchase impossible;
    [SerializeField] Sprite[] goodsImages; // 0 : soul , 1 : gem
    [SerializeField] GameObject soldOutImage;

    private bool sellingGem;
    private bool isSoldOut;
    private ISellingAble sellingAble;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isSoldOut) return;

        bool isOpen = false;
        int lackgoods = 0;
        if(sellingGem) {
            int gem = GameDataManger.Instance.GetGameData().gem;
            if(gem >= sellingAble.classStruct.gemCost) {
                possible.Setting(sellingAble , sellingGem , SoldOut);
                isOpen = true;
            }
            else {
                isOpen = false;
                lackgoods = sellingAble.classStruct.gemCost - gem;
                impossible.Setting(lackgoods , sellingGem);
            }
        }
        else {
            int soul = GameDataManger.Instance.GetGameData().soul;
            if(soul >= sellingAble.classStruct.soulCost) {
                possible.Setting(sellingAble , sellingGem , SoldOut);
                isOpen = true;
            }
            else {
                isOpen = false;
                lackgoods = sellingAble.classStruct.soulCost - soul;
                impossible.Setting(lackgoods , sellingGem);
            }
        }
        
        possible.gameObject.SetActive(isOpen);
        impossible.gameObject.SetActive(!isOpen);
        
    }

    public void Setting(ISellingAble sellingData , bool sellingGem , bool soldOut = false){
        if(soldOut) SoldOut();
        soldOutImage.SetActive(soldOut);
        this.sellingGem = sellingGem;
        sellingAble = sellingData;

        sellingImage.sprite = sellingData.image;
        
        if(sellingGem) {
            goodsTypeImage.sprite = goodsImages[0];
            goodsCostText.text = sellingData.classStruct.gemCost.ToString();
        } 
        else {
            goodsTypeImage.sprite = goodsImages[1];
            goodsCostText.text = sellingData.classStruct.soulCost.ToString();
        }

    }

    public void SoldOut(){
        soldOutImage.SetActive(true);
        isSoldOut = true;
        GameDataManger.Instance.GetGameData().soldOutItem[transform.GetSiblingIndex()] = true;
        GameDataManger.Instance.SaveData();
    }
}
