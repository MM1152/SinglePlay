using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDamageMeter : MonoBehaviour
{
    [SerializeField] SettingMobDamageMeter damageMeter;
    [SerializeField] Transform prefebParent;
    List<SettingMobDamageMeter> damageMeterPools = new List<SettingMobDamageMeter>();
    public void Init(Unit unit)
    {
        if(damageMeter == null) {
            Debug.LogError("Error Find SettingMob");
            return;
        }

        SettingMobDamageMeter createNew = Instantiate(damageMeter , prefebParent); 
        damageMeterPools.Add(createNew);
        StartCoroutine(WaitForUnitSetting(createNew, unit));
    }

    public void Redirect(Unit unit){
        for(int i = 0 ; i < damageMeterPools.Count; i++) {
            Debug.Log("SettingDamageTab");
            if(damageMeterPools[i].unitname == unit.unit.name) {
                damageMeterPools[i].Setting(unit);
            }
        }
    }
    IEnumerator WaitForUnitSetting(SettingMobDamageMeter createNew , Unit unit){
        yield return new WaitUntil(() => unit.maxHp != 0);
        createNew.Setting(unit);
    }
}
