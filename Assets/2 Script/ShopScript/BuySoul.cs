using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuySoul : MonoBehaviour , IPointerClickHandler , ISellingAble
{
    [SerializeField] private int cost;
    [SerializeField] private int reward;
    [SerializeField] private Sprite sprite;
    [SerializeField] private PossiblePurchase possibleBuyItem;
    public Sprite image { get ; set ; }
    public ClassStruct classStruct { get ; set; }
    public string saveDataType { get; set; }
    public int saveDatanum { get; set; }
    private void Awake() {
        image = sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(GameDataManger.Instance.GetGameData().gem >= cost) {
            possibleBuyItem.Setting(GetSoul , cost);
            possibleBuyItem.gameObject.SetActive(true);
        }
    }
    private void GetSoul(){
        GameDataManger.Instance.GetGameData().gem -= cost;
        GameDataManger.Instance.GetSoul(reward);
    }
}
