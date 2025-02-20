using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : MonoBehaviour
{
    Animator ani;
    Transform target;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    public void Setting(Transform target) {
        this.target = target;
    }
    void Update()
    {
        transform.position = target.transform.position + Vector3.up;
    }
    private void OnEnable() {
        StartCoroutine(WaitForHealAnimation());
    }
    IEnumerator WaitForHealAnimation() {
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(this.gameObject.name , this.gameObject);
    }
}
