using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider progressBar;
    [SerializeField] Text text;
    private static string nextScene = "MenuScene";
    private void Start() {
        StartCoroutine(LoadSecneAsync());
    }
    public static void LoadScene(string next){
        nextScene = next;
        SceneManager.LoadScene("LodingScene");

        Debug.Log($"Next Scene : {next}");

        
    }
    IEnumerator LoadSecneAsync(){

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while(!op.isDone) {
            Debug.Log(GameManager.Instance.checkVesion.checkingVersion);
            if(progressBar.value < 0.9f) {
                progressBar.value = op.progress;
            }  
            else if(progressBar.value >= 0.9f && GameManager.Instance.checkVesion.checkingVersion) {
                text.text = "화면을 터치하세요";
                yield return new WaitUntil(() => Input.touchCount >= 1f);

                op.allowSceneActivation = true;
                SoundManager.Instance.Stop(SoundManager.SFX.MinePlant);

                if(nextScene == "MenuScene") {
                    GameManager.Instance.showingMenuTools.HideOption(false);
                    if(GameManager.Instance.isPlayingTutorial && GameManager.Instance.GetTutorial() == 20) {
                        GameManager.Instance.StartTutorial(21);
                    }
                } else {
                    GameManager.Instance.showingMenuTools.HideOption(true);
                    if(GameManager.Instance.isPlayingTutorial) GameManager.Instance.StartTutorial(4);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
