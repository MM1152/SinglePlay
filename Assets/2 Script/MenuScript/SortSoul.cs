using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortSoul : MonoBehaviour
{
    public SoulsInfo[] souls;

    private void Start() {
        souls = GameObject.FindObjectsOfType<SoulsInfo>();
        for(int i = 0; i < souls.Length; i++) {
            souls[i].spawnProbabillity = (int) souls[i].GetUnitData().type;
        }
        MergeSort<SoulsInfo> sort = new MergeSort<SoulsInfo>(souls);
        souls = sort.get();

        for(int i = 0; i < souls.Length; i++) {
            souls[i].transform.SetSiblingIndex(i);
        }
    }

}
