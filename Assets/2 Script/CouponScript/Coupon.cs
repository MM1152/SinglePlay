using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Coupon : MonoBehaviour
{
    [SerializeField] InputField input;
    [SerializeField] Button couponButton;
    [SerializeField] GameObject sussecsCoupon;
    [SerializeField] GameObject acquireCoupon;
    [SerializeField] GameObject failCoupon;
    public static CouponInfo[] InitCoupons = new CouponInfo[] {
        new CouponInfo("LeeJuNo" , "gem" , 500)
    };
    string inputText;

    void Awake()
    {
        couponButton.onClick.AddListener(() => CkeckCoupon(inputText));
        gameObject.SetActive(false);
    }

    void Update()
    {
        inputText = input.text;
    }

    void CkeckCoupon(string coupon)
    {
        CouponData couponData = GameDataManger.Instance.GetCouponData();
        for (int i = 0; i < couponData.couponInfo.Count; i++)
        {
            if (couponData.couponInfo[i].couponId == coupon && !couponData.couponInfo[i].isAcquire)
            {
                sussecsCoupon.SetActive(true);
                couponData.couponInfo[i].isAcquire = true;

                if(couponData.couponInfo[i].giftType == "gem") {
                    GameDataManger.Instance.GetGameData().gem += couponData.couponInfo[i].value;
                }
                else if(couponData.couponInfo[i].giftType == "soul") {
                    GameDataManger.Instance.GetGameData().soul += couponData.couponInfo[i].value;
                }

                GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
                GameDataManger.Instance.SaveCouponData();
                return;
            }
            else if (couponData.couponInfo[i].couponId == coupon
                && couponData.couponInfo[i].isAcquire)
            {
                acquireCoupon.SetActive(true);
                return;
            }
        }
        failCoupon.SetActive(true);
    }
}

