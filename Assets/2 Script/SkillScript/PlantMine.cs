using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantMine : MonoBehaviour, SkillParent
{
    public SoulsSkillData soulsSkillData { get; set; }
    
    public float currnetCoolTime;

    [Range(0f , 1f)]
    public float delayAnimation;
    
    private Unit unit;
    private Mine mine;
    private Animator ani;
    
    private void Awake() {
        unit = GetComponent<Unit>();
        mine = Resources.Load<Mine>("UseSkillFolder/Mine");
        ani = GetComponent<Animator>();
    }
    public float GetSkillCoolTime()
    {
        return currnetCoolTime;
    }

    public void UseSkill()
    {
        currnetCoolTime -= Time.deltaTime;
        if(currnetCoolTime <= 0) {
            currnetCoolTime = soulsSkillData.skillCoolTime;
            StartCoroutine(WaitPlantAnimation());
        }
    }
    IEnumerator WaitPlantAnimation(){
        ani.SetBool("Skill" , true);
        unit.isSkill = true;
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);

        Mine minePrefeb = PoolingManager.Instance.ShowObject(mine.name + "(Clone)" , mine.gameObject).GetComponent<Mine>();
        minePrefeb.Setting(unit , soulsSkillData);

        if(!unit.sp.flipX) minePrefeb.transform.position = unit.transform.position + new Vector3(0.4f , -0.45f , 0f);
        else minePrefeb.transform.position = unit.transform.position + new Vector3(-0.4f , -0.45f , 0f);
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        ani.SetBool("Skill" , false);
        
        unit.isSkill = false;
    }
}
