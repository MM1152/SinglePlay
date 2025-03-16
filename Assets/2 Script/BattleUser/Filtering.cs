using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Filtering : MonoBehaviour
{
    [SerializeField] InputField input;
    [SerializeField] Text failToSetNickName;
    [SerializeField] Button button;
    public string[] lines;
    string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    TextAsset filepath;
    
    void Awake()
    {
        filepath = Resources.Load("BadWord") as TextAsset;
        StringReader stringReader = new StringReader(filepath.text);
        string source = stringReader.ReadToEnd();
        stringReader.Close();
        lines = Regex.Split(source , LINE_SPLIT_RE);

        button.onClick.AddListener(() => CheckNickName());
        
    }

    public void Open(){
        if(GameDataManger.Instance.GetGameData().userName == "" && !GameManager.Instance.isPlayingTutorial) gameObject.SetActive(true);
    }

    void CheckNickName(){
        
        if(input.text.Length > 8) {
            failToSetNickName.text = "7글자 이하로 작성해주세요";
            failToSetNickName.gameObject.SetActive(true);
            return;
        }
        for(int i = 0; i < lines.Length; i++) {
            if(input.text.Contains(lines[i])) {
                failToSetNickName.text = "비속어는 사용할 수 없습니다.";
                failToSetNickName.gameObject.SetActive(true);
                return;
            }
        }

        string check = Regex.Replace(input.text , @"[^a-zA-Z0-9가-힣]" , string.Empty , RegexOptions.Singleline);

        if(input.text.Equals(check)) {
            if(GameManager.Instance)
            GameDataManger.Instance.GetGameData().userName = input.text;
            GameDataManger.Instance.SaveData();
            GameManager.Instance.connectDB.WriteData(input.text);
            gameObject.SetActive(false);
        }else {
            failToSetNickName.text = "특수문자는 사용할 수 없습니다.";
            failToSetNickName.gameObject.SetActive(true);
        }
    }
}
