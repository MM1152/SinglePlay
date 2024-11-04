using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
    HP , DAMAGE , SPEED , ATTACKSPEED
}

[CreateAssetMenu(fileName = "RewardData" , menuName = "RewardData")]
public class ClearRewardData : ScriptableObject , ISpawnPosibillity
{
    public Sprite image;
    [TextArea] public string explain;
    public float damage;
    public float hp;
    public float probabillity;
    public float _spawnProbabillity;
    public float attackSpeed;
    public float speed;
    public float spawnProbabillity { get => _spawnProbabillity; set => _spawnProbabillity = value; }
    
}
