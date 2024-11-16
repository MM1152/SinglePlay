using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public float currenwideRangeSkillCoolTime;

    [Space(30)]
    [Header("순간이동 공격 스킬 설정")]
    public float teleportSkillCoolTime = 9f;
    public float currentteleportSkillCoolTime;

    private bool oneTime = true;
    private void OnEnable() { 
        attackPattenChange = true;
    }
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
            
            if (!isSkill)
            {
                Attack();
                currentteleportSkillCoolTime -= Time.deltaTime;
                currenwideRangeSkillCoolTime -= Time.deltaTime;
            }

            WideRangeSkill();
            TelePortAttack();
        }

    }


    void WideRangeSkill()
    {
        if (currenwideRangeSkillCoolTime <= 0 && !isSkill)
        {
            currenwideRangeSkillCoolTime = wideRangeSkillCoolTime;
            ani.SetBool("Skill", true);
            isSkill = true;
            StartCoroutine(WaitForAttackSkillAnimation("SKILL", 0.5f));
        }
    }
    void TelePortAttack()
    {
        if (currentteleportSkillCoolTime <= 0 && !isSkill)
        {
            currentteleportSkillCoolTime = teleportSkillCoolTime;
            ani.SetBool("Skill2", true);
            isSkill = true;
            StartCoroutine(WaitForAttackSkillAnimation("SKILL2", 0.4f));
        }
    }

    bool MovePosInAniamtion()
    {
        transform.position += Vector3.left * Time.deltaTime * speed;
        return ani.GetCurrentAnimatorStateInfo(0).IsName("SPAWN") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f;
    }
    //처음 등장 애니메이션 대기
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
    //스킬애니메이션 대기했다가 필요한 기능 구현
    IEnumerator WaitForAttackSkillAnimation(string animationParameter, float normalizedTime)
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= normalizedTime);
        isAttack = false; 
        ani.SetBool("Attack" , false);
        if (animationParameter == "SKILL")
        {
            for (float i = 0.2f; i <= 1f; i += 0.2f)
            {
                GameObject skill = PoolingManager.Instance.ShowObject(wideRangeSkillPrefeb.name + "(Clone)", wideRangeSkillPrefeb);
                skill.GetComponent<WideRangeSkill>().unit = this;
                skill.transform.position = Vector2.Lerp(transform.position, target.transform.position, i);
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (animationParameter == "SKILL2")
        {
            canFollow = false;

            GameObject WarningArea = PoolingManager.Instance.ShowObject("WarningArea(Clone)");
            InWarningArea inWarningArea = WarningArea.GetComponent<InWarningArea>();
            inWarningArea.Setting(1.2f, 1f, this);
            inWarningArea.gameObject.SetActive(true);
            inWarningArea.SetPosition(target.transform.position + Vector3.down * 0.6f);
            gameObject.transform.position = target.transform.position;

            gameObject.transform.position += Vector3.forward * 2f;

            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);

            canFollow = true;
            gameObject.transform.position -= Vector3.forward * 2f;

        }
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName(animationParameter) && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        oneTime = true;
        isSkill = false;
        ani.SetBool("Skill", false);
        ani.SetBool("Skill2", false);
    }

    protected override void Attack()
    {
        base.Attack();
        if (isAttack)
        {
            ShowWarningArea();
        }
        else {
            oneTime = true;
        }
    }

    void ShowWarningArea(){
        if (oneTime)
        {
            oneTime = false;
            GameObject WarningArea = PoolingManager.Instance.ShowObject("WarningArea(Clone)");
            InWarningArea inWarningArea = WarningArea.GetComponent<InWarningArea>();
            inWarningArea.Setting(1f, 1f, this);
            inWarningArea.SetPosition(target.transform.position + Vector3.down * 0.6f);
        }
    }
}
