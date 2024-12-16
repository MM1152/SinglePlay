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
    public bool UpgradeSummonUnitSkill;    
    public bool UpgradeAutoRepair;
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
    MergeSort<Unit> merge;

    private void Awake() {
        Instance = this;
        _statPoint += (int) (GameManager.Instance.reclicsDatas[5].inItPercent + (GameManager.Instance.reclicsDatas[5].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[5]));
        statPointText.text = "Point : " + statPoint;
    }

    public void UnLockSkill(string data) {
        
        switch (data) {
            case "소환수 강화" : 
                UpgradeSummonUnitSkill = true;
                break;
            case "자동 회복" :
                UpgradeAutoRepair = true;
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
        merge = new MergeSort<Unit>(summons.ToArray());
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

    private void SettingUnitProbabillity(){
        for(int i = 0 ; i < summons.Count; i++) {
            if(summons[i].spawnProbabillity == 0) {
                summons[i].spawnProbabillity = summons[i].unit.spawnProbabillity;
            }
        }
    }
}
