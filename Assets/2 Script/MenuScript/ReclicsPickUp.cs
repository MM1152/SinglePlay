using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReclicsPickUp : MonoBehaviour
{
    [SerializeField] PickUpButton pick_One_BNT;
    [SerializeField] PickUpButton pick_Ten_BNT;
    [SerializeField] GameObject showItems;
    [SerializeField] GameObject showItemsTransform;
    [SerializeField] Animator pickUpAnimation;
    [SerializeField] Slider decideLegendarySlider;
    [SerializeField] Text decideCount;
    
    public Dictionary<ItemClass , float> showPosibilityData = new Dictionary<ItemClass, float>();
    public List<float> spawnPosibillity = new List<float>();
    public float maxPosibillity = 0;
    private bool skipAnimation;
    private bool pickingItem;
    private SortReclics sortReclics;
    private GameObject blockTouch;
    private int openCount;
    private void Awake() {
        sortReclics = GameObject.FindObjectOfType<SortReclics>();
        blockTouch = transform.Find("Block Touch").gameObject;
        sortReclics.WaitReclicsSort += Init;
    }
    private void Start() {
        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() => SettingOpenCount()));
    }
    private void SettingOpenCount() {
        openCount = GameDataManger.Instance.GetGameData().openCount[0];
        decideLegendarySlider.value = openCount / 30f;
        decideCount.text = "전설확정까지 <color=green>" + (30 - openCount) + "</color>회 남았습니다.";
    }
    private void Update() {
        if(pickingItem && Input.touchCount >= 1) skipAnimation = true; 
    }

    private void Init() {
        for(int i = 0 ; i < sortReclics.reclicsInfo.Length; i++) {
            maxPosibillity += (int) sortReclics.reclicsInfo[i].GetReclicsData().itemclass;

        }

        for(int i = 0 ; i < sortReclics.reclicsInfo.Length; i++) {
            spawnPosibillity.Add((int) sortReclics.reclicsInfo[i].GetReclicsData().itemclass / maxPosibillity);

            if(showPosibilityData.ContainsKey(sortReclics.reclicsInfo[i].GetReclicsData().itemclass)) showPosibilityData[sortReclics.reclicsInfo[i].GetReclicsData().itemclass] += spawnPosibillity[i];
            else showPosibilityData.Add(sortReclics.reclicsInfo[i].GetReclicsData().itemclass , spawnPosibillity[i]);
        }
        pick_One_BNT.Init(this); 
        pick_Ten_BNT.Init(this);        
    }
    

    public IEnumerator ShowingReclics(int count){
        blockTouch.SetActive(true);
        int[] pickUpList = new int[count];

        GameDataManger.Instance.GetGameData().openCount[0] += count;
        bool decideLagendary = GameDataManger.Instance.GetGameData().openCount[0] >= 30;

        for(int i = 0 ; i < count; i++) {
            float posibillity = 0f;
            
            if(decideLagendary) {
                posibillity = UnityEngine.Random.Range(1f - showPosibilityData[ItemClass.LEGENDARY] , 1f);
                Debug.Log("SpawnPosibility : " + posibillity);
                GameDataManger.Instance.GetGameData().openCount[0] = 0;
            }
            else posibillity = UnityEngine.Random.Range(0f , 1f);

            float sum = 0;
            for(int j = 0; j < spawnPosibillity.Count; j++) {
                sum += spawnPosibillity[j];
                
                // 미리 픽업될 리스트를 count 에 맞게 뽑아 놓은 뒤 , 순차적으로 보여줘야할듯
                
                if(posibillity <= sum) {
                    sortReclics.reclicsInfo[j].PickUp();
                    pickUpList[i] = j;
                    Debug.Log("Pick reclicsinfo index " + j);
                    if(decideLagendary) decideLagendary = false;
                    break;
                }
            }
        }

        pickUpAnimation.SetBool("PickUp" , true);
        yield return new WaitUntil(() => pickUpAnimation.GetCurrentAnimatorStateInfo(0).IsName("ReclicsPickUp") 
                                        && pickUpAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        pickUpAnimation.SetBool("PickUp" , false);
        blockTouch.SetActive(false);
        pickingItem = true;
        skipAnimation = false;

        showItems.SetActive(true);

        foreach(int index in pickUpList) {
            ReclicsInfo info = Instantiate(sortReclics.reclicsInfo[index] , showItemsTransform.transform);
            info.parentReclicsInfo = sortReclics.reclicsInfo[index];
            if(!skipAnimation) yield return new WaitForSeconds(0.2f);
        }

        pickingItem = false;
        SettingOpenCount();
        GameDataManger.Instance.SaveData();
        openCount = GameDataManger.Instance.GetGameData().openCount[1];
    }
}
