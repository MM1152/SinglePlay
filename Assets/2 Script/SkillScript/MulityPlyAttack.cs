using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MulityPlyAttack : SkillParent
{
    public SoulsSkillData soulsSkillData;
    LongRangeScript longRangeUnit;

    public override void UseSkill()
    {
        
        if (longRangeUnit.isAttack)
        {
            float mulityPlyAttackPercent = Random.Range(0f, 1f);

            if (soulsSkillData.attackPercent / 100f <= mulityPlyAttackPercent)
            {
                
                GameObject currentTarget = longRangeUnit.target;
                GameObject otherTarget = default;
                for (int i = 0; i < 2; i++)
                {
                    float minDistance = 999999f;
                    foreach (Transform targets in longRangeUnit.targetList.transform)
                    {
                        bool findMinDistance = Vector2.Distance(targets.position, transform.position) < minDistance  // 거리가 가장 짧은지
                                               && targets.GetComponent<IFollowTarget>().canFollow // Follow 가능한 상태인지
                                               && targets != currentTarget // 이미 선택된 타켓이 아닌지
                                               && targets != otherTarget // 두번째로 선택된 타켓이 아닌지
                                               && Vector2.Distance(targets.position , transform.position) <= longRangeUnit.unit.attackRadious;
                        if (findMinDistance)
                        {
                            minDistance = Vector2.Distance(targets.position, transform.position);
                            otherTarget = targets.gameObject;
                        }
                    }
                    Debug.Log("other Target :" + otherTarget);
                    StartCoroutine(longRangeUnit.WaitForAttackAnimation(otherTarget));
                }
                
            }
        }
    }

    private void Awake()
    {
        longRangeUnit = GetComponent<LongRangeScript>();
    }
    private void Update()
    {
       UseSkill();
    }
}
