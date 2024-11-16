using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemClass{
    COMMON = 50 , UNCOMMON = 30 , RARE = 15, UNIQUE  = 5 , LEGENDARY = 1
}

[CreateAssetMenu (menuName = "ReclicsData")]
public class ReclicsData : ScriptableObject
{
    public ItemClass itemclass;
    public Sprite image;
    public int reclicsType;
    [TextArea]
    public string reclicsExplain;
    public float inItPercent;
    public float levelUpPercent;
}
