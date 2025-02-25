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
                for(int i = 0; i  < hitObject.Count; i++) {
                    GameObject hit = hitObject[i];
                    
                    if(hit == null) continue;

                    IDamageAble damageable;
                    if(hit.TryGetComponent<IDamageAble>(out damageable)) {
                        damageable.Hit(unit.damage * percent , unit , unit.critical , AttackType.SkillAttack);
                    }
                }
                
            } 
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag(unit.gameObject.tag) && (other.CompareTag("Player") || other.CompareTag("Enemy"))){
            hitObject.Add(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
         if(!other.CompareTag(unit.gameObject.tag) && (other.CompareTag("Player") || other.CompareTag("Enemy"))){
            hitObject.Remove(other.gameObject);
        }
    }
    public void SetPosition(Vector2 pos){
        transform.position = pos;
        transform.position += Vector3.forward * 6f;
    }
}
