using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : ShortRangeScipt
{
    [Space(50)]
    [Header("보스 설정")]
    [SerializeField] Sprite ShowingImage; // 보스 소개장면에서 보여질 이미지 설정
    [SerializeField] BossShowAnimation BossShow; // 보스 소개하는 애니메이션 접근 ( image 와 Text 설정해줌 )
    [SerializeField] GameObject wideRangeSkillPrefeb;

    [Space(30)]
    [Header("광범위 공격 스킬 설정")]
    public float wideRangeSkillCoolTime = 5f;
    public  float currenwideRangeSkillCoolTime;
    
    [Space(30)]
    [Header("순간이동 공격 스킬 설정")]
    public float teleportSkillCoolTime = 1000f;
    public  float currentteleportSkillCoolTime;

    private void OnEnable(){ }
    protected void Start()
    {
        BossShow = GameObject.FindAnyObjectByType<BossShowAnimation>();
        currenwideRangeSkillCoolTime = wideRangeSkillCoolTime;
        currentteleportSkillCoolTime = teleportSkillCoolTime;
        StartCoroutine(WaitForAnimationCorutine());
        Init(1);
    }
    protected override void Update()
    {
        
        if (!GameManager.Instance.playingAnimation)
        {
            base.Update();
            if(!isSkill) {
                currentteleportSkillCoolTime -= Time.deltaTime;
                currenwideRangeSkillCoolTime -= Time.deltaTime;
            }
        
            WideRangeSkill();
            TelePortAttack();
        }
        else {
            
        }

    }
    IEnumerator WaitForAnimationCorutine()
    {
        GameManager.Instance.playingAnimation = true;
        
        yield return new WaitUntil(() => MovePosInAniamtion()); 
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SPAWN") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        ani.SetBool("PlaySpawnAni", false);

        BossShow.setBossImage = ShowingImage;
        BossShow.setBossName = "KING";
        BossShow.setBossData = this;
        BossShow.SetAnimation(true);

        yield return new WaitUntil(() => BossShow.IsPlayAnimation());

        BossShow.SetAnimation(false);
        GameManager.Instance.playingAnimation = false;
    }

    void WideRangeSkill()
    {
        if (currenwideRangeSkillCoolTime <= 0 && !isSkill)
        {
            currenwideRangeSkillCoolTime = wideRangeSkillCoolTime;
            ani.SetBool("Skill", true);
            isSkill = true;
            StartCoroutine(WaitForAttackSkillAnimation("SKILL" , 0.5f));
        }
    }
    void TelePortAttack()
    {

    }
    IEnumerator WaitForAttackSkillAnimation(string animationParameter, float normalizedTime)
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        if (animationParameter == "SKILL")
        {
            for (float i = 0.2f; i <= 1f; i += 0.2f)
            {
                GameObject skill = PoolingManager.Instance.ShowObject(wideRangeSkillPrefeb.name+"(Clone)", wideRangeSkillPrefeb);

                skill.transform.position = Vector2.Lerp(transform.position, target.transform.position, i);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        isSkill = false;
        ani.SetBool("Skill", false);
    }

    bool MovePosInAniamtion(){
        transform.position += Vector3.left * Time.deltaTime * speed;
        return ani.GetCurrentAnimatorStateInfo(0).IsName("SPAWN") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f;
    }
}
