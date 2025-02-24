using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoner : LongRangeScript
{
    //\\TODO : 일청초마다 랜덤버프를 주는 스킬도 구현
    Vector2[] spawnPosition = new Vector2[] {
        new Vector2(2f , 0f) ,
        new Vector2(2f , -1.17f) ,
        new Vector2(0.35f , -1.76f) ,
        new Vector2(-1.22f , -0.97f) ,
        new Vector2(-0.72f , 0.26f)
    };
    public Action changeStatus;
    [SerializeField] GameObject EnemySpawn;
    [SerializeField] GameObject DieTitle;
    public Dictionary<string , float> additionalStats = new Dictionary<string , float>();

    bool oneTime;
    public float drainLife;
    private void OnEnable() { }
    private void Start() {
        RewardManager.Instance.SetSummonerStat = ChangeStat;
    }

    protected override void Awake()
    {
        Spawn(1);
        base.Awake();
        int i = 0;

        foreach(string key in GameManager.Instance.soulsInfo.Keys) {
            SpawnSoul(key , i++);
        }
    }
    private void Update()
    {
        if (!isDie)
        {
            if(hp > 0 && VirtualJoyStick.instance.isInput) {
                target = FindTarget(targetList);
                statusEffectMuchine.Update();
                return;
            }
            base.Update();
            if(GameManager.Instance.gameClear && target?.name != "NextStage") target = null; 

            if (GameManager.Instance.gameClear && !GameManager.Instance.playingShader)
            {
                target = GameManager.Instance.nextStage;   
            }
            else
            {
                if (target?.name != "NextStage") Attack();
                //if (SkillManager.Instance.SummonUpgradeSkill) SummonSkill();
            } 
            
        }
        else {
            if(!oneTime) {
                StartCoroutine(DieAnimation());
                oneTime = true;
            }
        }
    }
    public void Move(Vector3 movePos)
    {
        if(isAttack) return;
        if(hp <= 0) return;

        transform.position += movePos * Time.deltaTime * speed;
        ani.SetBool("Move" , true);
        sp.flipX = movePos.normalized.x <= 0;
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "NextStage" && !GameManager.Instance.playingShader) {
            target = null;
            ani.Play("InNextMap");
            SoundManager.Instance.Play(SoundManager.SFX.NextMap);
            StartCoroutine(GameManager.Instance.WaitForNextMap(() => SpawnMapPlayer()));
        }
    }
    private void SpawnMapPlayer(){
        transform.position = Vector3.zero;
        ani.Play("SpawnMap");
    }
    public void ChangeStat(string key = "None" , float value = 0){
        if(additionalStats.ContainsKey(key)) additionalStats[key] += value;
        else if(key != "None")additionalStats.Add(key , value);


        switch(key) {
            case "HP":
                maxHp += unit.hp * value;
                hp += hp * value;
                break;
            case "DAMAGE":
                damage += unit.damage * value;
                break;

            case "SPEED":
                speed += unit.speed * value;
                break;

            case "ATTACKSPEED":
                setInitAttackSpeed -= setInitAttackSpeed * value;
                break;
            
            case "Clitical":
                critical += value;
                break;
            
            case "CliticalDamage":
                cliticalPercent += value;
                break;
        }

        changeStatus?.Invoke();
    }
    private IEnumerator DieAnimation(){
        if(isDie) {
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f);
            GameManager.Instance.StopGame();  
            GameManager.Instance.ReturnToMenu();
            yield return new WaitUntil(() => Input.touchCount >= 1);
            
        }
    }
    public void SpawnSoul(string key , int spawnPos = 0){
        SummonUnit SpawnUnit = PoolingManager.Instance.ShowObject(EnemySpawn.name + "(Clone)",EnemySpawn).GetComponent<SummonUnit>();
        SpawnUnit.transform.position = spawnPosition[spawnPos++] + (Vector2)transform.position;
        SpawnUnit.tag = tag;
        SpawnUnit.Setting(GameManager.Instance.soulsInfo[key].SummonPrefeb , SpawnUnit.transform.position , transform.parent , this);
    }
}
