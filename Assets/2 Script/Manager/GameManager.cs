using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    #region Inspector
    [SerializeField] GameObject curtain;
    [SerializeField] GameObject rewardViewer;
    [SerializeField] Tutorial tutorial;
    public CheckVesion checkVesion;

    public ConnectDB connectDB;
    public SettingTab setting;
    public GetSoulAnimation getSoulAnimation;
    /// <summary>
    /// 1: Attack , 2: Hp , 3: Clitical , 4: AttackSpeed  , 5: MoveSpeed , 6: BonusTalent , 7: BonusGoods , 8: IncreaesDamage ,
    /// 9: IncreaesHp, 10: CoolTime, 11: SkillDamage,  12 IncreasedExp, 13 Dodge, [ 14 DrainLife ] 구현필요
    /// </summary>
    public List<ReclicsData> reclicsDatas;
    public bool reclisFin;
    public Dictionary<string , UnitData> allSoulInfo = new Dictionary<string , UnitData>();
    public List<string> soulsInfo = new List<string>();
    public List<string> battlesInfo = new List<string>();
    public BattleUserData otherBattleUserData;
    
    public bool soulsFin;
    
    public int currentStage = 10;
    public int clearMonseter;
    public bool gameClear;
    public bool playingAnimation;
    public bool playingShader;

    public delegate void DropSoul(UnitData unitData);
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

    public ShowingMenuTools showingMenuTools;
    public bool sortSoul;
    public bool sortReclis;

    //DailyQuest용 몬스터 처치 횟수
    public int clearMosetCount;
    public bool isPlayingTutorial;
    public string userName;
    #endregion
    private void Awake() {
        if(Instance == null) {
            Instance = this;    
            showingMenuTools = GetComponent<ShowingMenuTools>();
            getSoulAnimation = GetComponent<GetSoulAnimation>();
            connectDB = GetComponent<ConnectDB>();
            connectDB.Init();
            connectDB.CheckVersion((value) => checkVesion.DifferentVersion(value));
            

            //connectDB.CheckUserName((value) => checkVesion.DifferentVersion(value));
            Application.targetFrameRate = 60;
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
                GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
            }
            clearMonseter = 50;    
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
    public void GoBossMapTutorial(){
        currentStage = 9;
        clearMonseter = 0;
    }
    public void ReturnToMenu() {

        DailyQuestTab.ClearDailyQuest(QuestType.ClearMonster , clearMosetCount);
        clearMosetCount = 0;

        if(isPlayingTutorial) {
            dropSoul.Invoke(Resources.Load<GameObject>("DungeonEnemy/BasicRat").GetComponent<Unit>().unit);
        }
        

        if(dropSoul != null) {
            Delegate[] dele = dropSoul.GetInvocationList();

            // DropSoul에 참조된 모든 함수 제거
            foreach(DropSoul function in dele) {
                dropSoul -= function;
            }  
        }

        if(currentStage == 1 && dropSoulList.Count == 0) {
            LoadingScene.LoadScene("MenuScene");

            ResumeGame();
            return;
        }
        
        rewardViewer.SetActive(true);
        
        dropSoulList.Clear();
        DailyQuestTab.ClearDailyQuest(QuestType.PlayGame , 1);
        nextStage.SetActive(false);
    }
    public void ReturnToMain(string SceneName = "MainScene"){
        LoadingScene.LoadScene(SceneName);
        currentStage = 1;
        clearMonseter = 50;
        dropSoul += delegate(UnitData unitData) {
            GameData gameData = GameDataManger.Instance.GetGameData();
            if(dropSoulList.ContainsKey(unitData)) dropSoulList[unitData]++;
            else dropSoulList.Add(unitData , 1);
            gameData.soulsCount[unitData.typenumber - 1]++;
            GameDataManger.Instance.SaveData(GameDataManger.SaveType.GameData);
        };
        showingMenuTools.HideOption(true);
        ResumeGame();

    }
    public void StartTutorial(int index){
        tutorial.StartTutorial(index);
    }
    public int GetTutorial(){
        return tutorial.pastTutorialIndex;
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
