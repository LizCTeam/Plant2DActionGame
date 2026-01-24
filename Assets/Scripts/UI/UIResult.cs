using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : BasicBehaviour
{
    public TextMeshProUGUI ClearTime; 
    public TextMeshProUGUI KillCount;
    public TextMeshProUGUI MilkCount;
    public TextMeshProUGUI SecretCount;

    protected override void OnStart()
    {
        base.OnStart();
        InitUi();
    }

    public void InitUi()
    {
        if (GameResultSingleton.Instance)
        {
            var result = GameResultSingleton.Instance;
            SetClearTimeText(result.ResultTime);
            SetKillCountText(result.KillCount);
            SetMilkCountText(result.MilkCount);
            SetSecretCountText(result.SecretCount);
        }
        else
        {
            SetClearTimeText(0);
            SetKillCountText(0);
            SetMilkCountText(0);
            SetSecretCountText(0);   
        }
    }
    
    public static string FormatTime(float timeSeconds)
    {
        if (timeSeconds < 0f) timeSeconds = 0f;

        int minutes = (int)(timeSeconds / 60f);
        float seconds = timeSeconds % 60f;

        return $"{minutes}:{seconds:00.00}";
    }
    
    private void SetClearTimeText(float time)
        => ClearTime.text = $"タイム: \n{FormatTime(time)}";
    private void SetKillCountText(int killCount)
        => KillCount.text = $"たおしたてき: \n{killCount}たい";
    private void SetMilkCountText(int milkCount)
        => MilkCount.text = $"ぬすんだミルク: \n{milkCount}こ";
    private void SetSecretCountText(int secretCount)
        => SecretCount.text = $"シークレット: \n{secretCount}こ";
}