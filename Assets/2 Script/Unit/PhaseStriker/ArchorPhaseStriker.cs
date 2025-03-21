using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchorPhaseStriker : PhaseStriker
{
    [SerializeField] GameObject projectile;
    void OnEnable(){ }
    void Start(){
        summon = GetComponent<Summon>();
     }
    private void Awake() {
        Debug.Log("SettingFin ArchorPhaseStriker");
        base.Awake();
        attackPattenChange = true;
    }
    public void Setting(DaggerPhaseStriker dagger){
       
        daggerPhaseStriker = dagger;
        gameObject.tag = daggerPhaseStriker.gameObject.tag;
        if(GameManager.Instance.mapName == "BattleMap") {
            Respawn();
            UserFightSpawn(level);
        }
        else if(daggerPhaseStriker.summoner != null) {
            summoner = daggerPhaseStriker.summoner;
            SummonUnitSetting(); 
        }
        else SetBoss();
        Respawn();
        canFollow = false;
        gameObject.transform.localScale = new Vector3(0.6f , 0.6f , 1f);
    }
    private void SummonUnitSetting(){
        ani.SetBool("PlaySpawnAni" , false);
        SummonerSpawn(summoner);
        gameObject.transform.localScale = Vector3.one;
    }
    private void Update() {
        base.Update();
        if(canAttack) {
            StartCoroutine(WaitAttackDelay());
        }
        
    }
    IEnumerator WaitAttackDelay(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.7f);
        ArchorProjectile archorProjectile = PoolingManager.Instance.ShowObject(projectile.name + "(Clone)" , projectile).GetComponent<ArchorProjectile>();
        if(target != null) archorProjectile.Setting(target.transform , this);
        else PoolingManager.Instance.ReturnObject(archorProjectile.name , archorProjectile.gameObject);
    }
}
