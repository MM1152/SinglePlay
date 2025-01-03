using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainLife : MonoBehaviour , SkillParent
{
    float skillCoolTime;
    [SerializeField] BigDarker bigDarker;
    public Heap findNearEnemy;
    public SoulsSkillData soulsSkillData { get ; set ; }

    public float GetSkillCoolTime()
    {
        return -1;
    }
    public void UseSkill()
    {
        if(bigDarker.canAttack) {
            findNearEnemy = new Heap();
            foreach(Transform target in bigDarker.targetList.transform) {
                bool findMinDistance = Vector2.Distance(target.position , transform.position) <= bigDarker.unit.attackRadious // 거리가 가장 짧은지
                                        && target.GetComponent<IFollowTarget>().canFollow; // Follow 가능한 상태인지
                if (findMinDistance)
                {
                    findNearEnemy.Add(Vector2.Distance(transform.position , target.position) , target);
                }
            }

            while(true) {
                Transform targetPos = findNearEnemy.Pop();
                Debug.Log(targetPos.gameObject.name);
                if(targetPos != null && targetPos.gameObject != bigDarker.target) {
                    GameObject attack = PoolingManager.Instance.ShowObject(bigDarker.darkerAttack.name + "(Clone)" , bigDarker.darkerAttack);
                    attack.GetComponent<BigDarkerAttack>().target = targetPos;
                    bigDarker.target.GetComponent<IDamageAble>().Hit(bigDarker.damage , AttackType.SkillAttack);
                    Debug.Log(bigDarker.damage);
                    break;
                }
                else if(targetPos == null){
                    break;
                }
            }
        
        }
    }
    void Awake()
    {
        bigDarker = GetComponent<BigDarker>();
    }
}
