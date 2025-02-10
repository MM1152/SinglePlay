using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButtonScript : MonoBehaviour
{
    GameObject disableObject;
    Button bnt;

    private void Start(){
        bnt = GetComponent<Button>();
        disableObject = transform.parent.gameObject;
        
        bnt.onClick.AddListener(DisAble);
        bnt.onClick.AddListener(GameManager.Instance.ResumeGame);
    }   

    void DisAble(){
        disableObject.SetActive(false);
    }

}
