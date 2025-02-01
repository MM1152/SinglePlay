using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LightningAttack : MonoBehaviour
{
    private Animator ani;
    public Vector3 caster;
    public Vector3 target;
    public void Init(Vector3 pos , Vector3 caster){
        this.caster = caster;
        target = pos;
        StargetSkill();
    }
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void StargetSkill(){
        StartCoroutine(WaitForAnimation());

        float distance = Vector2.Distance(target , caster);
        transform.localScale = new Vector3(distance / 2 - 0.8f, 1f , 1f);
        transform.position = (target + caster) / 2;

        Vector2 direction = (target - transform.position).normalized;
        transform.right = direction;
    }

    IEnumerator WaitForAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , this.gameObject);
    } 

}
