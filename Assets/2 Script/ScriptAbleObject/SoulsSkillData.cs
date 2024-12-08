using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoulsSkillData" , fileName = "SoulsSkillData")]
public class SoulsSkillData : ScriptableObject
{
    public Sprite skillImages;
    public string skillName;
    
    [TextArea]
    public string skillExplainText;
    public float skillInitPercent;
    public float attackPercent;
    public float skillCoolTime;
    public Demon skillScript;

}
