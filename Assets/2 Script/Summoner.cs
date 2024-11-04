using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Summoner : LongRangeScript
{
    
    [SerializeField] float skillCoolDown;
    [SerializeField] float skillCurrentTime;

    public delegate void Function();
    public Function function;

    AttackSkill attack;
    private void Awake()
    {
        Init(1);
        attack = GetComponent<AttackSkill>();
        skillCoolDown = 5f;
        skillCurrentTime = skillCoolDown;
    }
    private void Update()
    {
        if (!isDie)
        {
            if (GameManager.Instance.gameClear && !GameManager.Instance.playingShader)
            {
                target = GameManager.Instance.nextStage;
            }
            else
            {
                if (!SkillManager.Instance.LightningAttack) Attack();
                if (SkillManager.Instance.SummonSkill) SummonSkill();
            }

            KeepChcek();
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
        switch(key) {
            case "hp":
                maxHp += maxHp * value;
                break;
            
            case "damage":
                damage += damage * value;
                break;

            case "speed":
                speed += speed * value;
                break;
            case "attackspeed":
                setInitAttackSpeed -= setInitAttackSpeed * value; 
                break;
        }
    }

}
