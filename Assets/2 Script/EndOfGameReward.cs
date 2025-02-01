using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfGameReward : MonoBehaviour
{
    [SerializeField] SettingReward reward;
    [SerializeField] Sprite soulImage;
    [SerializeField] GameObject parent;
    [SerializeField] Button getReward;
    [SerializeField] Button getReward_x2;

    
    private void Awake() {
        getReward.onClick.AddListener(() => {
            GameDataManger.Instance.SaveData();
            GameManager.Instance.ResumeGame();
            LoadingScene.LoadScene("MenuScene");
            parent.SetActive(false);
        });
    }
    private void OnEnable() {
        float bonusSoul = 1;
        if(GameDataManger.Instance.GetGameData().reclicsLevel[6] > 0) {
            bonusSoul += (GameManager.Instance.reclicsDatas[6].inItPercent + (GameManager.Instance.reclicsDatas[6].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[6] - 1)) / 100f;
        }
       
        GameData data = GameDataManger.Instance.GetGameData();
        SettingReward soul = Instantiate(reward , transform);

        soul.Setting(soulImage , (int)(GameManager.Instance.obtainablegoods * (GameManager.Instance.currentStage / (float) GameManager.Instance.maxStage)));
        data.soul += (int)(GameManager.Instance.obtainablegoods * (GameManager.Instance.currentStage / (float) GameManager.Instance.maxStage) * bonusSoul);
        Debug.Log((int)(GameManager.Instance.obtainablegoods * (GameManager.Instance.currentStage / (float) GameManager.Instance.maxStage)));

        foreach(UnitData unitData in GameManager.Instance.dropSoulList.Keys) {
           SettingReward unitSoul = Instantiate(reward , transform);
           unitSoul.Setting(unitData.image , GameManager.Instance.dropSoulList[unitData]); 
           data.soulsCount[unitData.typenumber - 1] += GameManager.Instance.dropSoulList[unitData];
        }
    }
    private void OnDisable(){
        foreach(Transform child in transform) {
            Destroy(child.gameObject);
        }
    }
}
