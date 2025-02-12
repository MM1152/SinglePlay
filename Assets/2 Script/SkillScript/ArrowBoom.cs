using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBoom : MonoBehaviour , SkillParent
{
    public SoulsSkillData soulsSkillData { get ; set ; }
    Unit unit;
    Animator ani;
    [SerializeField] float currentCoolTime;
    [SerializeField] GameObject arrowBoomVFX;
    [SerializeField] Animator AnimationVFX;
    private void Start() {
        unit = GetComponent<Unit>();
        ani = GetComponent<Animator>();
        currentCoolTime = soulsSkillData.skillCoolTime;
        arrowBoomVFX = Resources.Load<GameObject>("UseSkillFolder/ArchorSkillVFX");
        arrowBoomVFX = Instantiate(arrowBoomVFX, transform);
        arrowBoomVFX.transform.localScale = new Vector3(2f , 1.5f , 1f);
        AnimationVFX = arrowBoomVFX.GetComponent<Animator>();
        arrowBoomVFX.SetActive(false);
    }
    public float GetSkillCoolTime()
    {
        return currentCoolTime;
    }

    public void UseSkill()
    {
        if(unit.isDie) return;
        
        if(currentCoolTime <= 0 ) {
            currentCoolTime = soulsSkillData.skillCoolTime;
            ani.SetBool("Skill" , true);
            StartCoroutine(WaitAnimationCorutaine());
        }else {
            currentCoolTime -= Time.deltaTime;
        }
    }
    IEnumerator WaitAnimationCorutaine(){
        
        unit.isSkill = true;
        
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        InWarningArea warning = PoolingManager.Instance.ShowObject("CircleWarningArea(Clone)").GetComponent<InWarningArea>();
        warning.Setting(soulsSkillData.attackPercent / 100f, 0.4f , unit);
        warning.SetPosition(unit.target.transform.position); 

        ani.SetBool("Skill" , false);
        unit.isSkill = false;

        yield return new WaitForSeconds(0.2f);
        
        arrowBoomVFX.transform.position = unit.target.transform.position;
        arrowBoomVFX.SetActive(true);

        yield return new WaitUntil(() => AnimationVFX.GetCurrentAnimatorStateInfo(0).IsName("ArchorSkillVFX") && AnimationVFX.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        arrowBoomVFX.SetActive(false);   
    }
}
