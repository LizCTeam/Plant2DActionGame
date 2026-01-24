using UnityEngine;

public class GameManager : BasicBehaviour
{
    protected override void OnStart()
    {
        base.OnStart();
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}