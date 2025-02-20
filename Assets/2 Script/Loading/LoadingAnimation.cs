using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    [SerializeField] Transform mine;
    [Range(0f , 1f)] public float delay;
    
    Animator ani;
    bool oneTime;
    bool playMoveSFX;
    float moveTime;
    private void Awake() {
        ani = GetComponent<Animator>();
        moveTime = 0.5f;
        playMoveSFX = true;
    }
    private void Update() {
        mine.position = Vector3.zero + new Vector3(-0.1599998f ,-1.97f , 0f);

        if(playMoveSFX) moveTime -= Time.deltaTime;
        
        if(moveTime <= 0) {
            moveTime = 0.5f;
            SoundManager.Instance.Play(SoundManager.SFX.StepRock);
        }
        
        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= delay && !oneTime) {
            oneTime = true;
            playMoveSFX = false;
            SoundManager.Instance.Stop(SoundManager.SFX.StepRock);
            SoundManager.Instance.Play(SoundManager.SFX.MinePlant);
            SoundManager.Instance.sfxVolume = 0.2f;
            
        } else if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= delay + 0.4f) {
            playMoveSFX = true;
        }
        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
            oneTime = false;
            ani.Play("LoadingAnimation" , 0 , 0f);
        } 
    }
}
