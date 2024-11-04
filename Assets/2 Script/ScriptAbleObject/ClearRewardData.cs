using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
    HP , DAMAGE , SPEED , ATTACKSPEED
}

[CreateAssetMenu(fileName = "RewardData" , menuName = "RewardData")]
public class ClearRewardData : ScriptableObject , ISpawnPosibillity
{
    public Type type;
    public Sprite image;
    [TextArea] public string explain;
    public float percent;
    public float _spawnProbabillity;
    public float spawnProbabillity { get => _spawnProbabillity; set => _spawnProbabillity = value; }
    
}
