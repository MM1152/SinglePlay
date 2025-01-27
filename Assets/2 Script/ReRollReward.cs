using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReRollReward : MonoBehaviour
{
    [SerializeField] Text text;
    private int count;
    private int maxCount;
    private Button button;
    private Action action;
    
    void Awake(){
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            --count;
            action?.Invoke();
            CheckCount();
        });
    }

    void CheckCount(){
        if(count <= 0) {
            button.interactable = false;
        }else {
            button.interactable = true;
        }
        text.text = $"({count} / {maxCount})";
    }

    public void Setting(int maxCount , Action callback){
        this.maxCount = maxCount;
        count = maxCount;
        action ??= callback;
        
        CheckCount();
    }
}
