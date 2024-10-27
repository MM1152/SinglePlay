using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public int gmaeLevel = 1;

    private void Awake() {
        if(Instance == null) Instance = this;
        
    }

    public void StopGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
}
