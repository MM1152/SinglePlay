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
                    hit.GetComponent<IDamageAble>().Hit(unit.damage * percent);
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
        transform.position = pos;
        transform.position += Vector3.forward * 6f;
    }
}
