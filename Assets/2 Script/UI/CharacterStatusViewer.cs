using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class CharacterStatusViewer : MonoBehaviour
{
    private bool _isOpen;
    private Summoner summoner;
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
        summoner = GameObject.FindObjectOfType<Summoner>();
        isOpen = false;
    }
    private void OnEnable() {


        //   적용준인 능력치 불러와서 사용해야되는데 어떻게 불러올건지?
        //1. Summoner에서 RewardManager를 통해 능력치 추가기능 존재
        //2. GameManager를 통한 Summoner 능력치 추가 존재
        sb.Clear();

        sb.AppendLine($"{ReturnPercent(0) + ReturnAdditionalStat("DAMAGE")} %");
        sb.AppendLine($"{ReturnPercent(1) + ReturnAdditionalStat("HP")} %");
        sb.AppendLine($"{ReturnPercent(2)} % ");
        sb.AppendLine($"{ReturnPercent(3) + ReturnAdditionalStat("ATTACKSPEED")} %");
        sb.AppendLine($"{ReturnPercent(4) + ReturnAdditionalStat("SPEED")} %");
        sb.AppendLine($"{ReturnPercent(10)} %");
        sb.AppendLine($"{ReturnPercent(9)} %");

        percentText.text = sb.ToString();
    }

    private float ReturnPercent(int index){
        if(GameDataManger.Instance.GetGameData().reclicsLevel[index] > 0 || GameDataManger.Instance.GetGameData().reclicsCount[index] > 0) {
            return GameManager.Instance.reclicsDatas[index].inItPercent + (GameManager.Instance.reclicsDatas[index].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[index]);
        }
        return 0f;
    }

    private float ReturnAdditionalStat(string type) {
        if(summoner.additionalStats.ContainsKey(type)) return summoner.additionalStats[type] * 100f;
        return 0f;
    }
}
