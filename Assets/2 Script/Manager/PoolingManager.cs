using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;
    public static PoolingManager Instance {
        get {
            return instance;
        }
    }
    Dictionary<string , Queue<GameObject>> pools = new Dictionary<string , Queue<GameObject>>();
    
    [SerializeField] GameObject prefeb;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public GameObject ShowObject(string objectName){
        if(!pools.ContainsKey(objectName)) pools.Add(objectName , new Queue<GameObject>());

        GameObject poolingObj = null;
        if(pools[objectName].Count > 0) {
            poolingObj = pools[objectName].Dequeue();
        }
        else {
            poolingObj = Instantiate(prefeb);
        }
        poolingObj.transform.SetParent(transform.root);
        poolingObj.SetActive(true);
        return poolingObj;
    }


    public GameObject ShowObject(string objectName , GameObject prefebs){
        if(!pools.ContainsKey(objectName)) pools.Add(objectName , new Queue<GameObject>());

        GameObject poolingObj = null;
        if(pools[objectName].Count > 0) {
            poolingObj = pools[objectName].Dequeue();
        }
        else {
            poolingObj = Instantiate(prefebs);
        }
        poolingObj.transform.SetParent(transform.root);
        poolingObj.SetActive(true);
        return poolingObj;
    }
    public void ReturnObject(string objectName , GameObject returnObj){
        returnObj.transform.SetParent(this.transform);
        returnObj.SetActive(false);
        pools[objectName].Enqueue(returnObj);
    }
}
