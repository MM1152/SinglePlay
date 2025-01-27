using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulPickUp : MonoBehaviour
{
    [SerializeField] PickUpButton pick_One_BNT;
    [SerializeField] PickUpButton pick_Ten_BNT;
    [SerializeField] GameObject showItems;
    [SerializeField] GameObject showItemsTransform;
    [SerializeField] Animator pickUpAnimation;

    public List<float> spawnPosibillity = new List<float>();
    public float maxPosibillity = 0;
    private bool skipAnimation;
    private bool pickingItem;
    private SortSoul sortSoul;

    private void Awake() {
        sortSoul = GameObject.FindObjectOfType<SortSoul>();
        sortSoul.WaitSoulSort += Init;
    }

    private void Update() {
        if(pickingItem && Input.touchCount >= 1) skipAnimation = true; 
    }

    private void Init() {
        for(int i = 0 ; i < sortSoul.souls.Length; i++) {
            maxPosibillity += (int) sortSoul.souls[i].GetUnitData().type;
        }

        for(int i = 0 ; i < sortSoul.souls.Length; i++) {
            spawnPosibillity.Add((int) sortSoul.souls[i].GetUnitData().type / maxPosibillity);
        }
        pick_One_BNT.Init(this); 
        pick_Ten_BNT.Init(this);        
    }
    

    public IEnumerator ShowingReclics(int count){
        int[] pickUpList = new int[count];


        for(int i = 0 ; i < count; i++) {
            float posibillity = UnityEngine.Random.Range(0f , 1f);
            float sum = 0;
            for(int j = 0; j < spawnPosibillity.Count; j++) {
                sum += spawnPosibillity[j];
                
                // 미리 픽업될 리스트를 count 에 맞게 뽑아 놓은 뒤 , 순차적으로 보여줘야할듯
                if(posibillity <= sum) {
                    sortSoul.souls[j].GetSoul();
                    pickUpList[i] = j;
                    break;
                }
            }
        }

        pickUpAnimation.SetBool("PickUp" , true);
        yield return new WaitUntil(() => pickUpAnimation.GetCurrentAnimatorStateInfo(0).IsName("SoulPickUp") 
                                        && pickUpAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        pickUpAnimation.SetBool("PickUp" , false);
        
        pickingItem = true;
        skipAnimation = false;
        
        showItems.SetActive(true);

        foreach(int index in pickUpList) {
            SoulsInfo info = Instantiate(sortSoul.souls[index] , showItemsTransform.transform);
            //info.parentReclicsInfo = sortReclics.reclicsInfo[index];
            if(!skipAnimation) yield return new WaitForSeconds(0.2f);
        }

        pickingItem = false;
    }
}
