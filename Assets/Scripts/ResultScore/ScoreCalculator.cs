using UnityEngine;

namespace ResultScore
{
    public class ScoreCalculator
    {
        public static int CalculateScore(ScoreData data, TimeRankRule rule)
        {
            var timeRank = rule.Evaluate(data.ClearTime);
            int timeRankValue = (int)timeRank;

            float enemyRate = data.MaxEnemyCount > 0
                ? (float)data.EnemyKillCount / data.MaxEnemyCount
                : 0f;

            float score =
                (timeRankValue + data.HiddenItemCount)
                + (data.MilkCount * 100)
                * enemyRate
                * 100f;

            return Mathf.FloorToInt(score);
        }
    }
}