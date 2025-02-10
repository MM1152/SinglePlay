using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingTab : MonoBehaviour
{
    public SettingTabButton showHpButton;
    public SettingTabButton showDamageButton;
    public SettingTabButton mainVolumButton;
    public SettingTabButton bgmVolumButton;
    public SettingTabButton sfxVolumButton;
    private void Start() {
        if(PlayerPrefs.HasKey(showHpButton.gameObject.name)) showHpButton.isSelect = PlayerPrefs.GetInt(showHpButton.gameObject.name) == 1 ? true : false;
        if(PlayerPrefs.HasKey(showDamageButton.gameObject.name)) showDamageButton.isSelect = PlayerPrefs.GetInt(showDamageButton.gameObject.name) == 1 ? true : false;
        if(PlayerPrefs.HasKey(mainVolumButton.gameObject.name)) mainVolumButton.isSelect = PlayerPrefs.GetInt(mainVolumButton.gameObject.name) == 1 ? true : false;
        if(PlayerPrefs.HasKey(bgmVolumButton.gameObject.name)) bgmVolumButton.isSelect = PlayerPrefs.GetInt(bgmVolumButton.gameObject.name) == 1 ? true : false;
        if(PlayerPrefs.HasKey(sfxVolumButton.gameObject.name)) sfxVolumButton.isSelect = PlayerPrefs.GetInt(sfxVolumButton.gameObject.name) == 1 ? true : false;

        gameObject.SetActive(false);
    }
    public void ClickButton(SettingTabButton clickButtonInfo){
        if(clickButtonInfo.gameObject.name == "MainSound" && clickButtonInfo.isSelect) {
            bgmVolumButton.GetComponent<Button>().interactable = false;
            sfxVolumButton.GetComponent<Button>().interactable = false;
        } else if(clickButtonInfo.gameObject.name == "MainSound" && !clickButtonInfo.isSelect) {
            bgmVolumButton.GetComponent<Button>().interactable = true;
            sfxVolumButton.GetComponent<Button>().interactable = true;
        }
        SoundManager.Instance.Play();
    }

}
