using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InWarningArea : MonoBehaviour
{
    public Unit unit;
    public float percent;    
    public float attackDelay = 1.0f;
    
    private GameObject hitObject;
    public void Setting(float percent , float attackDelay , Unit unit){
        this.percent = percent;
        this.attackDelay = attackDelay; 
        this.unit = unit;
    }

    private void Update() {
        attackDelay -= Time.deltaTime;
        if(attackDelay <= 0f) {
            if(hitObject != null) hitObject.GetComponent<IDamageAble>().Hit(unit.damage * percent);
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            hitObject = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
         if(other.CompareTag("Player")) {
            hitObject = null;
        }
    }
    public void SetPosition(Vector2 pos){
        transform.position = pos;
    }
}
