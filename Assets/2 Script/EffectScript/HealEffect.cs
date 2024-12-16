using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    Animator ani;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(WaitForHealAnimation());
    }
    IEnumerator WaitForHealAnimation() {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        gameObject.SetActive(false);
    }
}
