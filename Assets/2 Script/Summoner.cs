using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Summoner : LongRangeScript
{
    [SerializeField] float skillCoolDown;
    [SerializeField] float skillCurrentTime;

    private Dictionary<string , float> additionalStats = new Dictionary<string , float>();

    public delegate void Function();
    public Function function;
    private void OnEnable() { }
    private void Start() {
        RewardManager.Instance.SetSummonerStat = ChangeStat;
    }
    protected override void Awake()
    {
        base.Awake();
        Init(1);
        skillCoolDown = 5f; //:fix 각 스킬 쿨타임 연결해서 관리해줘야됌
        skillCurrentTime = skillCoolDown;
    }
    protected override void Update()
    {
        if (!isDie)
        {
            base.Update();
            if(GameManager.Instance.gameClear && target?.name != "NextStage") target = null; 

            if (GameManager.Instance.gameClear && !GameManager.Instance.playingShader)
            {
                target = GameManager.Instance.nextStage;   
            }
            else
            {
                if (!SkillManager.Instance.LightningAttack && target?.name != "NextStage") Attack();
                if (SkillManager.Instance.SummonSkill) SummonSkill();
            } 
            
        }
    }
    public void Move(Vector3 movePos)
    {
        transform.position += movePos * Time.deltaTime * unit.speed;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "NextStage") {
            target = null;
            ani.Play("InNextMap");
            StartCoroutine(GameManager.Instance.WaitForNextMap(() => SpawnMapPlayer()));
        }
    }
    public void SummonSkill()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ISummonUnit.unitCount < SkillManager.Instance.UpgradeMaxSummonCount)
        {
            if (skillCurrentTime <= 0)
            {
                isSkill = true;
                ani?.SetBool("Skill", true);
                StartCoroutine(WaitForSkillAnimationCorutine());
            }
            skillCurrentTime -= Time.deltaTime;
        }
    }
    IEnumerator WaitForSkillAnimationCorutine()
    {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("SKILL") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        ani.SetBool("Skill", false);

        GameObject unit = SkillManager.Instance.GetSummonGameObjet();
        unit = PoolingManager.Instance.ShowObject(unit.name + "(Clone)", unit);
        unit.transform.SetParent(transform.parent);
        unit.GetComponent<ISummonUnit>().summoner = this;
        unit.tag = gameObject.tag;
        unit.transform.position = transform.position + Vector3.right;

        skillCurrentTime = skillCoolDown;
        isSkill = false;
    }
    private void SpawnMapPlayer(){
        transform.position = Vector3.zero;
        ani.Play("SpawnMap");
        function?.Invoke();
    }
    public void ChangeStat(string key , float value){
        if(additionalStats.ContainsKey(key)) additionalStats[key] += value;
        else additionalStats.Add(key , value);


        switch(key) {
            case "HP":
                maxHp = unit.hp * (additionalStats[key] + 1);
                hp += hp * value;
                break;
            
            case "DAMAGE":
                damage = unit.damage * (additionalStats[key] + 1);
                break;

            case "SPEED":
                speed = unit.speed * (additionalStats[key] + 1);
                break;

            case "ATTACKSPEED":
                setInitAttackSpeed -= unit.attackSpeed - unit.attackSpeed * additionalStats[key];
                break;
        }
    }
    public void TestCode(){
        unit.hp += 100f;
    }
}
