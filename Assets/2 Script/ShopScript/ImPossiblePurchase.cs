using System;
using UnityEngine;
using UnityEngine.UI;

public class ImPossiblePurchase : MonoBehaviour
{

    [SerializeField] private Text text;
    [SerializeField] private Image lackgoodsImage;
    [SerializeField] private Button impossible_PuchaseBNT;
    [SerializeField] private Sprite[] sprites;
    private void Awake() {
        impossible_PuchaseBNT.onClick.AddListener(() => {
            gameObject.SetActive(false);
        });
    }

    public void Setting(float lackgoods , bool sellingGem){
        if(sellingGem) lackgoodsImage.sprite = sprites[0];
        else lackgoodsImage.sprite = sprites[1]; 
        text.text = String.Format("{0} 만큼 재화가 \n 부족합니다." , lackgoods);
    }
}
