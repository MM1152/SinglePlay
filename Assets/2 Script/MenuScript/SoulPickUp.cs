using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulPickUp : MonoBehaviour
{
    [SerializeField] PickUpButton pick_One_BNT;
    [SerializeField] PickUpButton pick_Ten_BNT;
    [SerializeField] GameObject showItems;
    [SerializeField] GameObject showItemsTransform;
    [SerializeField] Animator pickUpAnimation;
    [SerializeField] Slider decideLegendarySlider;
    [SerializeField] Text decideCount;

    public List<float> spawnPosibillity = new List<float>();
    public Dictionary<ItemClass , float> showPosibilityData = new Dictionary<ItemClass, float>();
    public float maxPosibillity = 0;
    private bool skipAnimation;
    private bool pickingItem;
    private SortSoul sortSoul;
    private GameObject blockTouch;
    private int openCount;
    private void Awake() {
        sortSoul = GameObject.FindObjectOfType<SortSoul>();
        blockTouch = transform.Find("Block Touch").gameObject;
        sortSoul.WaitSoulSort += Init;
    }
    private void Start() {
        GameDataManger.Instance.StartCoroutine(GameDataManger.WaitForDownLoadData(() => SettingOpenCount()));
    }
    private void Update() {
        if(pickingItem && Input.touchCount >= 1) skipAnimation = true; 
    }
    private void SettingOpenCount() {
        openCount = GameDataManger.Instance.GetGameData().openCount[1];
        decideLegendarySlider.value = openCount / 30f;
        decideCount.text = "전설확정까지 <color=green>" + (30 - openCount) + "</color>회 남았습니다.";
    }
    private void Init() {
        for(int i = 0 ; i < sortSoul.souls.Length; i++) {
            maxPosibillity += (int) sortSoul.souls[i].GetUnitData().type;
        }

        for(int i = 0 ; i < sortSoul.souls.Length; i++) {
            spawnPosibillity.Add((int) sortSoul.souls[i].GetUnitData().type / maxPosibillity);
            if(showPosibilityData.ContainsKey(sortSoul.souls[i].GetUnitData().type)) showPosibilityData[sortSoul.souls[i].GetUnitData().type] += spawnPosibillity[i];
            else showPosibilityData.Add(sortSoul.souls[i].GetUnitData().type , spawnPosibillity[i]);
        }
        pick_One_BNT.Init(this); 
        pick_Ten_BNT.Init(this);        
    }
    

    public IEnumerator ShowingSoul(int count){
        GameDataManger.Instance.GetGameData().openCount[1] += count;
        bool decideLagendary = GameDataManger.Instance.GetGameData().openCount[1] >= 30;

        int[] pickUpList = new int[count];
        blockTouch.SetActive(true);
        
        for(int i = 0 ; i < count; i++) {
            float posibillity = 0f;

            if(decideLagendary) {
                posibillity = UnityEngine.Random.Range(1f - showPosibilityData[ItemClass.LEGENDARY] , 1f);
                Debug.Log(posibillity);
                GameDataManger.Instance.GetGameData().openCount[1] = 0;
            }
            else posibillity = UnityEngine.Random.Range(0f , 1f);

            float sum = 0;
            for(int j = 0; j < spawnPosibillity.Count; j++) {
                sum += spawnPosibillity[j];
                
                // 미리 픽업될 리스트를 count 에 맞게 뽑아 놓은 뒤 , 순차적으로 보여줘야할듯
                if(posibillity <= sum) {
                    sortSoul.souls[j].GetSoul();
                    pickUpList[i] = j;
                    if(decideLagendary) decideLagendary = false;
                    break;
                }
            }
        }

        pickUpAnimation.SetBool("PickUp" , true);
        yield return new WaitUntil(() => pickUpAnimation.GetCurrentAnimatorStateInfo(0).IsName("SoulPickUp") 
                                        && pickUpAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        pickUpAnimation.SetBool("PickUp" , false);
        blockTouch.SetActive(false);
        pickingItem = true;
        skipAnimation = false;
        
        showItems.SetActive(true);

        foreach(int index in pickUpList) {
            SoulsInfo info = Instantiate(sortSoul.souls[index] , showItemsTransform.transform);
            info.parentSoulsInfo = sortSoul.souls[index];
            if(!skipAnimation) yield return new WaitForSeconds(0.2f);
        }

        pickingItem = false;
        SettingOpenCount();
        GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
        openCount = GameDataManger.Instance.GetGameData().openCount[1];
    }
}
