using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject curtain;
    
    public static GameManager Instance {get; private set;}
    public int gameLevel = 10;
    public int clearMonseter = 15;
    public bool gameClear;
    public bool playingAnimation;
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
        nextStage.SetActive(true);
    }
    public void StopGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }

    public IEnumerator WaitForNextMap(Action action) {

        yield return new WaitForSeconds(0.3f);
        clearMonseter = 15;
        curtain.SetActive(true);
        nextStage.SetActive(false);

        Image image = curtain.GetComponent<Image>();
        Vector4 color = new Vector4(0 , 0 , 0 , 1);

        yield return new WaitForSeconds(1f);

        for(float i = 1f; i >= 0f; i -= 0.1f){
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
