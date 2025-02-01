using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : ShortRangeScipt 
{

    [SerializeField] BossShowAnimation BossShow; // 보스 소개하는 애니메이션 접근 ( image 와 Text 설정해줌 )
    protected void BossSetting(){
        BossShow = GameObject.FindAnyObjectByType<BossShowAnimation>();
        StartCoroutine(WaitForAnimationCorutine());
        Spawn(1);

        for (int i = 0; i < unit.soulsSkillData.Length; i++)
        {
            string findclass = unit.soulsSkillData[i].skillData.skillName;
            gameObject.AddComponent(Type.GetType(findclass)); // 스킬 이름에 따라 스킬 컴포넌트를 붙여줘서 사용하는 방식
            SkillParent skilldata = gameObject.GetComponent(Type.GetType(findclass)) as SkillParent;
            skilldata.soulsSkillData = unit.soulsSkillData[i].skillData;
            skillData.Add(skilldata);
        }
        
    }
    
    protected override void Update()
    {

        if (!GameManager.Instance.playingAnimation)
        {
            base.Update();
        }

    }

    IEnumerator WaitForAnimationCorutine()
    {
        GameManager.Instance.playingAnimation = true;

        yield return new WaitUntil(() => WaitSpawn());
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SPAWN") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        ani.SetBool("PlaySpawnAni", false);
        
        BossShow.setBossName = unit.name;
        BossShow.setBossData = this;
        BossShow.SetAnimation(true);

        yield return new WaitUntil(() => BossShow.IsPlayAnimation());

        BossShow.SetAnimation(false);
        GameManager.Instance.playingAnimation = false;
        yield return new WaitUntil(() => isDie);
        
        BossShow.transform.GetChild(0).gameObject.SetActive(false);
        BossShow.transform.GetChild(1).gameObject.SetActive(false);
    }

        //처음 등장 애니메이션 대기
    public virtual bool WaitSpawn()
    {
        return ani.GetCurrentAnimatorStateInfo(0).IsName("SPAWN") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f;
    }


}
