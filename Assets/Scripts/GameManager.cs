using UnityEngine;

public class GameManager : BasicBehaviour
{
    [SerializeField]
    private Camera cam;
    
    protected override void OnStart()
    {
        base.OnStart();
        Application.targetFrameRate = 60;
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