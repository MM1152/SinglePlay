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
        Debug.Log("UPdate");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Wall")) {
            Debug.Log("Projectile Hitting Wall");
            PoolingManager.Instance.ReturnObject(gameObject);

        }
        else if(!other.CompareTag(gameObject.tag)) {            
            Debug.Log("Projectile Hitting " + other.gameObject.tag);
            other.GetComponent<IDamageAble>().Hit(unitData.damage);
            PoolingManager.Instance.ReturnObject(gameObject);
        }
    }

    public void SetDirecetion(){
        direction = (target.transform.position - transform.position).normalized;
        Debug.Log("SetDirection");
    }
}
