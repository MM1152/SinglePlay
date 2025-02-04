using UnityEngine;

public interface IDamageAble {
    void Hit(float Damage , float Critical = 0 , AttackType attackType = AttackType.None , Unit unit = null);
}