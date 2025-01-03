using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    private Animator ani;
    public Summoner summoner;
    private void Awake() {
        ani = GetComponent<Animator>();
    }
    private void OnEnable() {
        StartCoroutine(WaitForAnimation());

        float distance = Vector2.Distance(summoner.target.transform.position , summoner.transform.position);
        transform.localScale = new Vector3(distance / 2 - 0.8f, 1f , 1f);
        transform.position = (summoner.target.transform.position + summoner.transform.position) / 2;

        Vector2 direction = (summoner.target.transform.position - transform.position).normalized;
        transform.right = direction;
    }   

    IEnumerator WaitForAnimation(){
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        PoolingManager.Instance.ReturnObject(gameObject.name , this.gameObject);
    } 

}
