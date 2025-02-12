using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWarningArea : MonoBehaviour
{
    public Unit unit;
    public float percent;    
    public float attackDelay = 1.0f;
    List<GameObject> hitObject = new List<GameObject>();
    public void Setting(float percent , float attackDelay , Unit unit){
        this.percent = percent;
        this.attackDelay = attackDelay; 
        this.unit = unit;
    }

    private void Update() {
        attackDelay -= Time.deltaTime;
        if(attackDelay <= 0f) {
            if(hitObject.Count > 0) {
                foreach(GameObject hit in hitObject) {
                    IDamageAble damageable;
                    if(hit.TryGetComponent<IDamageAble>(out damageable)) {
                        damageable.Hit(unit.damage * percent , unit.clitical , AttackType.SkillAttack , unit);
                    }
                    else Debug.LogError("GetComponent Fail : " + damageable.ToString());
                }
            } 

            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag(unit.gameObject.tag)) {
            hitObject.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
         if(!other.CompareTag(unit.gameObject.tag)) {
            hitObject.Remove(other.gameObject);
        }
    }
    public void SetPosition(Vector2 pos){
        transform.position = pos - new Vector2(0f , 0.5f);
        transform.position += Vector3.forward * 6f;
    }
}
