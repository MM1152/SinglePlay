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

    int soul;
    GameData data;
    private void Awake()
    {
        getReward.onClick.AddListener(() =>
        {
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
            GameManager.Instance.ResumeGame();
            parent.SetActive(false);
            LoadingScene.LoadScene("MenuScene");
            if(GameManager.Instance.isPlayingTutorial) {
                GameManager.Instance.StartTutorial(20);
            }
        });

        getReward_x2.onClick.AddListener(() =>
        {
            GoogleAdMobs.instance.ShowRewardedAd(() =>
            {
                GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
                GameManager.Instance.ResumeGame();
                foreach (UnitData unitData in GameManager.Instance.dropSoulList.Keys)
                {
                    data.soulsCount[unitData.typenumber - 1] += GameManager.Instance.dropSoulList[unitData];
                    GameDataManger.Instance.GetSoul(this.soul);
                }
                parent.SetActive(false);
                LoadingScene.LoadScene("MenuScene");
                if(GameManager.Instance.isPlayingTutorial) {
                    GameManager.Instance.StartTutorial(20);
                }
            });

        });
    }
    private void OnEnable()
    {
        float bonusSoul = 1;
        if (GameDataManger.Instance.GetGameData().reclicsLevel[6] > 0)
        {
            bonusSoul += (GameManager.Instance.reclicsDatas[6].inItPercent + (GameManager.Instance.reclicsDatas[6].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[6] - 1)) / 100f;
        }

        data = GameDataManger.Instance.GetGameData();
        SettingReward soul = Instantiate(reward, transform);
        this.soul = (int)(GameManager.Instance.obtainablegoods * (GameManager.Instance.currentStage / (float)GameManager.Instance.maxStage) * bonusSoul);
        soul.Setting(soulImage, this.soul);
        GameDataManger.Instance.GetSoul(this.soul);

        foreach (UnitData unitData in GameManager.Instance.dropSoulList.Keys)
        {
            SettingReward unitSoul = Instantiate(reward, transform);
            unitSoul.Setting(unitData.image, GameManager.Instance.dropSoulList[unitData]);
            data.soulsCount[unitData.typenumber - 1] += GameManager.Instance.dropSoulList[unitData];
        }
    }
    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
