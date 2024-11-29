using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ShowFPS : MonoBehaviour
{
    Thread thread;
    float deltaTime;
    Text text;
    float fps;
    private void Awake() {
        text = GetComponent<Text>();
        thread = new Thread(SetFPS);
        thread.Start();
    }
    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        text.text = $"{fps} FPS";
        
    }
    public void SetFPS(){
        while(true) {
            fps = 1.0f / deltaTime;
            Thread.Sleep(1000);
        }
       
    }
}
