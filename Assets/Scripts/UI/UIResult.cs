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

    private void InitUi()
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
    
    private void SetClearTimeText(float time)
        => ClearTime.text = $"クリアタイム: {time}";
    private void SetKillCountText(int killCount)
        => KillCount.text = $"たおしたてき: {killCount}";
    private void SetMilkCountText(int milkCount)
        => MilkCount.text = $"ミルク：{milkCount}";
    private void SetSecretCountText(int secretCount)
        => SecretCount.text = $"シークレット：{secretCount}";
}