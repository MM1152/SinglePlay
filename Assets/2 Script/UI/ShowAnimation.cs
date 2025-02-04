using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimation : MonoBehaviour
{
    RectTransform rect;
    private void Awake() {
        rect = GetComponent<RectTransform>();
    }
    private void OnEnable() {
        StartCoroutine(OpenAnimation());   
    }
    IEnumerator OpenAnimation(){
        float i;
        for(i = 0; i < 1.2f; i += 0.3f) {
            rect.localScale = new Vector3(i , i);
            yield return new WaitForSeconds(0.005f);
        }

        for(i = 1.2f; i >= 1.0f; i -= 0.1f) {
            rect.localScale = new Vector3(i , i);
            yield return new WaitForSeconds(0.005f);
        }
    }
}
