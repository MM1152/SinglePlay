using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct TierTable {
    public TierTable(string tierName , int tierScore , Color tierColor){
        this.tierName = tierName;
        this.tierScore = tierScore;
        this.tierColor = tierColor;
    }
    public string tierName;
    public int tierScore;
    public Color tierColor;
}

public class Tier : MonoBehaviour
{
    Image tierImage;
    public Text[] tierText; // Score , Name;
    TierTable[] tierTables =  {
        new TierTable("UnRank" , 100 , new Color(0.8941177f , 0.5803922f , 0.227451f)) ,
        new TierTable("Bronze" , 200 , new Color(0.5411765f , 0.2588235f , 0.0627451f)) ,
        new TierTable("Silver" , 350 , new Color(0.6196079f , 0.6f , 0.5882353f)) ,
        new TierTable("Gold" , 600 , new Color(0.8000001f , 0.5960785f , 0f)) ,
        new TierTable("Pletinum" , 800 , new Color(0.3960785f , 0.6156863f , 0.6078432f)) ,
        new TierTable("Ruby" , 9999999 , new Color(0.6588235f , 0.3137255f , 0.3529412f)) ,
    };

    int _tireScore;
    int tierScore
    {
        set
        {
            _tireScore += value;
            for(int i = 0 ; i < tierTables.Length; i++) {
                if(_tireScore < tierTables[i].tierScore) {
                    Setting(tierTables[i]);
                    break;
                }
            }
        }
    }

    
    public void Init(int score) {
        tierImage = GetComponent<Image>();
        tierText = transform.GetComponentsInChildren<Text>();
        
        this.tierScore = score;
    } 

    private void Setting(TierTable tierTable){
        tierImage.sprite = Resources.Load<Sprite>("TierImage/" + tierTable.tierName);

        if(tierText.Length > 0) {
            tierText[0].gameObject.AddComponent<Outline>().effectColor = new Color(0f , 0f , 0f , 0f);
            tierText[0].gameObject.GetComponent<Outline>().effectDistance = new Vector2(4f , 4f);
            tierText[0].text = _tireScore + "";
            tierText[0].color = tierTable.tierColor;

            tierText[1].gameObject.AddComponent<Outline>().effectColor = new Color(0f , 0f , 0f , 0f);
            tierText[1].gameObject.GetComponent<Outline>().effectDistance = new Vector2(4f , 4f);
            tierText[1].text = tierTable.tierName;
            tierText[1].color = tierTable.tierColor;
        }
    }
}
