using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortReclics : MonoBehaviour
{
    public ReclicsInfo[] reclicsInfo;
    public Action WaitReclicsSort;
    public void Start()
    {
        reclicsInfo = GameObject.FindObjectsOfType<ReclicsInfo>();
        
        for(int i = 0; i < reclicsInfo.Length; i++) {
            reclicsInfo[i].spawnProbabillity = (int) reclicsInfo[i].GetReclicsData().itemclass;
        }
        
        MergeSort<ReclicsInfo> sort = new MergeSort<ReclicsInfo>(reclicsInfo);
        reclicsInfo = sort.get();

        for(int i = 0; i < reclicsInfo.Length; i++) {
            reclicsInfo[i].transform.SetSiblingIndex(i);
        }
        WaitReclicsSort?.Invoke();
    }
    
}
