
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "UnitData", order = 0)]
public class UnitData : ScriptableObject {
    public string unitName;
    public float hp;
    public float mp;
    public float damage;
    public float speed;
    public float attackRadious;
}