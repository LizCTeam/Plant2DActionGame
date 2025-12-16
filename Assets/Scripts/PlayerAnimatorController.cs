using UnityEngine;

public class PlayerAnimatorController : BasicBehaviour
{
    private static readonly int _walk = Animator.StringToHash("Walk");
    private static readonly int _jump = Animator.StringToHash("Jump");
    
    public PlayerController _playerController;
    [SerializeField] private Player _player;

    private Animator _animator;
    
    private float horizontal;
    private float vertical;

    private bool walk = false;
    private bool jump = false;

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
        _animator.SetBool(_walk, walk);
        
        if (!_player.isGrounded())
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        _animator.SetBool(_jump, jump);
        
    }
}
