using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectReward : MonoBehaviour
{
    //#issue 1
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
    //:fix reward 데이터 설정 전에 OnEnable 실행으로 자꾸 오류뜨는거같음 해결 필요
    private void OnEnable() {
        SetRewardData();
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
            //\\TODO 능력치 적용시켜주는 로직 필요 어떤식으로 구현해야되지 하..
            Debug.Log(rewardData.type.ToString());
            RewardManager.Instance.SetSummonerStat.Invoke(rewardData.type.ToString() , rewardData.percent);
            transform.parent.gameObject.SetActive(false);
        }
    }
    public void SetRewardData() {
        rewardImage.sprite = rewardData.image;
    }
}
