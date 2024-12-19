
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSouls : MonoBehaviour , IPointerEnterHandler
{
    [SerializeField] private SoulsInfo _soulInfo;
    SoulsInfo soulsInfo
    {
        set
        {
            if(value == null) {
                _soulInfo = null;
                soulImage.sprite = null;
                soulImage.color = new Color(0 , 0 , 0 , 0);
                return;
            }
            _soulInfo = value;
            soulImage.sprite = value.GetUnitData().image;
            soulImage.color = new Color(1 , 1 , 1 , 1);
        }
    }
    [SerializeField] Image soulImage;
    [SerializeField] SoulsTab soulsTab;
    public static bool isEquip;
    
    private void Awake()
    {
        isEquip = false;
        soulsInfo = null;
    }
    public void SetSoulInfo(SoulsInfo soulsInfo)
    {
        if(_soulInfo != null)  SoulsManager.Instance.equipDic.Remove(_soulInfo);
        this.soulsInfo = soulsInfo;
        
        if(soulsInfo == null) {
            GameData data = GameDataManger.Instance.GetGameData();
            data.soulsEquip[transform.GetSiblingIndex()] = 0;
            GameDataManger.Instance.SaveData();
        }
        
        
        if(soulsInfo != null && !GameManager.Instance.soulsInfo.ContainsKey(soulsInfo.GetUnitData().name)){
            GameManager.Instance.soulsInfo.Add(soulsInfo.GetUnitData().name , soulsInfo.GetUnitData());
        }
    
    }
    public SoulsInfo GetSoulInfo()
    {
        return _soulInfo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isEquip) soulsTab.settingSoul(_soulInfo , false , this);
    }
    
}
