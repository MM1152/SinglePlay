using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapInfomationTab : MonoBehaviour
{
    [SerializeField] Button enterButton;
    [SerializeField] Text infomationText;
    [SerializeField] Transform getSoulListTransform;
    [SerializeField] GameObject rewardPrefeb;
    public string name;
    public string infomation;
    public int maxStage;
    public int mapindex;
    public float obtainablegoods;

    private void OnEnable() {
        infomationText.text = infomation;
        Unit[] enemys = Resources.LoadAll<Unit>(name + "Enemy");

        SettingReward reward = Instantiate(rewardPrefeb , getSoulListTransform).GetComponent<SettingReward>(); // 소울 소환
        
        for(int i = 0; i < enemys.Length; i++) {
            SoulsInfo soulInfo = Instantiate(SoulsManager.Instance.soulsInfos[enemys[i].unit.typenumber - 1].gameObject , getSoulListTransform).GetComponent<SoulsInfo>(); 
            soulInfo.levelText.transform.position += Vector3.down * 35f;
        }

        
    }
    private void OnDisable() {
        foreach(Transform soulinfo in getSoulListTransform ) {
            Destroy(soulinfo.gameObject);
        }
    }
    void Awake()
    {
        enterButton.onClick.AddListener(EnterMap);
    }
    void EnterMap(){
        GameManager.Instance.mapName = name;
        GameManager.Instance.maxStage = maxStage;
        GameManager.Instance.mapindex = mapindex;
        GameManager.Instance.obtainablegoods = obtainablegoods;
        GameManager.Instance.ReturnToMain(name + "Scene");
    }
}
