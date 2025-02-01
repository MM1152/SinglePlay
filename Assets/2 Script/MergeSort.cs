
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Heap {
    public List<float> heap;
    public List<Transform> target;
    public Heap() {
        heap = new List<float>();
        target = new List<Transform>();
        
        heap.Add(default);
        target.Add(default);
    }
    /// <param name="value">Distance To Taget</param>
    /// <param name="info">Target Transform</param>
    public void Add(float value , Transform info) {
        heap.Add(value);
        target.Add(info);

        int index = heap.Count - 1;
        while(index > 1) {
            int parent = (index - 1) / 2;
            
            if(heap[parent] > heap[index]) {
                float temp = heap[parent];
                heap[parent] = heap[index];
                heap[index] = temp;

                Transform target = this.target[parent];
                this.target[parent] = this.target[index];
                this.target[index] = target;
            }
            else {
                break;
            }

            index = parent;
        }
    }
    public Transform Pop(){
        if(heap.Count > 1) {
            Transform returnValue = target[1];

            heap[1] = heap[heap.Count - 1];
            target[1] = target[target.Count - 1];

            heap.RemoveAt(heap.Count - 1);
            target.RemoveAt(target.Count - 1);
            
            int index = 1;

            while(index < heap.Count) {
                int left = index * 2;
                int right = index * 2 + 1;
                int center = index;

                if(left < heap.Count && heap[left] < heap[index]) index = left;
                if(right < heap.Count && heap[right] < heap[index]) index = right;

                if(center == index) break;

                float temp = heap[index];
                heap[index] = heap[center];
                heap[center] = temp;

                Transform target = this.target[index];
                this.target[index] = this.target[center];
                this.target[center] = target;
            }

            return returnValue;
        } else {
            return null;
        }
    }
}

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
            for(int l = i; l <= mid; l++){
                sortedDataList[k] = datas[l];
                sortedList[k++] = unitSpawnProbabillity[l];
            }
        }
        else {
            for(int l = j; l <= right; l++) {
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