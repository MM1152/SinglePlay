using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPostion : MonoBehaviour
{
    TestAnimation testAnimation;
    Animator ani;
    private void Awake() {
        testAnimation = transform.parent.GetComponent<TestAnimation>();
        ani = GetComponent<Animator>();
    }

    private void Update() {
        ani.SetFloat("moveX" , testAnimation.targetpos.x);
        ani.SetFloat("moveY" , testAnimation.targetpos.y);
        if(testAnimation.canAttack) {
            ani.SetBool("isAttack" , true);
        }else {
            ani.SetBool("isAttack" , false);
        }
    }

}
