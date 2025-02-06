using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectReward : MonoBehaviour
{
    public ClearRewardData rewardData;
    [SerializeField] Text explanationText;
    [SerializeField] Text tierText;
    Button bnt;
    Image rewardImage;
    Outline outline;

    bool _isSelect;

    public bool isSelect
    {
        get { return _isSelect; }
        set
        {
            _isSelect = value;
            if (_isSelect) outline.effectColor = new Color(1, 0, 0, 1);
            else outline.effectColor = new Color(1, 0, 0, 0);
        }
    }

    private void Awake() {
        bnt = GetComponent<Button>();
        outline = GetComponent<Outline>();
        rewardImage = transform.GetChild(0).GetComponent<Image>();
        
        bnt.onClick.AddListener(() => GetReward());

    }

    private void Update(){
        if(EventSystem.current.currentSelectedGameObject == this.gameObject) {
            isSelect = true;
        }else {
            isSelect = false;
        }
    }
    
    private void GetReward(){
        if(isSelect && Input.GetTouch(0).tapCount >= 2){
            for(int i = 0; i < rewardData.type.Length; i++) {
                RewardManager.Instance.SetSummonerStat.Invoke(rewardData.type[i].ToString() , rewardData.percent);
            }
            transform.parent.gameObject.SetActive(false);
            rewardData = null;
        }
    }
    public void SetRewardData(ClearRewardData data) {
        rewardData = data;
        rewardData.classStruct = new ClassStruct(data.itemClass);
        explanationText.text = ChangeWord(data);
        tierText.color = rewardData.classStruct.thisItemColor;
        tierText.text = rewardData.itemClass.ToString();
        rewardImage.sprite = rewardData.image;
    }

    private string ChangeWord(ClearRewardData data){ 
        int first = data.explain.IndexOf("percent");
        if(first != -1){
            return data.explain.Replace("percent" , ((float)data.GetType().GetField("percent").GetValue(data) * 100f).ToString());
        } else {
            return "Fain To Change Word , Response GM To Email";
        }
    }
}
