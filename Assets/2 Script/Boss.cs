using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : ShortRangeScipt
{

    private void OnEnable(){
        StartCoroutine(WaitForAnimationCorutine());
    }

    IEnumerator WaitForAnimationCorutine(){
        GameManager.Instance.playingAnimation = true; 
        yield return new WaitUntil(() => ani.GetCurrentAnimatorStateInfo(0).IsName("Spawn") && ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        GameManager.Instance.playingAnimation = false;
    }
}
