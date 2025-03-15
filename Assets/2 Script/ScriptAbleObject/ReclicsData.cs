using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum ItemClass{
    COMMON = 50 , UNCOMMON = 30 , RARE = 20, UNIQUE  = 8 , LEGENDARY = 3
}

public struct ClassStruct {
    public readonly ItemClass itemClass;
    public readonly float soulInintPercent;
    public readonly float soulLevelUpPercent;
    public readonly float dropSoulpercent;
    public readonly float battlePointPercent;
    public readonly float initCost;
    public readonly float levelUpCost;
    public readonly int soulCost;
    public readonly int gemCost;
    public readonly Color thisItemColor; //= Color.gray;
    
    public ClassStruct(ItemClass itemClass) {
        this.itemClass = itemClass; 
        
        switch(this.itemClass) {
            default :
                dropSoulpercent = 0f;
                soulInintPercent = 0f;
                soulLevelUpPercent = 0f;
                initCost = 0f;
                levelUpCost = 0f;
                battlePointPercent = 0f;
                thisItemColor = Color.clear;
                soulCost = 0;
                gemCost = 0;
                break;
            case ItemClass.COMMON :
                dropSoulpercent = 0.015f;
                soulInintPercent = 30f;
                soulLevelUpPercent = 5f;
                initCost = 40f;
                levelUpCost = 2.1f;
                battlePointPercent = 1f;
                thisItemColor = Color.white;
                soulCost = 500;
                gemCost = 10;
                break;
            case ItemClass.UNCOMMON :
                dropSoulpercent = 0.008f;
                soulInintPercent = 40f;
                soulLevelUpPercent = 8f;
                initCost = 80f;
                levelUpCost = 2.2f;
                battlePointPercent = 1.2f;
                thisItemColor = Color.green;
                soulCost = 1000;
                gemCost = 20;
                break;
            case ItemClass.RARE :
                dropSoulpercent = 0.003f;
                soulInintPercent = 50f;
                soulLevelUpPercent = 10f;
                initCost = 120f;
                levelUpCost = 2.3f;
                battlePointPercent = 1.5f;
                thisItemColor = new Color(0.3443396f , 0.3443396f , 1f);
                soulCost = 2500;
                gemCost = 50;
                break;
            case ItemClass.UNIQUE :
                dropSoulpercent = 0.0008f;
                soulInintPercent = 65f;
                soulLevelUpPercent = 12f;
                initCost = 160f;
                levelUpCost = 2.4f;
                battlePointPercent = 2.0f;
                thisItemColor = new Color(0.8257728f , 0 , 0.8301887f);
                soulCost = 5000;
                gemCost = 100;
                break;
            case ItemClass.LEGENDARY :
                dropSoulpercent = 0.0002f;
                soulInintPercent = 80f;
                soulLevelUpPercent = 17f;
                initCost = 250f;
                levelUpCost = 2.5f;
                battlePointPercent = 3f;
                thisItemColor = Color.yellow;
                soulCost = 10000;
                gemCost = 200;
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
