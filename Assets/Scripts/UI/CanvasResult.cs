using UnityEngine;

public class CanvasResult : BasicBehaviour
{
    private static readonly int IsGameClear = Animator.StringToHash("isGameClear");
    private static readonly int IsGameOver = Animator.StringToHash("isGameOver");
    private Animator _animator;

    protected override void OnAwake()
    {
        base.OnAwake();
        _animator = GetComponent<Animator>();
    }
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void GameClear()
    {
        _animator.SetBool(IsGameClear, true);
    }

    public void GameOver()
    {
        _animator.SetBool(IsGameOver, true);
    }
    
    public void GameEnd()
    {
        if(GameResultSingleton.Instance.IsGameClear)
        {
            GameClear();
        }
        else
        {
            GameOver();
        }
    }
}