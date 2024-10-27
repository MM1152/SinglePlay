using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "SkillData", menuName = "SkillData", order = 0)]
public class SkillData : ScriptableObject
{
    public string skillName;
    public float coolTime;
    public int skillLevel;
    public int maxSkillLevel;
    public string skillExplain;
}
