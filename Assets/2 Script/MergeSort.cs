
using System.Collections.Generic;
using Unity.VisualScripting;

public class MergeSort{
    float[] sortedList;
    Unit[] sortedUnitList;
    Unit[] units;
    List<float> unitSpawnprobabillity = new List<float>();
    public MergeSort(Unit[] units) {
        this.units = units;
        
        sortedList = new float[units.Length];
        sortedUnitList = new Unit[units.Length];

        foreach(Unit unit in this.units) {
            unitSpawnprobabillity.Add(unit.unit.spawnProbabillity);
        }   

        Merge(unitSpawnprobabillity , 0 , unitSpawnprobabillity.Count - 1);
    }
    public Unit[] get(){
        return units;
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
                sortedUnitList[k] = units[j];
                sortedList[k++] = unitSpawnProbabillity[j++];
            }
            else {
                sortedUnitList[k] = units[i];
                sortedList[k++] = unitSpawnProbabillity[i++];
            }
        }

        if(i <= mid) {
            for(int l = i; i <= mid; i++){
                sortedUnitList[k] = units[l];
                sortedList[k++] = unitSpawnProbabillity[l];
            }
        }
        else {
            for(int l = j; j <= right; j++) {
                sortedUnitList[k] = units[l];
                sortedList[k++] = unitSpawnProbabillity[l];
            }
        }

        for(int l = left; l <= right; l++) {
            units[l] = sortedUnitList[l];
            unitSpawnProbabillity[l] = sortedList[l];
        }
    }
}