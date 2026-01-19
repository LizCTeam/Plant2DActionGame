using ResultScore;
using TMPro;
using UnityEngine;

public class GameManager : BasicBehaviour
{
    [SerializeField]
    public TextMeshProUGUI scoreText;
    [SerializeField]
    public TextMeshProUGUI timeText;

    protected override void OnAwake()
    {
        base.OnAwake();
        Application.targetFrameRate = 60;
    }

    protected override void OnStart()
    {
        base.OnStart();
        var manager = ScoreManager.Instance;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (ScoreManager.Instance != null)
        {
            timeText.text = "Time : " + ToMinuteSecond(ScoreManager.Instance.scoreData.ClearTime);
            scoreText.text = "Score : " + ScoreManager.Instance.GetTotalScore();
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    public static string ToMinuteSecond(float time)
    {
        if (time < 0f) time = 0f;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        return $"{minutes}:{seconds:00}";
    }
}