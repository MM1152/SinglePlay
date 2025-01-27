
public class MushRoom : ShortRangeScipt, ISummonUnit
{
    public Summoner summoner { get; set; }

    private void OnEnable() {
        base.OnEnable();
        if(summoner == null) Spawn(GameManager.Instance.currentStage);
        else SummonerSpawn(summoner);
    }
}
