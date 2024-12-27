using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ItemClass{
    COMMON = 50 , UNCOMMON = 30 , RARE = 15, UNIQUE  = 5 , LEGENDARY = 1
}

public struct ClassStruct {
    public readonly ItemClass itemClass;
    public readonly float soulInintPercent;
    public readonly float soulLevelUpPercent;
    public readonly float dropSoulpercent;
    public readonly float initCost;
    public readonly float levelUpCost;
    public readonly Color thisItemColor; //= Color.gray;
    
    public ClassStruct(ItemClass itemClass) {
        Debug.Log(itemClass);
        this.itemClass = itemClass; 
        
        switch(this.itemClass) {
            default :
                dropSoulpercent = 0f;
                soulInintPercent = 0f;
                soulLevelUpPercent = 0f;
                initCost = 0f;
                levelUpCost = 0f;
                thisItemColor = Color.clear;
                break;
            case ItemClass.COMMON :
                dropSoulpercent = 0.05f;
                soulInintPercent = 30f;
                soulLevelUpPercent = 5f;
                initCost = 40f;
                levelUpCost = 2.1f;
                thisItemColor = Color.white;
                break;
            case ItemClass.UNCOMMON :
                dropSoulpercent = 0.008f;
                soulInintPercent = 40f;
                soulLevelUpPercent = 8f;
                initCost = 80f;
                levelUpCost = 2.2f;
                thisItemColor = Color.green;
                break;
            case ItemClass.RARE :
                dropSoulpercent = 0.003f;
                soulInintPercent = 50f;
                soulLevelUpPercent = 10f;
                initCost = 120f;
                levelUpCost = 2.3f;
                thisItemColor = Color.blue;
                break;
            case ItemClass.UNIQUE :
                dropSoulpercent = 0.0008f;
                soulInintPercent = 65f;
                soulLevelUpPercent = 12f;
                initCost = 160f;
                levelUpCost = 2.4f;
                thisItemColor = new Color(0.8257728f , 0 , 0.8301887f);
                break;
            case ItemClass.LEGENDARY :
                dropSoulpercent = 0.0002f;
                soulInintPercent = 80f;
                soulLevelUpPercent = 17f;
                initCost = 250f;
                levelUpCost = 2.5f;
                thisItemColor = Color.yellow;
                break;
        }
    }
}


[CreateAssetMenu (menuName = "ReclicsData")]
public class ReclicsData : ScriptableObject
{
    public ItemClass itemclass;
    public ClassStruct classStruct;
    public Sprite image;
    public int reclicsType;
    [TextArea]
    public string reclicsExplain;
    public float inItPercent;
    public float levelUpPercent;
}
