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
    List<int> reclicsData;
    public bool isOpen {
        get { return _isOpen; }
        set {
            _isOpen = value;
            gameObject.SetActive(value);
        }
    }

    private void Awake() {
        reclicsData = GameDataManger.Instance.GetGameData().reclicsLevel;
        isOpen = false;
    }
    private void OnEnable() {


        //\\TODO : 적용준인 능력치 불러와서 사용해야되는데 어떻게 불러올건지?
        //1. Summoner에서 RewardManager를 통해 능력치 추가기능 존재
        //2. GameManager를 통한 Summoner 능력치 추가 존재
        sb.Clear();
        
        for(int i = 0; i <= 4; i++) {
            if(reclicsData[i] > 0) {
                sb.AppendLine($"{ReturnPercent(i)} %");
            }
        }
        if(reclicsData[10] > 0) sb.AppendLine($"{ReturnPercent(10)}");
        if(reclicsData[9] > 0) sb.AppendLine($"{ReturnPercent(9)}");
        
        percentText.text = sb.ToString();
    }

    private float ReturnPercent(int index){
        return GameManager.Instance.reclicsDatas[index].inItPercent + (GameManager.Instance.reclicsDatas[index].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[index]);
    }
}
