using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class King : ShortRangeScipt , ISummonUnit
{
    [Space(50)]
    [Header("보스 설정")]
    [SerializeField] Sprite ShowingImage; // 보스 소개장면에서 보여질 이미지 설정
    [SerializeField] BossShowAnimation BossShow; // 보스 소개하는 애니메이션 접근 ( image 와 Text 설정해줌 )

    public Summoner summoner { get ; set ; }

    private void BossSetting(){
        BossShow = GameObject.FindAnyObjectByType<BossShowAnimation>();
        attackPattenChange = true; // 어택이 기본 ShortRangeScript의 어택방식을 따르지 않을때 선언
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
    private void SummonUnitSetting(){
        ani.SetBool("PlaySpawnAni" , false);
        SummonerSpawn(summoner);
        gameObject.transform.localScale -= Vector3.one * 0.2f ;
    }
    private void Start(){
        if(summoner == null) BossSetting();
        else SummonUnitSetting();
    }
    private void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {

        if (!GameManager.Instance.playingAnimation)
        {
            base.Update();
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

    protected override void Attack()
    {
        base.Attack();
        if (canAttack && gameObject.CompareTag("Boss"))
        {
            ShowWarningArea();
        }
    }

    void ShowWarningArea()
    {
        GameObject WarningArea = PoolingManager.Instance.ShowObject("WarningArea(Clone)");
        InWarningArea inWarningArea = WarningArea.GetComponent<InWarningArea>();
        inWarningArea.Setting(1f, 2f, this);
        inWarningArea.SetPosition(target.transform.position + Vector3.down * 0.6f);
    }
}
