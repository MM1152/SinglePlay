using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMap : MonoBehaviour , IFollowTarget
{
    public SpriteRenderer material; // 현재 스프라이트 렌더러 내에 material 접근용
    public Material showNextMapShader; // 인스턴스 만들기 용
    public float fade = 0;

    public bool canFollow { get ; set; }

    private void OnEnable() {
        fade = 0;
        GameManager.Instance.playingShader = true;
        canFollow = true;
        StartCoroutine(WaitForShader());
    }

    private void OnDisable() {
        canFollow = false;
    }

    private void Awake() {
        material = GetComponent<SpriteRenderer>();
        showNextMapShader = Instantiate(GetComponent<Renderer>().material);
        material.material = showNextMapShader;  // 복사된 material 링크 시키기 
    }

    private void Update() {
        if(fade <= 1) fade += 0.01f;
        material.material.SetFloat("_Fade" , fade );
    }

    IEnumerator WaitForShader(){
        yield return new WaitUntil(() =>  fade >= 1 );
        transform.GetChild(0).gameObject.SetActive(true);
        GameManager.Instance.playingShader = false;
    }


}
