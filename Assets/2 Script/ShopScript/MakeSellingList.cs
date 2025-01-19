using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSellingList : MonoBehaviour
{
    [SerializeField] Transform sellingListTransform;
    // 어차피 팔 데이터 List화 시켜서 담아놔야됌
    // 즉 각 데이터순으로 정렬되있는 아이템 리스트 가져와서 담아놓은 상태에서 각각의 확률에 맞춰서 뽑으면 된다는 거임.
    // 겜 시작하고 난뒤 데이터 가져와서 리스트에 담아놓은 뒤에 정렬한번 하는 식으로 데이터 가져와놓은 상태에서 상점판매 아이템 구현해주면 될거같음.
    [SerializeField] private SoulsInfo[] soulList;
    [SerializeField] private ReclicsInfo[] reclicsList;
    float soulListPosibility;
    float reclicsPosibillity;
    SortSoul sortedSoul;
    SortReclics sortedReclics;
    private void Start() {
        sortedSoul = GameObject.FindObjectOfType<SortSoul>();
        sortedReclics = GameObject.FindObjectOfType<SortReclics>();

        Init();
        SettingShopList();
    }
    public void SettingShopList(){     
        foreach(Transform child in sellingListTransform) {
            int rand = UnityEngine.Random.Range(0 , 2);
            ISellingAble index = GetSellingItem(rand);
            
            if(index == null) Debug.LogError(" Fail To Setting ShopItem List" );
            else {
                float gem_or_soul = UnityEngine.Random.Range(0f , 1f);
                child.GetComponent<ItemList>().Setting(index , gem_or_soul >= 0.3 ? true : false );
            }
        }
    }
    private ISellingAble GetSellingItem(int index){
        float posibility = UnityEngine.Random.Range(0f , 1f);
        float probability = 0;
        if(index == 0) {
            for(int i = 0 ; i < soulList.Length; i++) {
                probability += soulList[i].spawnProbabillity / soulListPosibility;
                if(probability >= posibility) {
                    return soulList[i].GetComponent<ISellingAble>();
                }
            }
            return null;
        }     
        else if(index == 1) {
            for(int i = 0 ; i < reclicsList.Length; i++) {
                probability += reclicsList[i].spawnProbabillity / reclicsPosibillity;
                if(probability >= posibility) {
                    return reclicsList[i].GetComponent<ISellingAble>();
                }
            }
            return null;
        }
        else {
            return null;
        }
    }

    private void Init(){
        soulList = sortedSoul.souls;
        reclicsList = sortedReclics.reclicsInfo;

        for(int i = 0 ; i < soulList.Length; i++) {
            soulList[i].spawnProbabillity = (int) soulList[i].GetUnitData().type;
            soulListPosibility += soulList[i].spawnProbabillity;
        }
        for(int i = 0; i < reclicsList.Length; i++) {
            reclicsList[i].spawnProbabillity = (int) reclicsList[i].GetReclicsData().itemclass;
            reclicsPosibillity += reclicsList[i].spawnProbabillity;
        }
    }
}
