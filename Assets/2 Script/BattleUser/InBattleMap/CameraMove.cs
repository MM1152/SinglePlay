using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform target;
    public float smooth;

    public void Setting(Transform target){
        this.target = target;
    }

    void Update()
    {
        if(target != null) {
            transform.position = Vector3.Lerp(transform.position , target.position , Time.deltaTime * smooth);
            transform.position += Vector3.back * 10f;
        }
    }
}
