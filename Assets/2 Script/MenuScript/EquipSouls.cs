using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class EquipSouls : MonoBehaviour , IPointerEnterHandler
{
    private SoulsInfo _soulInfo;
    SoulsInfo soulsInfo
    {
        set
        {
            if(value == null) {
                _soulInfo = null;
                soulImage.sprite = null;
                return;
            }
            _soulInfo = value;
            soulImage.sprite = value.GetUnitData().image;
            //\\ Image 넣어주기 , 해당하는 칸 클릭시 해당하는 소울 상태창 뜨게 만들어주기
        }
    }
    [SerializeField] Image soulImage;
    [SerializeField] SoulsTab soulsTab;
    public static bool isEquip;
    
    private void Awake()
    {
        isEquip = false;
    }
    public void SetSoulInfo(SoulsInfo soulsInfo)
    {
        this.soulsInfo = soulsInfo;
    }
    public SoulsInfo GetSoulInfo()
    {
        return _soulInfo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetTouch(0).phase == TouchPhase.Began && !isEquip) soulsTab.settingSoul(_soulInfo);
    }

}
