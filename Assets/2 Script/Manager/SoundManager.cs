using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //\\TODO : 배경 음악 , 공격 효과음 , 스킬 효과음 구성

    [Header("BGM")]
    public AudioClip[] bgm;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfx;
    public float sfxVolume;
    AudioSource[] sfxPlayer;

    public enum BGM { Menu , Main };
    public enum SFX { Open , DisOpen , BuyItem , SelectItem  , MinePlant , Hit , Dodge};
    //\\TODO : 소울 획득 브금 , 젬 획득 브금 , 스킬 사용브금 등 추가 
    private static SoundManager _instance;
    public static SoundManager Instance {
        get { return _instance; }
    }

    private void Awake() {
        if(_instance == null) {
            _instance = this;   
            DontDestroyOnLoad(this);
            Init();
            sfxVolume = 0.5f;
        }
        else {
            Destroy(this);
        }
    }
    public void Play(SFX index){
        if(GameManager.Instance.setting.sfxVolumButton.isSelect || GameManager.Instance.setting.mainVolumButton.isSelect) {
            for(int i = 0 ; i < sfxPlayer.Length; i++) {
                if(sfxPlayer[i].isPlaying) sfxPlayer[i].Stop();
            }
            return;
        }

        for(int i = 0 ; i < sfxPlayer.Length; i++) {
            if(sfxPlayer[i].isPlaying) continue;

            sfxPlayer[i].clip = sfx[(int) index];
            sfxPlayer[i].Play();
            break;
        }
    }
    public void Play(){
        if(GameManager.Instance.setting.bgmVolumButton.isSelect || GameManager.Instance.setting.mainVolumButton.isSelect) {
            bgmPlayer.Stop();
            return;
        }
        if(bgmPlayer.isPlaying) return;
        bgmPlayer.Play();
    }

    private void Init(){
        GameObject bgm = new GameObject("BGM");
        GameObject sfx = new GameObject("SFX");

        bgm.transform.SetParent(transform);
        sfx.transform.SetParent(transform);

        bgm.AddComponent<AudioSource>();
        bgmPlayer = bgm.GetComponent<AudioSource>();
        bgmPlayer.loop = true;
        bgmPlayer.playOnAwake = false;
        

        sfxPlayer = new AudioSource[this.sfx.Length];
        for(int i = 0 ; i < this.sfx.Length; i++) {
            sfxPlayer[i] = sfx.AddComponent<AudioSource>();
            sfxPlayer[i].loop = false;
            sfxPlayer[i].playOnAwake = false;
            sfxPlayer[i].volume = 0.2f;
        }
        
        
        bgmPlayer.clip = this.bgm[0];
        bgmPlayer.volume = 0.3f;
        bgmPlayer.Play();
    }
    
}
