using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [SerializeField] Text statPointText;

    public static SkillManager Instance { get ; private set ;}
    public List<Unit> summons = new List<Unit>();
    public Dictionary<SkillData , int> skillDatas = new Dictionary<SkillData , int>();
    [Space(50)]
    [Header("SummonSkill")]
    public bool UpgradeSummonUnitSkill;    
    public bool UpgradeAutoRepair;
    public bool UpgradeResurrection; //MaxSpawnCount;
    public bool UpgradeSummonUnitSpeed;

    [Space(50)]
    [Header("AttackSkill")]
    public bool LightningAttack;
    public bool LightningAttackUpgrade;
    public bool batAttack;
    public bool batUpgradeAttackPercent;
    public bool batUpgradeCoolTime;

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
        if(GameDataManger.Instance.GetGameData().reclicsLevel[5] > 0 || GameDataManger.Instance.GetGameData().reclicsCount[5] > 0) {
            _statPoint += (int) (GameManager.Instance.reclicsDatas[5].inItPercent + (GameManager.Instance.reclicsDatas[5].levelUpPercent * GameDataManger.Instance.GetGameData().reclicsLevel[5]));
        }
        statPointText.text = "Point : " + statPoint;
    }

    public void UnLockSkill(SkillData data) {
        if(!skillDatas.ContainsKey(data)) {
            skillDatas.Add(data , 1);
        } 
        else skillDatas[data]++;

        switch (data.skillName) {
            case "소환수 강화" : 
                UpgradeSummonUnitSkill = true;
                break;
            case "자동 회복" :
                UpgradeAutoRepair = true;
                break;
            case "부활" :
                UpgradeResurrection = true;
                break;
            case "번개 공격" :
                LightningAttack = true;
                break;
            case "박쥐 소환" :
                batAttack = true;
                break;
            case "박쥐 공격확률 증가" :
                batUpgradeAttackPercent = true;
                break;
            case "박쥐 생성속도 증가" :
                batUpgradeCoolTime = true;
                break;
            case "번개 분할" : 
                LightningAttackUpgrade = true;
                break;
            case "소환수 이동속도 증가":
                UpgradeSummonUnitSpeed = true;
                break;
        }

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

    public SkillData GetSkillData(string skillName){
        foreach(SkillData data in skillDatas.Keys) {
            if(data.skillName == skillName) {
                return data;
            }
        }
        return null;
    }
}
