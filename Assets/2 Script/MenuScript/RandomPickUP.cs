using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class RandomPickUP : MonoBehaviour
{
    [SerializeField] Button pick_One_BNT;
    [SerializeField] Button pick_Ten_BNT;
    [SerializeField] GameObject showItems;
    [SerializeField] GameObject showItemsTransform;
    [SerializeField] GameObject reclicsPrefeb;
    [SerializeField] Animator pickUpAnimation;

    public List<float> spawnPosibillity = new List<float>();
    public float maxPosibillity = 0;
    private bool skipAnimation;
    GameData data;
    private void Awake() {
        pick_One_BNT.onClick.AddListener(() => PickUp(1));
        pick_Ten_BNT.onClick.AddListener(() => PickUp(10));   
    }

    private void Start() {
        for(int i = 0 ; i < ReclicsManager.Instance.reclicsDatas.Length; i++) {
            maxPosibillity += (int) ReclicsManager.Instance.reclicsDatas[i].GetReclicsData().itemclass;
        }

        for(int i = 0 ; i < ReclicsManager.Instance.reclicsDatas.Length; i++) {
            spawnPosibillity.Add((int) ReclicsManager.Instance.reclicsDatas[i].GetReclicsData().itemclass / maxPosibillity);
        }
    }

    void PickUp(int count) {
        //\\TODO 현재 재화에 맞춰 뽑기 실행하도록 해야함
        skipAnimation = false;
        StartCoroutine(ShowingReclics(count));
    }
    IEnumerator ShowingReclics(int count){
        pickUpAnimation.SetBool("PickUp" , true);
        yield return new WaitUntil(() => pickUpAnimation.GetCurrentAnimatorStateInfo(0).IsName("PickUpAnimation") && pickUpAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        pickUpAnimation.SetBool("PickUp" , false);

        showItems.SetActive(true);
        for(int i = 0 ; i < count; i++) {
            float posibillity = Random.Range(0f , 1f);
            float sum = 0;
            if(Input.touchCount >= 1) skipAnimation = true;
            for(int j = 0; j < ReclicsManager.Instance.reclicsDatas.Length; j++) {
                sum += (int) ReclicsManager.Instance.reclicsDatas[j].GetReclicsData().itemclass / maxPosibillity;
                
                if(posibillity <= sum) {
                    ReclicsManager.Instance.reclicsDatas[j].PickUp();
                    ReclicsInfo info = Instantiate(ReclicsManager.Instance.reclicsDatas[j] , showItemsTransform.transform);
                    info.parentReclicsInfo = ReclicsManager.Instance.reclicsDatas[j];
                    if(!skipAnimation) yield return new WaitForSeconds(0.2f);
                    break;
                }
            }
                    
        }
    }
}
