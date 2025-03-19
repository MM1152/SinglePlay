using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CheckBattleResult : MonoBehaviour
{
    [SerializeField] Text resultText;
    [SerializeField] GameObject playerSpawn;
    [SerializeField] GameObject enemySpawn;

    bool isFin;
    public bool isStart;
    void Update()
    {
        if(!isFin && isStart) {
            if(playerSpawn.transform.childCount == 0){
                resultText.text = "<color=red> Lose </color>"; 
                isFin = true;
            }
            else if(enemySpawn.transform.childCount == 0) {
                resultText.text = "<color=green> Win </color>";
                isFin = true;
            }
        }
        else if(isFin && isStart && !resultText.gameObject.activeSelf) resultText.gameObject.SetActive(true);
        else if(isFin && isStart && resultText.gameObject.activeSelf) {
            if(Input.touchCount > 0){
                LoadingScene.LoadScene("MenuScene");
            }
        }
    }
}
