using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wideRangeSkill : MonoBehaviour
{
    Animator ani;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(WaitForAnimationCorutine());
    }

    IEnumerator WaitForAnimationCorutine(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
