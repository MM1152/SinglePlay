using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill : MonoBehaviour
{
    [SerializeField] GameObject skillPrefeb;
    
    Summoner summoner;

    float damagePercent = 1.2f;
    float skillCoolTime = 2f;
    float currentSkillCoolTime;

    private void Awake()
    {
        summoner = GetComponent<Summoner>();
        currentSkillCoolTime = skillCoolTime;
    }
    private void Update() {
        Skill();
    }
    private void Skill()
    {
        if (SkillManager.Instance.LightningAttack)
        {
            if(currentSkillCoolTime <= 0){
                GameObject lightning = PoolingManager.Instance.ShowObject(skillPrefeb.name +"(Clone)" , skillPrefeb);   
                float distance = Vector2.Distance(summoner.target.transform.position , transform.position);

                lightning.transform.localScale = new Vector3(distance / 2, 1f , 1f);
                lightning.transform.position = (summoner.target.transform.position + transform.position) / 2;

                Vector2 direction = (summoner.target.transform.position - transform.position).normalized;
                lightning.transform.right = direction;

                summoner.target.GetComponent<IDamageAble>().Hit(summoner.damage * damagePercent);
                currentSkillCoolTime = skillCoolTime;

            }
              
            currentSkillCoolTime -= Time.deltaTime;
        }
    }
}
