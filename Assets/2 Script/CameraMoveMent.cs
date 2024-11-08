using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [SerializeField] Transform target; // Player를 담은 부모오브젝트 , 항상 첫번째 자식이 카메라의 타깃이 됌
    [SerializeField] Transform nextMapHole;
    [SerializeField] Transform boss;
    public float smooting;
    float size;
    void Awake()
    {
        size = Camera.main.orthographicSize;
        target = target.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.playingShader){
            transform.position = Vector2.Lerp(transform.position , nextMapHole.position , smooting);
            transform.position += Vector3.back * 10f;
        }else if(GameManager.Instance.playingAnimation){
            boss = EnemySpawner.Instance.Boss[GameManager.Instance.gameLevel / 10 - 1].transform;
            transform.position = Vector2.Lerp(transform.position , boss.position , smooting);
            transform.position += Vector3.back * 10f;
        }else if(target.GetComponent<Unit>().hp <= 0){
            GameManager.Instance.SlowGame(0.4f);
            size -= 0.01f;
            size = Math.Clamp(size , 1.5f , 5);
            Camera.main.orthographicSize = size;
        }
        else {
            transform.position = Vector2.Lerp(transform.position , target.position , smooting);
            transform.position += Vector3.back * 10f;
        }
    }
}
