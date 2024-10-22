using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUnit : ShortRangeScipt , IDamageAble
{
    public static int unitCount;
    public void Awake() { // 소환물은 프리팹이라 인스펙터창에서 드래그로 적용을 못시켜줘서 targetList 설정필요
        base.Awake();
        unitCount++;
        targetList = GameObject.Find("EnemyList").gameObject;
        Debug.Log($"hp {hp} , mp {mp} , damage {damage}");
    }
    private void Update()
    {
        FollowTarget();
        base.Update();
    }
    private void OnDisable() {
        unitCount--;
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
}
