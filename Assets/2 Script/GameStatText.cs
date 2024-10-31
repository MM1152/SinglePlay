using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatText : MonoBehaviour
{
    Text clearMonsetertext;
    
    void Awake()
    {
        clearMonsetertext = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        clearMonsetertext.text = "Clear : " + GameManager.Instance.clearMonseter;
    }
}
