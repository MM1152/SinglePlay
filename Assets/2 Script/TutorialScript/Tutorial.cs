using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    //\\TODO : Tutorial을 원하는 버튼이 클릭 되면 그때 실행시키기로
    public SettingTutorial[] tutorials;
    public int pastTutorialIndex = -1;
    public void StartTutorial(int index){
        if(pastTutorialIndex != -1) {
            transform.GetChild(pastTutorialIndex).gameObject.SetActive(false);
        }
        Debug.Log($"StartTutorial {index}");
        transform.GetChild(index).gameObject.SetActive(true);
        pastTutorialIndex = index;
    }
}
