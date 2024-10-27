using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ProjecTile : MonoBehaviour
{
    public UnitData unitData;
    public Transform target;

    private Rigidbody2D rg;
    private Vector3 direction;
    
    private void Awake() {
        rg = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        rg.AddForce(direction  , ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Wall")) {
            PoolingManager.Instance.ReturnObject("Projectile" , gameObject);

        }
        else if(other.GetComponent<IDamageAble>() != null && !other.CompareTag(gameObject.tag) && !other.GetComponent<Unit>().isDie) {       
            Debug.Log($"Get OtherIDamageAble : {other.GetComponent<IDamageAble>()}");     
            other.GetComponent<IDamageAble>().Hit(unitData.damage);
            PoolingManager.Instance.ReturnObject("Projectile" , gameObject);
        }
    }

    public void SetDirecetion(){
        direction = (target.transform.position - transform.position).normalized;
    }
}
