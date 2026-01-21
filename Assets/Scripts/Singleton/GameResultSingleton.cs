using UnityEngine;

public class GameResultSingleton : BasicBehaviour
{
    public static GameResultSingleton Instance { get; private set; }
    
    public int KillCount { get; private set; }
    public int MilkCount { get; private set; }
    public int SecretCount { get; private set; }
    public float ResultTime { get; private set; }
    
    private bool _isTimerRunning; 
    
    protected override void OnAwake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (_isTimerRunning)
        {
            ResultTime += Time.deltaTime;
        }
    }

    public void InitResult()
    {
        KillCount = 0;
        MilkCount = 0;
        SecretCount = 0;
        ResultTime = 0;
    }

    public void StartTimer()
    {
        _isTimerRunning = true;
        ResultTime = 0f;
    }

    public void StopTimer()
    {
        _isTimerRunning = false;
    }
    
    public void IncrementKillCount() => KillCount++;
    public void IncrementMilkCount() => MilkCount++;
    public void IncrementSecretCount() => SecretCount++;
}