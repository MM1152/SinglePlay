using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckVesion : MonoBehaviour
{
    [SerializeField] Text versionText;
    [SerializeField] Button button;

    public bool checkingVersion;
    void Awake()
    {
        button.onClick.AddListener(() => {
            Application.OpenURL("https://play.google.com/store/apps/details?id=com.DefaultCompany.SinglePlayGame");
            Application.Quit();
        });
    }
    public void DifferentVersion(string version){
        Debug.Log(version);
        Debug.Log(Application.version);
        if(version != Application.version) {
            Debug.Log("Version Differnt");
            versionText.text = $"현재 버전 : {Application.version}\n신규 버전 : {version}";
            gameObject.SetActive(true);
            return;
        }
            
        checkingVersion = true;
    }
}
