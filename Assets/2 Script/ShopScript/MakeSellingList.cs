using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeSellingList : MonoBehaviour
{
    [SerializeField] Transform sellingListTransform;
    // 어차피 팔 데이터 List화 시켜서 담아놔야됌
    // 즉 각 데이터순으로 정렬되있는 아이템 리스트 가져와서 담아놓은 상태에서 각각의 확률에 맞춰서 뽑으면 된다는 거임.
    // 겜 시작하고 난뒤 데이터 가져와서 리스트에 담아놓은 뒤에 정렬한번 하는 식으로 데이터 가져와놓은 상태에서 상점판매 아이템 구현해주면 될거같음.
    [SerializeField] private SoulsInfo[] soulList;
    [SerializeField] private ReclicsInfo[] reclicsList;


    private ChangeShopListButtonAd resetShopListButtonAd;
    float soulListPosibility;
    float reclicsPosibillity;
    [SerializeField] SortSoul sortedSoul;
    [SerializeField] SortReclics sortedReclics;
    void Awake()
    {
        sortedSoul = GameObject.FindObjectOfType<SortSoul>();
        sortedReclics = GameObject.FindObjectOfType<SortReclics>();
    }
    private void Start()
    {

        resetShopListButtonAd = GameObject.FindObjectOfType<ChangeShopListButtonAd>();

        resetShopListButtonAd.rewardFunction += SettingShopList;
        Init();

        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() =>
        {
            if (!GameDataManger.Instance.GetGameData().settingShopList) SettingShopList();
            else SettingShopList(GameDataManger.Instance.GetGameData());
        }));

    }
    public void SettingShopList()
    {
        foreach (Transform child in sellingListTransform)
        {
            int rand = UnityEngine.Random.Range(0, 2);

            ISellingAble sellingAble = null;
            SoulsInfo soulsInfo = null;
            ReclicsInfo reclicsInfo = null;
            int index = GetSellingItem(rand);

            if (index == -1)
            {
                Debug.LogError(" Fail To Setting ShopItem List");
                return;
            }

            if (rand == 0)
            {
                soulsInfo = soulList[index];
                sellingAble = soulsInfo.GetComponent<ISellingAble>();
            }
            else if (rand == 1)
            {
                reclicsInfo = reclicsList[index];
                sellingAble = reclicsInfo.GetComponent<ISellingAble>();
            }

            int childSibling = child.GetSiblingIndex();
            float gem_or_soul = UnityEngine.Random.Range(0f, 1f);

            GameData gameData = GameDataManger.Instance.GetGameData();
            gameData.sellingItemListType[childSibling] = sellingAble.saveDataType;
            gameData.sellingItemListNum[childSibling] = sellingAble.saveDatanum;
            gameData.sellingGem[childSibling] = gem_or_soul >= 0.3 ? true : false;
            gameData.settingShopList = true;
            gameData.soldOutItem[childSibling] = false;

            child.GetComponent<ItemList>().Setting(sellingAble, gem_or_soul >= 0.3 ? true : false ,  soulsInfo : soulsInfo , reclicsInfo : reclicsInfo);

        }
        GameDataManger.Instance.SaveData();
    }
    public void SettingShopList(GameData gameData)
    {
        for (int i = 0; i < sellingListTransform.childCount; i++)
        {
            if (gameData.sellingItemListType[i] == "Soul")
            {
                sellingListTransform.GetChild(i).GetComponent<ItemList>()
                .Setting(SoulsManager.Instance.soulsInfos[gameData.sellingItemListNum[i]]
                        .GetComponent<ISellingAble>(), gameData.sellingGem[i], gameData.soldOutItem[i] , soulsInfo : SoulsManager.Instance.soulsInfos[gameData.sellingItemListNum[i]]);
            }
            else if (gameData.sellingItemListType[i] == "Reclics")
            {
                sellingListTransform.GetChild(i).GetComponent<ItemList>()
                .Setting(ReclicsManager.Instance.reclicsDatas[gameData.sellingItemListNum[i]]
                        .GetComponent<ISellingAble>(), gameData.sellingGem[i], gameData.soldOutItem[i] , reclicsInfo : ReclicsManager.Instance.reclicsDatas[gameData.sellingItemListNum[i]]);
            }
        }
    }
    private int GetSellingItem(int index)
    {
        float posibility = UnityEngine.Random.Range(0f, 1f);
        float probability = 0;
        Debug.Log($"뽑기 index : " + index);
        if (index == 0)
        {
            for (int i = 0; i < soulList.Length; i++)
            {
                probability += soulList[i].spawnProbabillity / soulListPosibility;
                if (probability >= posibility)
                {
                    Debug.Log(soulList[i].GetUnitData().name);
                    return i;
                    //soulList[i].GetComponent<ISellingAble>();
                }
            }
            return -1;
        }
        else if (index == 1)
        {
            for (int i = 0; i < reclicsList.Length; i++)
            {
                probability += reclicsList[i].spawnProbabillity / reclicsPosibillity;
                if (probability >= posibility)
                {
                    return i;
                    //reclicsList[i].GetComponent<ISellingAble>();
                }
            }
            return -1;
        }
        else
        {
            return -1;
        }
    }

    private void Init()
    {
        soulList = sortedSoul.souls;
        reclicsList = sortedReclics.reclicsInfo;

        for (int i = 0; i < soulList.Length; i++)
        {
            soulList[i].spawnProbabillity = (int)soulList[i].GetUnitData().type;
            soulListPosibility += soulList[i].spawnProbabillity;
        }
        for (int i = 0; i < reclicsList.Length; i++)
        {
            reclicsList[i].spawnProbabillity = (int)reclicsList[i].GetReclicsData().itemclass;
            reclicsPosibillity += reclicsList[i].spawnProbabillity;
        }
    }

}
