using UnityEngine;

public class DrainLifeUpgrade : MonoBehaviour , SkillParent
{
    public DrainLife drainLife;

    public SoulsSkillData soulsSkillData { get ; set ; }

    public float GetSkillCoolTime()
    {
        return -1;
    }

    public void UseSkill()
    {
        return;
    }

    private void Start() {
        GetComponent<DrainLife>().targetNumber += 3;
    }
}