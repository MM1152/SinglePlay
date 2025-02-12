using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text text;
    string color;
    public Transform target {
        set {
            transform.position = value.position;    
        }
    }
    float limitTime;
    private float _damage;
    
    public float damage {
        get { return _damage; }
        set {
            _damage = value;
            limitTime = 0;
            if(value == 0) text.text = $"<color={color}> Dodge </color>";
            else text.text = $"<color={color}>" + _damage + "</color>";
            StartCoroutine(DamageAnimation());
        }
    }
    private void Update() {
        limitTime += Time.deltaTime;
        if(limitTime >= 5f) Destroy(this);
    }
    IEnumerator DamageAnimation(){
        for(int i = 0; i < 10f; i++) {
            transform.position += Vector3.up * 0.05f;
            transform.localScale -= Vector3.one * 0.05f;
            yield return new WaitForSeconds(0.05f);
        }
        
        transform.localScale =  Vector3.one;
        PoolingManager.Instance.ReturnObject(gameObject.name , gameObject);
    }

    public void Setting(AttackType attackType){
        switch(attackType) {
            case AttackType.None : 
                color = "red";
                break;
            case AttackType.CriticalAttack :
                color = "yellow";
                break;
            case AttackType.SkillAttack :
                color = "purple";
                break;
            case AttackType.Dodge :
                color = "black";
                break;
            case AttackType.Burn :
                color = "orange";
                break;
        }
    }
}
