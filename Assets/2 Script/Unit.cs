
using UnityEngine;


public class Unit : MonoBehaviour {
    [SerializeField] protected UnitData unit;
    [SerializeField] protected float setInitAttackSpeed; // 초기화될 공격속도
    [SerializeField] protected float currentAttackSpeed; // 현재 공격까지 남은시간
    /************Targeting***************/
    protected GameObject target; // 공격할 대상
    [SerializeField] protected GameObject targetList; // 적이라면 Player를 담고있는 부모 , Player라면 적에 대한 정보를 담고있는 부모
    /************************************/

    public void Awake() {
        setInitAttackSpeed = unit.attackSpeed;
        Debug.Log("Update in " + this.gameObject.name);
    }

    public void Update() {
        currentAttackSpeed -= Time.deltaTime;

    }

    protected void FollowTarget(){
        if(target == null) target = FindTarget(targetList);
        if(Vector2.Distance(target.transform.position , transform.position) < unit.attackRadious) return;
        
        transform.position += (target.transform.position - transform.position).normalized * unit.speed * Time.deltaTime;
    }

    protected GameObject FindTarget(GameObject TargetList){
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

    protected void Attack(){
        if(unit.attackRadious > Vector2.Distance(target.transform.position , transform.position) && currentAttackSpeed <= 0) {
            currentAttackSpeed = setInitAttackSpeed;
            if(unit.attackType == AttackType.LongRange) {
                GameObject attackObj = PoolingManager.Instance.ShowObject();
                ProjecTile projecTile = attackObj.GetComponent<ProjecTile>();
                
                projecTile.tag = gameObject.tag;
                projecTile.target = target.transform;
                projecTile.unitData = unit;
                attackObj.transform.position = this.gameObject.transform.position;
                
                projecTile.SetDirecetion();
                attackObj.SetActive(true);
            }
            
        }
    }
}