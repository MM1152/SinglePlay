using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mine : MonoBehaviour
{
    private Unit unit;
    private Animator ani;
    private SoulsSkillData skillData;
    private void Awake() {
        ani = GetComponent<Animator>();
        
    }
    public void Setting(Unit unit , SoulsSkillData skillData){
        this.unit = unit;
        this.skillData = skillData;
        gameObject.tag = unit.gameObject.tag;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.gameObject.CompareTag(unit.tag)) {
            ani.SetBool("IsBoom" , true);
            Unit unit;
            if(other.TryGetComponent<Unit>(out unit)) {
                unit.Hit(unit.damage * (skillData.attackPercent / 100f), unit , unit.clitical , AttackType.Burn);
                unit.statusEffectMuchine.SetStatusEffect(new BurnEffect() , unit);
            }
            StartCoroutine(WaitAnimation());
        }
    }

    IEnumerator WaitAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("MineAnimation") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
