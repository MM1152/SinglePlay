using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowItemInfomation : MonoBehaviour , IPointerClickHandler 
{
    [SerializeField] ReclisExplainTab reclisExplainTab;
    [SerializeField] SoulsExplainTab soulsExplainTab;
    private ItemList parent;
    void Awake()
    {
        parent = transform.parent.GetComponent<ItemList>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(parent.soulsInfo != null) soulsExplainTab.OpenToShop(parent.soulsInfo);
        else reclisExplainTab.OpenToShop(parent.reclicsInfo);
    }

}
