using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class PossiblePurchase : MonoBehaviour
{
    private ISellingAble sellingData;
    [SerializeField] private Text text;
    [SerializeField] private Image image;
    [SerializeField] private Button puchaseBNT;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject success;
    
    Action soldOut;

    bool sellingGem;
    private void Awake()
    {
        puchaseBNT.onClick.AddListener(() => Purchase());
    }

    private void Purchase()
    {
        if (sellingData != null)
        {
            switch (sellingData.saveDataType)
            {
                case "Soul":
                    SoulsManager.Instance.soulsInfos[sellingData.saveDatanum].GetSoul();
                    break;
                case "Reclics":
                    ReclicsManager.Instance.reclicsDatas[sellingData.saveDatanum].PickUp();
                    break;
            }
            if(sellingGem) GameDataManger.Instance.GetGem(-sellingData.classStruct.gemCost);
            else GameDataManger.Instance.GetSoul(-sellingData.classStruct.soulCost);
            
        }
        
        
        gameObject.SetActive(false);
        success.SetActive(true);
        SoundManager.Instance.Play(SoundManager.SFX.BuyItem);
        DailyQuestTab.ClearDailyQuest(QuestType.BuyShop , 1);
        soldOut?.Invoke();

        sellingData = null;
    }

    public void Setting(ISellingAble data, bool sellingGem, Action callback)
    {
        sellingData = data;
        this.sellingGem = sellingGem;
        if (this.sellingGem)
        {
            text.text = String.Format("{0} 에\n구매하십니까?", data.classStruct.gemCost);
            image.sprite = sprites[0];
        }
        else
        {
            text.text = String.Format("{0} 에\n구매하십니까?", data.classStruct.soulCost);
            image.sprite = sprites[1];
        }
        soldOut = callback;
    }
    public void Setting(Action callback, int cost)
    {
        text.text = String.Format("{0} 에\n구매하십니까?", cost);
        image.sprite = sprites[0];

        soldOut = callback;
    }
}
