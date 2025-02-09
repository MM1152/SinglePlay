using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void DropSoul(UnitData unitData);

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] GameObject curtain;
    [SerializeField] GameObject rewardViewer;
    public SettingTab setting;
    /// <summary>
    /// 1: Attack , 2: Hp , 3: Clitical , 4: AttackSpeed  , 5: MoveSpeed , 6: BonusTalent , 7: BonusGoods , 8: IncreaesDamage ,
    /// 9: IncreaesHp, 10: CoolTime, 11: SkillDamage,  12 IncreasedExp, 13 Dodge, 구현필요 [ 14 DrainLife ] 구현필요
    /// </summary>
    public List<ReclicsData> reclicsDatas;
    public bool reclisFin;
    public Dictionary<string , UnitData> soulsInfo = new Dictionary<string, UnitData>();
    public bool soulsFin;
    
    public int currentStage = 10;
    public int clearMonseter;
    public bool gameClear;
    public bool playingAnimation;
    public bool playingShader;

    public DropSoul dropSoul;
    public GameObject nextStage;

    public int mapindex;
    public string mapName;
    public int maxStage;
    public float obtainablegoods;
    /// <summary>
    /// 현재 맵에서 드랍된 유닛 소울
    /// </summary>
    public Dictionary<UnitData , int> dropSoulList = new Dictionary<UnitData, int>();

    private ShowingMenuTools showingMenuTools;
    //\\TODO maxStaget 클리어시 GameManager에서 게임 클리어및 종료 신호 보내줘야함;
    // 획득 재화량은 currentStage / maxStaget
    private void Awake() {
        if(Instance == null) {
            Instance = this;    
            showingMenuTools = GetComponent<ShowingMenuTools>();
            DontDestroyOnLoad(this);
        }
        else  {
            Destroy(this);
        }
        
    }
    private void Update() {
        if(clearMonseter <= 0) ClearLevel();
    }
    public void ClearLevel(){
        if(currentStage >= maxStage) {
            if(mapindex + 1 > GameDataManger.Instance.GetGameData().unLockMap.Count) {
                GameDataManger.Instance.GetGameData().unLockMap.Add(true);
                GameDataManger.Instance.SaveData();
            }
            
            ReturnToMenu();
            return;
        }
        gameClear = true;
        nextStage.SetActive(true);
    }
    public void StopGame(){
        Time.timeScale = 0;
    }

    public void ResumeGame(){
        Time.timeScale = 1;
    }
    public void SlowGame(float size){
        Time.timeScale = size;
    }
    public void ReturnToMenu() {

        Delegate[] dele = dropSoul.GetInvocationList();

        // DropSoul에 참조된 모든 함수 제거
        foreach(DropSoul function in dele) {
            dropSoul -= function;
        }  
    
        if(currentStage == 1 && dropSoulList.Count == 0) {
            LoadingScene.LoadScene("MenuScene");
            ResumeGame();
            return;
        }
        
        //\\TODO 여기다가 결과창 보여주면 될거같은데.
        // 우짜지 ㅅㅂ..
        //\\TODO 업적시스템 추가
        //\\플레이어 자체 레벨 시스템 구현
        //\\레벨당 보상 구현 ㄱ 

        rewardViewer.SetActive(true);
        showingMenuTools.HideOption(false);
        dropSoulList.Clear();
    }
    public void ReturnToMain(string SceneName = "MainScene"){
        LoadingScene.LoadScene(SceneName);
        clearMonseter = 50;
        dropSoul += delegate(UnitData unitData) {
            GameData gameData = GameDataManger.Instance.GetGameData();
            if(dropSoulList.ContainsKey(unitData)) dropSoulList[unitData]++;
            else dropSoulList.Add(unitData , 1);
            gameData.soulsCount[unitData.typenumber - 1]++;
            GameDataManger.Instance.SaveData();
        };
        showingMenuTools.HideOption(true);
        ResumeGame();
    }
    public IEnumerator WaitForNextMap(Action action) {

        yield return new WaitForSeconds(0.3f);
        
        clearMonseter = 50;
        curtain.SetActive(true);
        nextStage.SetActive(false);

        Image image = curtain.GetComponent<Image>();
        Vector4 color = new Vector4(0 , 0 , 0 , 1);

        yield return new WaitForSeconds(1f);

        for(float i = 1f; i >= 0f; i -= 0.2f){
            image.color = new Color(0 , 0 , 0 , i);
            if(i == 1f) action.Invoke();
            
            yield return new WaitForSeconds(0.2f);
        }
        
        curtain.SetActive(false);
        currentStage++;
        
        image.color = color;
        gameClear = false;
        EnemySpawner.Instance.isBossSpawn = false;
    }
}
