using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsTab : MonoBehaviour
{
    [SerializeField] SoulsExplainTab soulsExplainTab;
    private SoulsInfo currentSoulInfo;
    public Action<SoulsInfo> settingSoul;
    public Dictionary<EquipSouls , SoulsInfo> equipDic = new Dictionary<EquipSouls , SoulsInfo>();
    private void Start()
    {
        settingSoul += SetInfo;
        soulsExplainTab.SetEquip += StartCoroutine;
    }

    public void SetInfo(SoulsInfo info)
    {
        if (info == null) return;
        soulsExplainTab.SettingSoulExplainTab(info);
        currentSoulInfo = info;
        soulsExplainTab.gameObject.SetActive(true);
    }
    public void StartCoroutine(){
        StartCoroutine(WaitForTouch());
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
                //\\TODO 클릭시 이미 존재하는 소환수면 지워주고 위치 이동 /

                equipSouls.SetSoulInfo(currentSoulInfo);
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
