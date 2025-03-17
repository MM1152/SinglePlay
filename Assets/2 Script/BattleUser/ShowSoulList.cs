using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class ShowSoulList : MonoBehaviour
{
    [SerializeField] SortSoul sortSoul;
    [SerializeField] SoulsExplainTab soulsExplainTab;
    [SerializeField] Button button;
    List<SoulsInfo> list = new List<SoulsInfo>();
 
    void OnEnable()
    {
        soulsExplainTab.SetEquip += StartCoroutine;
        for (int i = 0; i < sortSoul.souls.Length; i++)
        {
            if (!sortSoul.souls[i].unLock) continue;
            if (list.Contains(sortSoul.souls[i])){
                continue;
            } 

            SoulsInfo copy = Instantiate(sortSoul.souls[i].gameObject, transform).GetComponent<SoulsInfo>();
            copy.parentSoulsInfo = sortSoul.souls[i].gameObject.GetComponent<SoulsInfo>();
            copy.slider.gameObject.SetActive(false);
            list.Add(sortSoul.souls[i]);
        }
    }

    public void StartCoroutine()
    {
        if(gameObject.activeSelf) StartCoroutine(WaitForTouch());
    }

    IEnumerator WaitForTouch()
    {
        SoulsInfo currentSoulInfo = soulsExplainTab.GetSoulInfo();
        yield return new WaitUntil(() => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary);

        RaycastHit2D hit = Physics2D.Raycast(Input.GetTouch(0).position, Camera.main.transform.forward);
        if (hit.collider != null)
        {
            GameObject selectObject = hit.collider.gameObject;
            EquipSouls equipSouls;
            Debug.Log(currentSoulInfo);
            if (selectObject.TryGetComponent<EquipSouls>(out equipSouls))
            {
                int changesiblingIndex = -1;

                if (SoulsManager.Instance.battleEquipDic.ContainsKey(currentSoulInfo))
                {
                    changesiblingIndex = SoulsManager.Instance.battleEquipDic[currentSoulInfo].transform.GetSiblingIndex();
                    Debug.Log(currentSoulInfo);
                    SoulsManager.Instance.battleEquipDic[currentSoulInfo].SetSoulInfoForBattle(null);
                    SoulsManager.Instance.battleEquipDic[currentSoulInfo] = equipSouls;
                }
                else
                {
                    SoulsManager.Instance.battleEquipDic.Add(currentSoulInfo, equipSouls);
                }
                equipSouls.SetSoulInfoForBattle(currentSoulInfo);

                GameData data = GameDataManger.Instance.GetGameData();
                if (changesiblingIndex != -1) data.battleEquip[changesiblingIndex] = 0;
                data.battleEquip[equipSouls.transform.GetSiblingIndex()] = currentSoulInfo.GetUnitData().typenumber;
                GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
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
