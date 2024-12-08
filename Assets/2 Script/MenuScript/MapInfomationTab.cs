using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapInfomationTab : MonoBehaviour
{
    [SerializeField] Button enterButton;
    public string name;
    // Start is called before the first frame update
    void Awake()
    {
        enterButton.onClick.AddListener(EnterMap);
    }
    void EnterMap(){
        GameManager.Instance.mapName = name;
        GameManager.Instance.ReturnToMain();
    }
}
