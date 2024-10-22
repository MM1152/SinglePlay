using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeScript : Unit
{
    protected void Awake() {
        base.Awake();
        Debug.Log($"hp {hp} , mp {mp} , damage {damage}");
    }
    protected void Update() {
        base.Update();
        Attack();
        if(!VirtualJoyStick.instance.isInput) FollowTarget();
    }
    public void Hit(float Damage)
    {
        hp -= Damage;
    }
}
