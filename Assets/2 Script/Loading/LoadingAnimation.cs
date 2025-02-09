using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] Transform mine;
    [Range(0f , 1f)] public float delay;
    
    Animator ani;
    bool oneTime;
    private void Awake() {
        ani = GetComponent<Animator>();
       
    }
    private void Update() {
        mine.position = Vector3.zero + new Vector3(-0.1599998f ,-1.97f , 0f);

        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= delay && !oneTime) {
            oneTime = true;
            SoundManager.Instance.Play(SoundManager.SFX.MinePlant);
            SoundManager.Instance.sfxVolume = 0.2f;
        } 
        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            oneTime = false;
            ani.Play("LoadingAnimation" , 0 , 0f);
        } 
    }
}
