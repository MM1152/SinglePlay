using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReRollReward : MonoBehaviour
{
    [SerializeField] Text text;
    private int _count;
    public int count
    {
        get { return _count; }
        set
        {
            _count = value;
            CheckCount();
        }
    }
    private int maxCount;
    private Button button;
    private Action action;

    void Awake()
    {
        button = GetComponent<Button>();
        

    }
    private void Start() {
        ReRollRewardAd reRollReward;
        if (TryGetComponent<ReRollRewardAd>(out reRollReward))
        {
            reRollReward.rewardAction = action;
        }
        else
        {
            button.onClick.AddListener(() =>
            {
                --count;
                action?.Invoke();
            });
        }        
    }
    void CheckCount()
    {
        if (count <= 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
        text.text = $"({count} / {maxCount})";
    }

    public void Setting(int maxCount, Action callback)
    {
        this.maxCount = maxCount;
        count = maxCount;
        action ??= callback;

        CheckCount();
    }
}
