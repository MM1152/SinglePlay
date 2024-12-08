using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Summoner : LongRangeScript
{
    Vector2[] spawnPosition = new Vector2[] {
        new Vector2(1f , 0f) ,
        new Vector2(0.8f , -0.5f) ,
        new Vector2(0.5f , -1f) ,
        new Vector2(0.8f , 0.5f) ,
        new Vector2(0.5f , 1f)
    };

    [SerializeField] GameObject EnemySpawn;
    [SerializeField] GameObject DieTitle;

    private Dictionary<string , float> additionalStats = new Dictionary<string , float>();

    
    public delegate void Function();
    public Function function;
    bool oneTime;
    private void OnEnable() { }
    private void Start() {
        RewardManager.Instance.SetSummonerStat = ChangeStat;
    }
    protected override void Awake()
    {
        base.Awake();
        Spawn(1);
        int i = 0;
        foreach(string key in GameManager.Instance.soulsInfo.Keys) {
            SummonUnit SpawnUnit = Instantiate(EnemySpawn).GetComponent<SummonUnit>();
            SpawnUnit.transform.position = spawnPosition[i++] + (Vector2)transform.position;
            SpawnUnit.tag = tag;
            SpawnUnit.Setting(GameManager.Instance.soulsInfo[key].SummonPrefeb , SpawnUnit.transform.position , transform.parent , this);
        }
    }
    private void Update()
    {
        if (!isDie)
        {
            if(VirtualJoyStick.instance.isInput) {
                return;
            }
            base.Update();
            if(GameManager.Instance.gameClear && target?.name.Split(' ')[0] != "NextStage") target = null; 

            if (GameManager.Instance.gameClear && !GameManager.Instance.playingShader)
            {
                target = GameManager.Instance.nextStage;   
            }
            else
            {
                if (!SkillManager.Instance.LightningAttack && target?.name.Split(' ')[0] != "NextStage") Attack();
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
        transform.position += movePos * Time.deltaTime * speed;
        isAttack = false;
        ani.SetBool("Attack" , false);
        ani.SetBool("Move" , true);
        sp.flipX = movePos.normalized.x <= 0;
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name == "NextStage") {
            target = null;
            ani.Play("InNextMap");
            StartCoroutine(GameManager.Instance.WaitForNextMap(() => SpawnMapPlayer()));
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
                maxHp += unit.hp * (value + 1);
                hp += hp * value;
                break;
            
            case "DAMAGE":
                damage += unit.damage * (value + 1);
                break;

            case "SPEED":
                speed += unit.speed * (value + 1);
                break;

            case "ATTACKSPEED":
                setInitAttackSpeed -= unit.attackSpeed * additionalStats[key];
                break;
        }
    }
    private IEnumerator DieAnimation(){
        if(isDie) {
            
            
            GameManager.Instance.SlowGame(0.6f);
            yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f);
            DieTitle.SetActive(true);
            GameManager.Instance.StopGame();  
            
            yield return new WaitUntil(() => Input.touchCount >= 1);
            GameManager.Instance.ReturnToMenu();
        }
    }
}
