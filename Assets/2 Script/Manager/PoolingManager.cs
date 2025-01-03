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
    [SerializeField] GameObject damageTextPrefeb;
    [SerializeField] Transform damageCanvas;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        GameObject show = ShowObject(prefeb.name+"(Clone)" , prefeb);
        ReturnObject(prefeb.name+"(Clone)" , show);
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

    public GameObject ShowDamage(){
        if(!pools.ContainsKey(damageTextPrefeb.name+"(Clone)")) pools.Add(damageTextPrefeb.name+"(Clone)" , new Queue<GameObject>());

        GameObject poolingObj = null;
        if(pools[damageTextPrefeb.name+"(Clone)"].Count > 0) {
            poolingObj = pools[damageTextPrefeb.name+"(Clone)"].Dequeue();
        }
        else {
            poolingObj = Instantiate(damageTextPrefeb);
        }
        poolingObj.transform.SetParent(damageCanvas);
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
