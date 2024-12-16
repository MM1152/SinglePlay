using UnityEngine;

public abstract class SkillParent : MonoBehaviour {
    public SoulsSkillData soulsSkillData;
    public abstract void UseSkill();
    public abstract float GetSkillCoolTime();
} 