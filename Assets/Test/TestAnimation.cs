using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    [SerializeField] GameObject target;
    Animator ani;
    public bool canAttack = false;
    public Vector2 targetpos;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void Update() {
        if(target != null) {
            targetpos = (target.transform.position - transform.position).normalized;
            ani.SetFloat("moveX" , targetpos.x);
            ani.SetFloat("moveY" , targetpos.y);

            if(Vector2.Distance(target.transform.position , transform.position) > 1f){
                transform.position += (Vector3)targetpos * Time.deltaTime;
                ani.SetBool("isMove" , true);
            }else {
                canAttack = true;
                ani.SetBool("isMove" , false);
            }
            
        }
    }
}
