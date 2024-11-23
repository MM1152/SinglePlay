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

    public List<float> spawnPosibillity = new List<float>();
    public float maxPosibillity = 0;
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
        // 아이템 확률로 뽑아오는 코드
        showItems.SetActive(true);
        for(int i = 0 ; i < count; i++) {
            float posibillity = Random.Range(0f , 1f);
            float sum = 0;

            for(int j = 0; j < ReclicsManager.Instance.reclicsDatas.Length; j++) {
                sum += (int) ReclicsManager.Instance.reclicsDatas[j].GetReclicsData().itemclass / maxPosibillity;

                if(posibillity <= sum) {
                    ReclicsManager.Instance.reclicsDatas[j].PickUp();
                    ReclicsInfo info = Instantiate(ReclicsManager.Instance.reclicsDatas[j] , showItemsTransform.transform);
                    info.parentReclicsInfo = ReclicsManager.Instance.reclicsDatas[j];
                    break;
                }
            }
                    
        }
        /*if(data.soul >= count * 100) {
            
        }*/
    }
}
