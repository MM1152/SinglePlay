using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "SellingItemData", menuName = "SellingItemData", order = 0)]
public class SellingItemData : ScriptableObject
{
    public Sprite sprite;
    public ItemClass itemClass;
    public ClassStruct classStruct;
    public float spawnProbabillity;
    public float sellingTypeToSoul;
    public float sellingTypeToGem;    
}
