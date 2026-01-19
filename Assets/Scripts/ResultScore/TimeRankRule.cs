namespace ResultScore
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Score/TimeRankRule")]
    public class TimeRankRule : ScriptableObject
    {
        [System.Serializable]
        public struct RankLine
        {
            public TimeRank rank;
            public float clearTime; // 秒
        }

        public RankLine[] rankLines;

        public TimeRank Evaluate(float time)
        {
            TimeRank result = TimeRank.Bronze;

            foreach (var line in rankLines)
            {
                if (time <= line.clearTime)
                    result = line.rank;
            }

            return result;
        }
    }

}