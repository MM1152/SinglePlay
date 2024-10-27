using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeAttack : MonoBehaviour
{   
    public UnitData unit;
    public Unit parent;
    public Vector2 target;
    private void Awake() {
        parent = transform.parent.GetComponent<Unit>();    
    }
    private void OnEnable() {
        transform.position = target;
        StartCoroutine(DisappearCorutine());
    }
    IEnumerator DisappearCorutine(){
        yield return new WaitWhile(() => parent.isAttack);
            
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<IDamageAble>() != null && !other.CompareTag(transform.parent.tag) && !other.GetComponent<Unit>().isDie) {
            other.GetComponent<IDamageAble>().Hit(unit.damage);
        }
    }
    
}
