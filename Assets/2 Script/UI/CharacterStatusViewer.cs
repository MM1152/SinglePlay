using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusViewer : MonoBehaviour
{
    private bool _isOpen;
    [SerializeField] Text percentText;
    StringBuilder sb = new StringBuilder();
    public bool isOpen {
        get { return _isOpen; }
        set {
            _isOpen = value;
            gameObject.SetActive(value);
        }
    }

    private void Awake() {
        isOpen = false;

    }
    private void OnEnable() {


        //\\TODO : 적용준인 능력치 불러와서 사용해야되는데 어떻게 불러올건지?
        //1. Summoner에서 RewardManager를 통해 능력치 추가기능 존재
        //2. GameManager를 통한 Summoner 능력치 추가 존재
        sb.Clear();
        for(int i = 0; i <= 4; i++) {
            sb.AppendLine($"{GameManager.Instance.reclicsDatas[i].inItPercent + (GameManager.Instance.reclicsDatas[i].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[i])} %");
        }
        percentText.text = sb.ToString();
    }
}
