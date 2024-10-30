using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    Animator ani;

    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(WaitForAnimation());
    }   

    IEnumerator WaitForAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , this.gameObject);
    } 
}
