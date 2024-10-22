using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "SkillData", menuName = "SkillData", order = 0)]
public class SkillData : ScriptableObject
{
    public AttackType attackType;
    public string unitName;
    public float attackSpeed;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
}
