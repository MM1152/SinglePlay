using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField] Transform target; // Player를 담은 부모오브젝트 , 항상 첫번째 자식이 카메라의 타깃이 됌
    [SerializeField] Transform nextMapHole;
    [SerializeField] Transform boss;
    public float smooting;
    void Awake()
    {
        target = target.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.playingShader){
            transform.position = Vector2.Lerp(transform.position , nextMapHole.position , smooting);
            transform.position += Vector3.back * 10f;
        }else if(GameManager.Instance.playingAnimation){
            boss ??= GameObject.FindWithTag("Boss").transform;
            transform.position = Vector2.Lerp(transform.position , nextMapHole.position , smooting);
            transform.position += Vector3.back * 10f;
        }else {
            transform.position = Vector2.Lerp(transform.position , target.position , smooting);
            transform.position += Vector3.back * 10f;
        }
    }
}
