using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDarkerAttack : MonoBehaviour
{
    Animator ani;
    public Transform target;

    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(EndforAnimation());
    }
    private void Update() {
        transform.position = target.transform.position;
    }
    IEnumerator EndforAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
