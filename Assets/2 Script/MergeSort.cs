
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 게임오브젝트를 유닛의 생성확률에 맞춰 정렬해준다.
/// </summary>
public class MergeSort<T> where T : ISpawnPosibillity{
    float[] sortedList;
    T[] sortedDataList;
    T[] datas;
    List<float> unitSpawnprobabillity = new List<float>();

    /// <summary>
    /// 정렬할 데이터들 배열로 넣어주기
    /// </summary>
    /// <param name="units"></param>
    public MergeSort(T[] units) { 
        this.datas = units;
        
        sortedList = new float[units.Length];
        sortedDataList = new T[units.Length];

        foreach(T unit in this.datas) {
            unitSpawnprobabillity.Add(unit.spawnProbabillity);
        }   

        Merge(unitSpawnprobabillity , 0 , unitSpawnprobabillity.Count - 1);
    }
    /// <summary>
    /// 정렬된 배열 받기
    /// </summary>
    /// <returns></returns>
    public T[] get(){ 
        return datas;
    }
    public void Merge(List<float> unitSpawnProbabillity , int left , int right){
        
        if(left < right) {
            int mid = (left + right) / 2;

            Merge(unitSpawnProbabillity , left ,  mid);
            Merge(unitSpawnProbabillity , mid + 1 , right);
            Sorted(unitSpawnProbabillity , left , right );
        }
    }

    private void Sorted(List<float> unitSpawnProbabillity , int left , int right){
        int i = left;
        int mid = (left + right) / 2;
        int j = mid + 1;
        int k = left;

        while(i <= mid && j <= right){ 
            if(unitSpawnProbabillity[i] <= unitSpawnProbabillity[j]) {
                sortedDataList[k] = datas[j];
                sortedList[k++] = unitSpawnProbabillity[j++];
            }
            else {
                sortedDataList[k] = datas[i];
                sortedList[k++] = unitSpawnProbabillity[i++];
            }
        }

         if(i <= mid) {
            for(int l = i; i <= mid; i++){
                sortedDataList[k] = datas[l];
                sortedList[k++] = unitSpawnProbabillity[l];
            }
        }
        else {
            for(int l = j; j <= right; j++) {
                sortedDataList[k] = datas[l];
                sortedList[k++] = unitSpawnProbabillity[l];
            }
        }

        for(int l = left; l <= right; l++) {
            datas[l] = sortedDataList[l];
            unitSpawnProbabillity[l] = sortedList[l];
        }
    }
}