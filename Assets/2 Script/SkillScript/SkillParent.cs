using UnityEngine;

public interface SkillParent {
    public SoulsSkillData soulsSkillData { get; set; }
    public void UseSkill();
    public float GetSkillCoolTime(); // -1 로 설정해야 해당슬릇은 쿨타임이 없다고 판단
    
} 