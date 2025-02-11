 
using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType {
    None , CriticalAttack , SkillAttack , Dodge , Burn
}
public struct Status {
    public float attackStat;
    public float hpStat;
    public float speedStat;
}

[Serializable]
public class SkillUnLockLevel{
    public int level;
    public SoulsSkillData skillData;
}

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]
public class UnitData : ScriptableObject {
    public string name;
    public ItemClass type;
    public ClassStruct classStruct;
    public int typenumber;
    public Sprite image;
    public float attackSpeed;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
    public float spawnProbabillity;
    [Space(50)]
    [Header ("소환수로 부릴 시 사용되는 탭")]
    public SkillUnLockLevel[] soulsSkillData;
    public GameObject SummonPrefeb;
    [TextArea]
    public string explainText;
    public string[] stat;
    public Status bonusStat = new Status();
    public Status curStat = new Status();
    [Header ("폼 변환시 변환 될 유닛데이터")]
    public UnitData changeFormInfo;
}