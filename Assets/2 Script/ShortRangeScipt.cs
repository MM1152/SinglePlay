using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeScipt : Unit , IDamageAble
{
    public void Awake() {
        base.Awake();
        Debug.Log($"hp {hp} , mp {mp} , damage {damage}");
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
    // Update is called once per frame
    private void Update()
    {
        FollowTarget();
        base.Update();
        Attack();
    }
}
