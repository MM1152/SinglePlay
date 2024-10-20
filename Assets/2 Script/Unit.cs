
using UnityEngine;


public class Unit : MonoBehaviour {
    [SerializeField] protected UnitData unit;

    /************Targeting***************/
    GameObject target; // 공격할 대상
    GameObject targetList; // 적이라면 Player를 담고있는 부모 , Player라면 적에 대한 정보를 담고있는 부모
    /************************************/


    private void FollowTarget(){
        if(target == null) target = FindTarget(targetList);
        if(Vector2.Distance(target.transform.position , transform.position) < 0.5f) return;
        
        transform.position += (target.transform.position - transform.position).normalized * unit.speed * Time.deltaTime;
    }

    private GameObject FindTarget(GameObject TargetList){
        if(target != null) return target;
        
        GameObject returnGameObject = null;
        float minDistance = 9999999f;

        foreach(Transform targets in TargetList.transform) {
            if(Vector2.Distance(targets.position , transform.position) < minDistance) {
                minDistance = Vector2.Distance(targets.position , transform.position);
                returnGameObject = targets.gameObject;
            }
        }
        
        return returnGameObject;
    }
}