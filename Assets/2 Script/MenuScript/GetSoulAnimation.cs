using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class GetSoulAnimation : MonoBehaviour
{

    Vector2 point1;
    Vector2 point2;
    Vector2 point3;

    Vector2 p1;
    Vector2 p2;
    RectTransform target;
    RectTransform secondTarget;

    static bool first;
    void Awake()
    {
        target = this.GetComponent<RectTransform>();
        secondTarget = transform.parent.GetComponent<RectTransform>();
    }
    public void StartGetSoulAnimation(){

        transform.position = Vector2.zero;
        
        gameObject.SetActive(true);     
        point1 = transform.position; 
        point2 = new Vector2( 0f , Random.Range(3f , 10f));
        point3 = new Vector2(Random.Range(-5f , 5f) , 0f);


        StartCoroutine(BezierCurve());
    }

    IEnumerator BezierCurve(){
        for(float i = 0; i < 1f; i += 0.025f) {
            p1 = Vector3.Lerp(point1 , point2 , i);
            p2 = Vector3.Lerp(point2 , point3 , i);
            target.position = Camera.main.WorldToScreenPoint(Vector3.Lerp(p1 , p2 , i));
            yield return new WaitForSeconds(0.005f);
        }
        SoundManager.Instance.Play(SoundManager.SFX.BuyItem);
        for(float i = 0; i < 1f; i += 0.05f) {
            target.position = Vector3.Lerp(target.position , secondTarget.position , i);
            yield return new WaitForSeconds(0.005f);
        }


        gameObject.SetActive(false);
    }
}
