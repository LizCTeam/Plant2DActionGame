using UnityEngine;

public class PlayerAnimatorController : BasicBehaviour
{
    [SerializeField] private Animator _animator;
    public PlayerController _playerController;

    private float horizontal;
    private float vertical;

    private bool walk = false;
    

    protected override void OnAwake()
    {
        base.OnAwake();
        _animator = GetComponent<Animator>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Mathf.Abs(_playerController.inputDirection.x) >= 0.1f)
        {
            walk = true;
        }
        else
        {
            walk = false;
        }
        _animator.SetBool("Walk", walk);
    }
}
