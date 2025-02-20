using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDamageMeter : MonoBehaviour
{
    [SerializeField] SettingMobDamageMeter damageMeter;
    [SerializeField] Transform prefebParent;
    GameObject on_off;
    List<SettingMobDamageMeter> damageMeterPools = new List<SettingMobDamageMeter>();
    public void Init(Unit unit)
    {
        on_off = transform.GetChild(0).gameObject;
        if(damageMeter == null) {
            Debug.LogError("Error Find SettingMob");
            return;
        }

       
        SettingMobDamageMeter createNew = Instantiate(damageMeter , prefebParent); 
        damageMeterPools.Add(createNew);
        createNew.Setting(unit);
    }

    public void Redirect(Unit unit){
        for(int i = 0 ; i < damageMeterPools.Count; i++) {
            Debug.Log("SettingDamageTab");
            if(damageMeterPools[i].unitname == unit.unit.name) {
                
                damageMeterPools[i].Setting(unit);
            }
        }
    }

}
