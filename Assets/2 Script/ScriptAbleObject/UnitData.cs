 
using System;
using UnityEngine;

public enum AttackType {
    None , LongRange , ShortRange
}
[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]
public class UnitData : ScriptableObject {
    public ItemClass type;
    public int typenumber;
    public Sprite image;
    public float attackSpeed;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
    public float spawnProbabillity;
}