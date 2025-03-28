using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsTab : MonoBehaviour
{
    [SerializeField] SoulsExplainTab soulsExplainTab;
    private SoulsInfo currentSoulInfo;
    
    private void Start()
    {
        soulsExplainTab.Init(this);
        soulsExplainTab.SetEquip += StartCoroutine;
        
    }

    public void SetInfo(SoulsInfo info , bool open_To_SoulTab , EquipSouls equip)
    {
        if (info == null) return;
        soulsExplainTab.SettingSoulExplainTab(info , open_To_SoulTab , equip);
        currentSoulInfo = info;
        soulsExplainTab.gameObject.SetActive(true);
    }
    
    public void StartCoroutine(){
        if(gameObject.activeSelf) StartCoroutine(WaitForTouch());
    }

    IEnumerator WaitForTouch()
    {
        yield return new WaitUntil(() => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary);

        RaycastHit2D hit = Physics2D.Raycast(Input.GetTouch(0).position, Camera.main.transform.forward);
        if (hit.collider != null)
        {
            GameObject selectObject = hit.collider.gameObject;
            EquipSouls equipSouls;

            if (selectObject.TryGetComponent<EquipSouls>(out equipSouls))
            {
                int changesiblingIndex = -1;
                
                if(SoulsManager.Instance.equipDic.ContainsKey(currentSoulInfo)) {
                    changesiblingIndex = SoulsManager.Instance.equipDic[currentSoulInfo].transform.GetSiblingIndex();
                    SoulsManager.Instance.equipDic[currentSoulInfo].SetSoulInfo(null);
                    SoulsManager.Instance.equipDic[currentSoulInfo] = equipSouls;
                }
                else {
                    SoulsManager.Instance.equipDic.Add(currentSoulInfo , equipSouls);
                }
                equipSouls.SetSoulInfo(currentSoulInfo);

                GameData data = GameDataManger.Instance.GetGameData();
                if(changesiblingIndex != -1) data.soulsEquip[changesiblingIndex] = 0;
                data.soulsEquip[equipSouls.transform.GetSiblingIndex()] = currentSoulInfo.GetUnitData().typenumber;
                GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);

                            
                if(GameManager.Instance.isPlayingTutorial) {
                    GameManager.Instance.StartTutorial(25);
                }
            }
            else {
                if(GameManager.Instance.isPlayingTutorial) {
                    GameManager.Instance.StartTutorial(22);
                }
            }

            EquipSouls.isEquip = false;
            currentSoulInfo = null;
        }
        else
        {
            EquipSouls.isEquip = false;
            currentSoulInfo = null;
        }
    }

}
