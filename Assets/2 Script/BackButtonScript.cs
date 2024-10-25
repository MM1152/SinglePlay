using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BackButtonScript : MonoBehaviour
{
    GameObject disableObject;
    Button bnt;

    private void Awake(){
        bnt = GetComponent<Button>();
        disableObject = transform.parent.gameObject;
        
        bnt.onClick.AddListener(delegate {DisAble();});
        bnt.onClick.AddListener(delegate {GameManager.Instance.ResumeGame();});
    }   

    void DisAble(){
        disableObject.SetActive(false);
    }

}
