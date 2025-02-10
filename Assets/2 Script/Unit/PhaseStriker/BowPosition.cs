using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowPosition : MonoBehaviour
{
    Unit unit;
    SpriteRenderer sp;
    private void Awake() {
        unit = transform.parent.GetComponent<Unit>();
        sp = GetComponent<SpriteRenderer>();
    }
    private void Update() {
        sp.flipX = unit.sp.flipX;
    }
}
