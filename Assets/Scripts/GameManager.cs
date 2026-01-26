using UnityEngine;

public class GameManager : BasicBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool IsBossDead = false;
    public GameObject Goal;
    
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
    
    protected override void OnStart()
    {
        base.OnStart();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (IsBossDead)
        {
            Goal.gameObject.SetActive(true);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}