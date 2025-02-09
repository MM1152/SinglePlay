using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingTabButton : MonoBehaviour
{
    private bool _isSelect;
    public bool isSelect { 
        get { return _isSelect; }
        set {
            _isSelect = value;
            ChangeImage();
            settingTab.ClickButton(this);
        }
    }
    [SerializeField] GameObject toggleImage;
    SettingTab settingTab;
    Button button;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        settingTab = GameObject.FindAnyObjectByType<SettingTab>();

        button.onClick.AddListener(() => {
            isSelect = !isSelect;
            Debug.Log(this.gameObject.name);
            PlayerPrefs.SetInt(this.gameObject.name , isSelect == true ? 1 : 0);
        });
    }
    void ChangeImage(){
        toggleImage = transform.Find("ToggleImage").gameObject;
        toggleImage.SetActive(isSelect);
    }
}
