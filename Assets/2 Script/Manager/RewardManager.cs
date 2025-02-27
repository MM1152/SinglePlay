using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour , ISpawnPosibillity
{
    public static RewardManager Instance{get ; private set ;}
    public float spawnProbabillity { get ; set ; }
    public GameObject rewardViewer;

    public delegate void Function(string key , float value); 
    public Function SetSummonerStat; // 리워드 선택시 Player의 GetReward 함수를 호출하도록 구현

    [SerializeField] List<float> probabillityList = new List<float>();
    [SerializeField] ClearRewardData[] rewardData;
    
    
    MergeSort<ClearRewardData> sort;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        rewardData = Resources.LoadAll<ClearRewardData>("RewardData");
        sort = new MergeSort<ClearRewardData>(rewardData);
        rewardData = sort.get();
        SettingProbabillity();
    }
    
    public void ShowRewardView(){
        rewardViewer.SetActive(true);
    }

    public ClearRewardData GetRewardData(){
        float value = 0;
        float item = Random.Range(0f , 1f);
        bool[] already = new bool[rewardData.Length];
        for(int i = 0; i < probabillityList.Count; i++) {
            value += probabillityList[i];
            if(already[i]) {
                i++;
                value += probabillityList[i];
            }
            if(value >= item) return rewardData[i];
        }

        return null;
    }

    void SettingProbabillity(){
        float probabillitySum = 0;

        foreach(ClearRewardData probabillity in rewardData) {
            probabillitySum += probabillity.spawnProbabillity;
        }   

        for(int i= 0 ; i< rewardData.Length; i++){
            probabillityList.Add(rewardData[i].spawnProbabillity / probabillitySum);
        }
    }
}
