
using System;
using UnityEngine;

public enum AttackType {
    None , LongRange , ShortRange
}
[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]
public class UnitData : ScriptableObject {
    public AttackType attackType;
    public string unitName;
    public float attackSpeed;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
}