
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSouls : MonoBehaviour , IPointerEnterHandler
{
    // 배틀파워 저장위치필요
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
    [SerializeField] BattlePower battlePower;
    public static bool isEquip;

    public void Awake()
    {
        isEquip = false;
        battlePower = FindAnyObjectByType<BattlePower>();
    }
    public void SetSoulInfo(SoulsInfo soulsInfo)
    {
        if(_soulInfo != null)  {
            battlePower.SettingBattlePower(-_soulInfo.battlePower);
            SoulsManager.Instance.equipDic.Remove(_soulInfo);    
        }

        this.soulsInfo = soulsInfo;
        
        if(soulsInfo == null) {
            GameData data = GameDataManger.Instance.GetGameData();
            data.soulsEquip[transform.GetSiblingIndex()] = 0;
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
            return;
        }
        
        if(soulsInfo != null && !GameManager.Instance.soulsInfo.Contains(soulsInfo.GetUnitData().name)){
            GameManager.Instance.soulsInfo.Add(soulsInfo.GetUnitData().name);
        }
        battlePower ??= FindAnyObjectByType<BattlePower>();
        battlePower.SettingBattlePower(soulsInfo.battlePower);
    }

    public void SetSoulInfoForBattle(SoulsInfo soulsInfo){
        if(_soulInfo != null)  {
            SoulsManager.Instance.battleEquipDic.Remove(_soulInfo);    
        }
        
        this.soulsInfo = soulsInfo;

        if(soulsInfo == null) {
            GameData data = GameDataManger.Instance.GetGameData();
            data.battleEquip[transform.GetSiblingIndex()] = 0;
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
            return;
        }

        if(soulsInfo != null && !GameManager.Instance.battlesInfo.Contains(soulsInfo.GetUnitData().name)){
            GameManager.Instance.battlesInfo.Add(soulsInfo.GetUnitData().name);
        }
    }

    public SoulsInfo GetSoulInfo()
    {
        return _soulInfo;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !isEquip) soulsTab.SetInfo(_soulInfo , false , this);
    }
    
}
