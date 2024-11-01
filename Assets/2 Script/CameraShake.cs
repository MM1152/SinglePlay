using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeAmout;
    

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.playingShader) {
            transform.position += Random.insideUnitSphere * shakeAmout;
        }
    }
}
