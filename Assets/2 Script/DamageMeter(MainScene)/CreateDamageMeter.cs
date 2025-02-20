using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDamageMeter : MonoBehaviour
{
    [SerializeField] SettingMobDamageMeter damageMeter;
    [SerializeField] Transform prefebParent;
    GameObject on_off;
    public void Init(Unit unit)
    {
        on_off = transform.GetChild(0).gameObject;
        if(damageMeter == null) {
            Debug.LogError("Error Find SettingMob");
            return;
        }

       
        SettingMobDamageMeter createNew = Instantiate(damageMeter , prefebParent); 
        createNew.Setting(unit);
    }

}
