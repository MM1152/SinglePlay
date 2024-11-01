using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public int gameLevel = 1;
    public int clearMonseter = 15;
    public bool gameClear;
    public bool playingShader;

    public GameObject nextStage;

    private void Awake() {
        if(Instance == null) Instance = this;    
    }
    private void Update() {
        if(clearMonseter <= 0) ClearLevel();
    }
    public void ClearLevel(){
        gameClear = true;
        clearMonseter = 15;
        gameLevel++;
        nextStage.SetActive(true);
    }
    public void StopGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
}
