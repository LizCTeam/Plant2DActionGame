using ResultScore;
using UnityEngine;

public class ScoreManager : BasicBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public ScoreData scoreData;

    [Header("Rules")]
    public TimeRankRule timeRankRule;
    public TotalRankRule totalRankRule;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        scoreData = new ScoreData();
    }

    protected override void OnStart()
    {
        base.OnStart();
        print(scoreData.ClearTime);
        print(scoreData.MaxEnemyCount);
        print(scoreData.EnemyKillCount);
        print(scoreData.HiddenItemCount);
        print(scoreData.MilkCount);
        print(scoreData.MaxHiddenItemCount);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        scoreData.ClearTime += Time.deltaTime;
    }

    public void ResetScore()
    {
        scoreData = new ScoreData();
    }
    
    public TimeRank GetTimeRank()
    {
        return timeRankRule.Evaluate(scoreData.ClearTime);
    }

    public int GetTotalScore()
    {
        return ScoreCalculator.CalculateScore(
            scoreData,
            timeRankRule
        );
    }

    public TotalRank GetTotalRank()
    {
        int score = GetTotalScore();
        return totalRankRule.Evaluate(score);
    }
}