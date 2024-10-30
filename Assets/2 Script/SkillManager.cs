using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] Text statPointText;

    public static SkillManager Instance { get ; private set ;}
    public List<Unit> summons = new List<Unit>();
    public Dictionary<string , int> skillDatas = new Dictionary<string , int>();
    [Space(50)]
    [Header("SummonSkill")]
    public bool SummonSkill;    
    public bool UpgradeSummonSkill;
    public int UpgradeMaxSummonCount = 2; //MaxSpawnCount;

    [Space(50)]
    [Header("AttackSkill")]
    public bool LightningAttack;

    private int _statPoint = 1;
    public int statPoint {
        get {
            return _statPoint;
        }
        set {
            _statPoint = value;
            statPointText.text = "Point : " + _statPoint;
        }
    }
    private float sumPosiboillity = 0;
    MergeSort merge;

    private void Awake() {
        Instance = this;
        statPointText.text = "Point : " + statPoint;
    }

    public void UnLockSkill(string data) {
        
        switch (data) {
            case "해골 소환" : 
                SummonSkill = true;
                break;

            case "해골 마법사 소환" :
                UpgradeSummonSkill = true;
                break;

            case "최대 소환횟수 증가" :
                UpgradeMaxSummonCount++;
                break;
            case "번개 공격" :
                LightningAttack = true;
                break;
        }
        if(!skillDatas.ContainsKey(data)) {
            skillDatas.Add(data , 1);
        } 
        else skillDatas[data]++;

        statPoint--;
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
