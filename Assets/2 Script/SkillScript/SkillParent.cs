using UnityEngine;

public interface SkillParent {
    public SoulsSkillData soulsSkillData { get; set; }
    public void UseSkill();
    public float GetSkillCoolTime();
} 