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
    int battleScore = GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore;
    void Update()
    {
        if(!isFin && isStart) {
            if(playerSpawn.transform.childCount == 0){
                resultText.text = "<color=red> Lose </color>"; 
                battleScore = battleScore - 10 < 0 ? 0 : battleScore - 10;
                isFin = true;
            }
            else if(enemySpawn.transform.childCount == 0) {
                battleScore  += 20;
                isFin = true;
            }
        }
        else if(isFin && isStart && !resultText.gameObject.activeSelf) resultText.gameObject.SetActive(true);
        else if(isFin && isStart && resultText.gameObject.activeSelf) {
            if(Input.touchCount > 0){
                GameDataManger.Instance.GetBattleData().user[GameDataManger.Instance.battlUserIndex].battleScore = battleScore;
                GameDataManger.Instance.SaveData(GameDataManger.SaveType.BattleData);

                GameManager.Instance.connectDB.WriteBattleScore();
                LoadingScene.LoadScene("MenuScene");
            }
        }
    }
}
