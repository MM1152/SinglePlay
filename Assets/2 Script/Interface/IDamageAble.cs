using UnityEngine;

public interface IDamageAble {
    void Hit(float Damage , Unit unit , float Critical = 0  , AttackType attackType = AttackType.None );
}