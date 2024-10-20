using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Summoner : Unit , IDamageAble
{   
    
    
    public void Hit(float Damage)
    {
        unit.hp -= Damage;
    }
    public void Move(Vector3 movePos)
    {
        transform.position += movePos * Time.deltaTime;
    }
}
