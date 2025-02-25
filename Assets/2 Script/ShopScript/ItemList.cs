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
    public SoulsInfo soulsInfo;
    public ReclicsInfo reclicsInfo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isSoldOut) {
            SoundManager.Instance.Play(SoundManager.SFX.DisOpen);
            return;
        }
        
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
        if(impossible.gameObject.activeSelf) SoundManager.Instance.Play(SoundManager.SFX.DisOpen);
        else SoundManager.Instance.Play(SoundManager.SFX.SelectItem);
        
    }

    public void Setting(ISellingAble sellingData , bool sellingGem , bool soldOut = false , SoulsInfo soulsInfo = null , ReclicsInfo reclicsInfo = null){
        this.soulsInfo = null;
        this.reclicsInfo = null;

        if(soldOut) SoldOut();

        soldOutImage.SetActive(soldOut);
        this.sellingGem = sellingGem;
        sellingAble = sellingData;

        if(soulsInfo != null) this.soulsInfo = soulsInfo;
        else this.reclicsInfo = reclicsInfo;

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
