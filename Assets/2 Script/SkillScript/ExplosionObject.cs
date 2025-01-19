using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionObject : MonoBehaviour
{
    float damage;
    Animator ani;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    public void Setting(float damage){
        this.damage = damage;
        StartCoroutine(WaitAttack());
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag(gameObject.tag)) {
            other.GetComponent<Unit>().Hit(damage , AttackType.SkillAttack);
        }
    }

    IEnumerator WaitAttack(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Explosion") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
