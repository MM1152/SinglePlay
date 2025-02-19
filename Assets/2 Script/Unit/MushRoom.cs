
public class MushRoom : ShortRangeScipt, ISummonUnit
{
    public Summoner summoner { get; set; }
    public int damageMeter { get ; set ; }
    private void OnEnable() {
        base.OnEnable();
        if(summoner == null) Spawn(GameManager.Instance.currentStage);
        else SummonerSpawn(summoner);
    }
}
