using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectReward : MonoBehaviour
{
    public ClearRewardData rewardData;
    [SerializeField] Text explanationText;

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
            explanationText.text = this.rewardData.explain;
        }else {
            isSelect = false;
        }

        
    }
    
    private void GetReward(){
        if(isSelect && Input.GetTouch(0).tapCount >= 2){
            Debug.Log(rewardData.type.ToString());
            RewardManager.Instance.SetSummonerStat.Invoke(rewardData.type.ToString() , rewardData.percent);
            transform.parent.gameObject.SetActive(false);
        }
    }
    public void SetRewardData(ClearRewardData data) {
        rewardData = data;
        rewardImage.sprite = rewardData.image;
    }
}
