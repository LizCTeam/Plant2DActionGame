using UnityEngine;

public class AnimatorController : BasicBehaviour
{
    [SerializeField] private Animator _animator;
    private PlayerController _playerController;

    private float horizontal;
    private float vertical;

    private bool walk = false;
    

    protected override void OnAwake()
    {
        base.OnAwake();
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Mathf.Abs(_playerController.inputDirection.x) >= 1.00f)
        {
            walk = true;
        }
        else if(Mathf.Abs(_playerController.inputDirection.x) >= -1.00f)
        {
            walk = true;
        }
        else if (Mathf.Abs(_playerController.inputDirection.x) == 0)
        {
            walk = false;
        }
        _animator.SetBool("Walk", walk);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
