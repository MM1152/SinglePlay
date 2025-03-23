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
    string[] lines;
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
            SetFailToSetNickName("7글자 이하로 작성해주세요");
            return;
        }
        for(int i = 0; i < lines.Length; i++) {
            if(input.text.Contains(lines[i])) {
                SetFailToSetNickName("비속어는 사용할 수 없습니다.");
                return;
            }
        }

        string check = Regex.Replace(input.text , @"[^a-zA-Z0-9가-힣]" , string.Empty , RegexOptions.Singleline);

        if(input.text.Equals(check)) {
            BattleDatas battleDatas = GameDataManger.Instance.GetBattleData();
            for(int i = 0; i < battleDatas.user.Count; i++) {
                if(battleDatas.user[i].userName == check) {
                    SetFailToSetNickName("중복되는 닉네임 입니다.");
                    return;
                }
            }

            GameDataManger.Instance.GetGameData().userName = input.text;
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
            GameManager.Instance.connectDB.WriteUserData();
            gameObject.SetActive(false);
        }else {
            SetFailToSetNickName("특수문자는 사용할 수 없습니다.");
        }
    }

    void SetFailToSetNickName(string text){
        failToSetNickName.text = text;
        failToSetNickName.gameObject.SetActive(true);
    }
}
