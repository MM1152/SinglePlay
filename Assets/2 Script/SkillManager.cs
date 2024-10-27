using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get ; private set ;}
    public List<Unit> summons = new List<Unit>();
    private Dictionary<string , int> skillDatas = new Dictionary<string , int>();

    public bool SummonSkill;
    public bool UpgradeSummonSkill;

    private float sumPosiboillity = 0;
    MergeSort merge;
    private void Awake() {
        Instance = this;
    }

    public void UnLockSkill(string data) {
        
        switch (data) {
            case "SummonSkill" : 
                SummonSkill = true;
                break;

            case "UpgradeSummonSkill" :
                UpgradeSummonSkill = true;
                break;
        }
        if(!skillDatas.ContainsKey(data)) {
            skillDatas.Add(data , 1);
        } 
        else skillDatas[data]++;
    }

    public void AddSummonsUnit(Unit summonUnit){
        summons.Add(summonUnit);
        merge = new MergeSort(summons.ToArray());
        sumPosiboillity = 0;

        for(int i = 0; i < summons.Count; i++) {
            sumPosiboillity += summons[i].unit.spawnProbabillity;
        }
    }

    /// <returns>소환될 유닛</returns>
    public GameObject GetSummonGameObjet(){
        GameObject unit = null;

        float spawnPosibillity = Random.Range(0f , 1f);
        float spawn = 0;
        for(int i = 0; i < summons.Count; i++) {
            spawn += summons[i].unit.spawnProbabillity / sumPosiboillity;
            if(spawn >= spawnPosibillity) {
                unit = summons[i].gameObject;
                return unit;
            }
        }
        
        return unit;
    }
}
