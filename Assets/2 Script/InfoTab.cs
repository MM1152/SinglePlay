using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoTab : MonoBehaviour
{
    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OpenInfoTab());
    }

    void OpenInfoTab(){
        GameManager.Instance.transform.Find("Canvas").Find("MakerInfo").gameObject.SetActive(true);
    }
}
