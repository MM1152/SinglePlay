using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using JetBrains.Annotations;
using UnityEngine;
public class GoogleAdMobs : MonoBehaviour
{
    #if UNITY_ANDROID
        public bool isTest = false;
    #elif UNITY_EDITOR
        public bool isTest = true;
    #endif
    private string _adUnitId;
    private static GoogleAdMobs _instance;
    public static GoogleAdMobs instance {
        get {
            return _instance;
        }
    }

    private RewardedAd _rewardedAd;
    public bool isPlayAd;
    void Awake(){
        if(_instance == null) {
            _instance = this;
            if(isTest) {
                _adUnitId = "ca-app-pub-3940256099942544/5224354917";
            }
            else {
                _adUnitId = "ca-app-pub-8044713535911201/2430838782";
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });
        LoadRewardAds();
        
    }

    public void LoadRewardAds()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Load Ads");

        var adRequest = new AdRequest();

        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Load Error error : " + error);
                return;
            }

            Debug.Log("Ad Load response : " + ad.GetResponseInfo());

            _rewardedAd = ad;
            RegisterEventHandlers(_rewardedAd);
        });

    }

    public bool ShowRewardedAd(Action callback)
    {
        
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            isPlayAd = true;
            _rewardedAd.Show((Reward reward) => {
                DailyQuestTab.ClearDailyQuest(QuestType.PlayAds , 1);
                callback();
                isPlayAd = false;
            });
            return true;
        }else {
            return false;
        }
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);
            LoadRewardAds();
        };
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Close ad tab");
            LoadRewardAds();
        };
    }
    
}
