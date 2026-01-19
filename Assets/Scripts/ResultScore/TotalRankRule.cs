using ResultScore;
using UnityEngine;

[CreateAssetMenu(menuName = "Score/Total Rank Rule")]
public class TotalRankRule : ScriptableObject
{
    [System.Serializable]
    public struct RankLine
    {
        public TotalRank rank;
        public int minScore;
    }

    public RankLine[] rankLines;

    public TotalRank Evaluate(int score)
    {
        TotalRank result = TotalRank.D;

        foreach (var line in rankLines)
        {
            if (score >= line.minScore)
                result = line.rank;
        }

        return result;
    }
}