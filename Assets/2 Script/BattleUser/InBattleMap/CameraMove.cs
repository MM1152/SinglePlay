using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Unit unit;

    public float smooth;
    Func<Transform> GetNewTarget;
    public void Setting(Transform target){
        this.target = target;
        unit = target.gameObject.GetComponent<Unit>();
    }
    public void SettingAction(Func<Transform> action) {
        this.GetNewTarget = action;
    }
    void Update()
    {
        if(target != null && !unit.isDie) {
            transform.position = Vector3.Lerp(transform.position , target.position , Time.deltaTime * smooth);
            transform.position += Vector3.back * 10f;
        }
        else if(target != null && unit.isDie) {
            target = GetNewTarget?.Invoke();
        }
    }
}
