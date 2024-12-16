using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void DropSoul(UnitData unitData);

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject curtain;
    public string mapName;
    public List<ReclicsData> reclicsDatas;
    public bool reclisFin;
    public Dictionary<string , UnitData> soulsInfo = new Dictionary<string, UnitData>();
    public bool soulsFin;
    public static GameManager Instance {get; private set;}
    public int gameLevel = 10;
    public int clearMonseter;
    public bool gameClear;
    public bool playingAnimation;
    public bool playingShader;

    public DropSoul dropSoul;
    public GameObject nextStage;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;    
            DontDestroyOnLoad(this);
        }
        else  {
            Destroy(this);
        }
        
    }
    private void Update() {
        if(clearMonseter <= 0) ClearLevel();
    }
    public void ClearLevel(){
        gameClear = true;
        nextStage.SetActive(true);
    }
    public void StopGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
    public void SlowGame(float size){
        Time.timeScale = size;
    }
    public void ReturnToMenu() {
        SceneManager.LoadScene("MenuScene");
        Delegate[] dele = dropSoul.GetInvocationList();
        foreach(DropSoul function in dele) {
            dropSoul -= function;
        }
        ResumeGame();
    }
    public void ReturnToMain(){
        dropSoul += delegate(UnitData unitData) {
            GameData gameData = GameDataManger.Instance.GetGameData();
            gameData.soulsCount[unitData.typenumber - 1]++;
            GameDataManger.Instance.SaveData();
        };
        SceneManager.LoadScene("MainScene");
        ResumeGame();
    }
    public IEnumerator WaitForNextMap(Action action) {

        yield return new WaitForSeconds(0.3f);
        clearMonseter = 50;
        curtain.SetActive(true);
        nextStage.SetActive(false);

        Image image = curtain.GetComponent<Image>();
        Vector4 color = new Vector4(0 , 0 , 0 , 1);

        yield return new WaitForSeconds(1f);

        for(float i = 1f; i >= 0f; i -= 0.2f){
            image.color = new Color(0 , 0 , 0 , i);
            if(i == 1f) action.Invoke();
            
            yield return new WaitForSeconds(0.2f);
        }
        
        curtain.SetActive(false);
        gameLevel++;
        
        image.color = color;
        gameClear = false;
    }
}
