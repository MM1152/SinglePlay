using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeAttack : MonoBehaviour
{   
    public UnitData unit;
    public Vector2 target;
    private void OnEnable() {
        transform.localPosition = (target - (Vector2)transform.parent.position).normalized;
        StartCoroutine(DisappearCorutine());
    }
    IEnumerator DisappearCorutine(){
        yield return new WaitForSeconds(0.5f);  // 애니메이션 추가되면 공격 애니메이션 끝날때까지 대기
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag(transform.parent.tag)) {
            other.GetComponent<IDamageAble>().Hit(unit.damage);
        }
    }
    
}
