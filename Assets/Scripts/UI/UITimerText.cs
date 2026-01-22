using TMPro;
using UnityEngine;

public class UITimerText : BasicBehaviour
{
    private TextMeshProUGUI Timertext;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        Timertext = GetComponent<TextMeshProUGUI>();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        Timertext.text = "Time " + FormatTime(GameResultSingleton.Instance?.ResultTime);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    public static string FormatTime(float? timeSeconds)
    {
        if (timeSeconds < 0f) timeSeconds = 0f;

        int minutes = (int)(timeSeconds / 60f);
        float? seconds = timeSeconds % 60f;

        return $"{minutes}:{seconds:00.00}";
    }
}