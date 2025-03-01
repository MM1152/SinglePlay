using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingTutorial : MonoBehaviour
{
    void Awake()
    {
        if(transform.GetSiblingIndex() == 14) {
            GameManager.Instance.ResumeGame();
            GameManager.Instance.GoBossMapTutorial();
        }
    }
}
