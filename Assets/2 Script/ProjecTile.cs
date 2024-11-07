
using System.Collections;
using System.Timers;
using UnityEngine;

public class ProjecTile : MonoBehaviour
{
    SpriteRenderer sp;
    public Unit unitData;
    public Transform target;
    [Range(1f , 100f)] [SerializeField] float speed;
    private Vector2 direction;
    
    private void OnEnable() {
        StartCoroutine(WaitForReturnCorutine());
    }
    private void Awake() {
        sp = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Wall")) {
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
        else if(other.GetComponent<IDamageAble>() != null && !other.CompareTag(gameObject.tag) && !other.GetComponent<Unit>().isDie) {        
            other.GetComponent<IDamageAble>().Hit(unitData.damage);
            PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
        }
    }

    public void SetDirecetion(){
        direction = (target.transform.position - transform.position).normalized;
        transform.right = -direction;
    }

    private IEnumerator WaitForReturnCorutine(){
        yield return new WaitForSeconds(3f);
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }
}
