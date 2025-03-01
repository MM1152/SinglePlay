using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    public void Stop(){
        GameManager.Instance.StopGame();
    }
    public void Resume() {
        GameManager.Instance.ResumeGame();
        if(GameManager.Instance.isPlayingTutorial) {
            GameManager.Instance.StartTutorial(12);
        }
    }
}
