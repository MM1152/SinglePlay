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
    }
    IEnumerator LoadSecneAsync(){
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        while(!op.isDone) {
            if(progressBar.value <= 0.9f) {
                progressBar.value = op.progress;
            }
            if(progressBar.value >= 0.9f) {
                text.text = "화면을 터치하세요";
                yield return new WaitUntil(() => {
                    if(Input.touchCount >= 1) {
                        op.allowSceneActivation = true;
                    }
                    return Input.touchCount >= 1f;
                });
            }
            yield return null;
        }
    }
}
