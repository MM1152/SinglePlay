using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugingTools : MonoBehaviour
{
    Transform followCharacter;
    Unit unit;
    [SerializeField] Text hp;
    [SerializeField] Text damage;
    private void Awake() {
        followCharacter = transform.parent.transform;
        unit = followCharacter.GetComponent<Unit>();
    }

    private void Update() {
        hp.text = "Hp : " + unit.hp;
        damage.text =  "Damage : " + unit.damage;
        hp.transform.position = Camera.main.WorldToScreenPoint(followCharacter.position + Vector3.up * 1.3f);
        damage.transform.position = Camera.main.WorldToScreenPoint(followCharacter.position + Vector3.up * 1f);
    }
}
