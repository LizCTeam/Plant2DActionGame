using UnityEngine;

public class CanvasResult : BasicBehaviour
{
    private static readonly int IsGameClear = Animator.StringToHash("isGameClear");
    private static readonly int IsGameOver = Animator.StringToHash("isGameOver");
    private static readonly int IsFade = Animator.StringToHash("isFade");

    [HideInInspector]
    public Animator animator;

    protected override void OnAwake()
    {
        base.OnAwake();
        animator = GetComponent<Animator>();
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
        animator.SetBool(IsGameClear, true);
    }

    public void GameOver()
    {
        animator.SetBool(IsGameOver, true);
    }

    public void FadeIn()
    {
        animator.SetBool(IsFade, true);
    }
    
    public void FadeOut()
    {
        animator.SetBool(IsFade, false);
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