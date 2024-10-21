using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager Instance { get; private set;}
    private Queue<GameObject> pool = new Queue<GameObject>();

    [SerializeField] GameObject prefeb;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public GameObject ShowObject(){
        GameObject poolingObj = null;
        if(pool.Count > 0) {
            poolingObj = pool.Dequeue();
        }
        else {
            poolingObj = Instantiate(prefeb);
        }
        poolingObj.transform.SetParent(transform.root);
        return poolingObj;
    }
    public void ReturnObject(GameObject returnObj){
        returnObj.transform.SetParent(this.transform);
        returnObj.SetActive(false);
        pool.Enqueue(returnObj);
    }
}
