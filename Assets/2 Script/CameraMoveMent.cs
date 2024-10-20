using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField] Transform target; // Player를 담은 부모오브젝트 , 항상 첫번째 자식이 카메라의 타깃이 됌
    public float smooting;
    void Awake()
    {
        target = target.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position , new Vector2(target.position.x , target.position.y) , smooting);
        transform.position += Vector3.back * 10f;
    }
}
