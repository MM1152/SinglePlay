using UnityEngine;

public interface IDamageAble {
    void Hit(float Damage , AttackType attackType = AttackType.None);
}