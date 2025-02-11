using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchorProjectile : MonoBehaviour
{
    Vector2 direction;
    Unit unit;
    Animator ani;
    Transform target;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    public void Setting(Transform target , Unit unit){
        this.unit = unit;
        this.target = target;
        transform.position = unit.transform.position;
        SetDirecetion(this.target);
        StartCoroutine(WaitAnimtion());
    }
    void SetDirecetion(Transform target){
        float distance = Vector2.Distance(target.transform.position , transform.position);
        transform.localScale = new Vector3(distance * 0.3f , 1f , 1f);

        direction = (target.transform.position - transform.position).normalized;
        transform.right = direction;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag != unit.gameObject.tag) {
            other.GetComponent<IDamageAble>().Hit(unit.damage , unit.clitical , unit : unit);
        }
    }
    IEnumerator WaitAnimtion(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("ArchorProjectile") &&  ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
