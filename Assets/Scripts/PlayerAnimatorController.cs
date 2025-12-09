using UnityEngine;

public class PlayerAnimatorController : BasicBehaviour
{
    private static readonly int _walk = Animator.StringToHash("Walk");
    private static readonly int _jump = Animator.StringToHash("Jump");

    [SerializeField] private Animator _animator;
    public PlayerController _playerController;
    [SerializeField] private Player _player;

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
        //横移動のアニメーション
        if (Mathf.Abs(_playerController.inputDirection.x) >= 0.1f)
        {
            walk = true;
        }
        else
        {
            walk = false;
        }
        _animator.SetBool(_walk, walk);

        //ジャンプのアニメーション
        if (_player.isGrounded()==false)
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
