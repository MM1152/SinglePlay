using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusViewer : MonoBehaviour
{
    private bool _isOpen;
    [SerializeField] Text percentText;
    public bool isOpen {
        get { return _isOpen; }
        set {
            _isOpen = value;
            gameObject.SetActive(value);
        }
    }
    RectTransform rect;
    private void Awake() {
        isOpen = false;
        rect = GetComponent<RectTransform>();
    }
    private void OnEnable() {
        StartCoroutine(OpenAnimation());

        //\\TODO : 적용준인 능력치 불러와서 사용해야되는데 어떻게 불러올건지?
    }
    IEnumerator OpenAnimation(){
        float i;
        for(i = 0; i < 1.2f; i += 0.1f) {
            rect.localScale = new Vector3(i , i);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        for(i = 1.2f; i >= 1.0f; i -= 0.1f) {
            rect.localScale = new Vector3(i , i);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
