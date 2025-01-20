using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using JetBrains.Annotations;
using UnityEngine;
public class GoogleAdMobs : MonoBehaviour
{
    #if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3940256099942544/5224354917";
    #elif UNITY_IPHONE
        private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
    #else
        private string _adUnitId = "unused";
    #endif
    private static GoogleAdMobs _instance;
    public static GoogleAdMobs instance {
        get {
            return _instance;
        }
    }

    private RewardedAd _rewardedAd;

    void Awake(){
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });

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
            //RegisterEventHandlers(_rewardedAd);
        });

    }

    public void ShowRewardedAd(Action callback)
    {
        LoadRewardAds();
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing AD");
            _rewardedAd.Show((Reward reward) => {
                callback();
            });
        }else {
            Debug.LogError("Fail to Load ADs");
        }
    }
    
    private void RegisterEventHandlers(RewardedAd ad)
    {

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                       "with error : " + error);
        };
    }
    
}
