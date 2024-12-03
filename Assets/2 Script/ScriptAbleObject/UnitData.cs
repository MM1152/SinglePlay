 
using System;
using UnityEngine;

public enum AttackType {
    None , LongRange , ShortRange
}
[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]
public class UnitData : ScriptableObject {
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

    [Header ("소환수로 부릴 시 사용되는 탭")]
    public SoulsSkillData[] soulsSkillData;
    [TextArea]
    public string explainText;
}