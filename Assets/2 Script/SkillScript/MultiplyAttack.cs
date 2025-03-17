using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyAttack : MonoBehaviour , SkillParent
{
    LongRangeScript longRangeUnit;
    bool executeSkill;
    public GameObject currentTarget;
    public GameObject otherTarget;
    public GameObject checkTarget;

    public SoulsSkillData soulsSkillData { get ; set ; }

    public void UseSkill()
    {
        
        if (longRangeUnit.isAttack && !executeSkill)
        {
            executeSkill = true;

            float mulityPlyAttackPercent = Random.Range(0f, 1f);

            if (soulsSkillData.skillInitPercent / 100f <= mulityPlyAttackPercent)
            {
                currentTarget = longRangeUnit.target;
                otherTarget = default;
                checkTarget = default; 
                for (int i = 0; i < 2; i++)
                {
                    float minDistance = 999999f;
                    foreach (Transform targets in longRangeUnit.targetList.transform)
                    {
                        bool findMinDistance = Vector2.Distance(targets.position, transform.position) < minDistance  // 거리가 가장 짧은지
                                               && targets.GetComponent<IFollowTarget>().canFollow // Follow 가능한 상태인지
                                               && targets.gameObject != currentTarget // 이미 선택된 타켓이 아닌지
                                               && targets.gameObject != otherTarget // 두번째로 선택된 타켓이 아닌지
                                               && Vector2.Distance(targets.position , transform.position) <= longRangeUnit.unit.attackRadious;
                        if (findMinDistance)
                        {
                            minDistance = Vector2.Distance(targets.position, transform.position);
                            otherTarget = targets.gameObject;
                        }
                    }
                    
                    if(checkTarget == otherTarget) return;
                
                    checkTarget = otherTarget;

                    StartCoroutine(longRangeUnit.WaitForAttackAnimation(otherTarget));
                }
               
            }
        }
        else if(!longRangeUnit.isAttack) executeSkill = false;
    }

    private void Awake()
    {
        longRangeUnit = GetComponent<LongRangeScript>();
    }

    public float GetSkillCoolTime()
    {
        return -1;
    }
}
