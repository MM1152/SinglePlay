using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnMainMenu : MonoBehaviour
{
    Button mainMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton = GetComponent<Button>();
        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MenuScene");
        });
    }

}
