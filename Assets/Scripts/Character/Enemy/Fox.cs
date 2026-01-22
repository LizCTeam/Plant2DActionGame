using IceMilkTea.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Fox : Enemy, IDamageable
{
    private static readonly int IsDetected = Animator.StringToHash("isDetected");

    [SerializeField]
    private Player _player;
    
    [SerializeField]
    private Animator _foxAnimator;
    
    [SerializeField]
    private GameObject VisualRoot;
    
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _deceleration;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField] 
    private float _velPower = 0.9f;
    [SerializeField]
    private float _jumpPower = 10f;
    
    [SerializeField]
    private float _detectionRadius = 25f;
    
    private float _currentSpeed;
    
    private ImtStateMachine<Fox> _stateMachine;
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        var playerDistance = (_player.transform.position - transform.position).magnitude;
        if (playerDistance < _detectionRadius)
        {
            _foxAnimator.SetBool(IsDetected, true);
            Move();
            TryJump();
        }
        else
        {
            _foxAnimator.SetBool(IsDetected, false);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    public void Move()
    {
        var targetSpeed = .0f;
        var direction = Vector2.zero;
        
        if (_player != null)
        {
            direction = (_player.transform.position - transform.position).normalized;
            targetSpeed = direction.x * _maxSpeed;
        }
        
        var speedDif = targetSpeed - _body.linearVelocity.x;
        var accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _acceleration : _deceleration;
        var movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, _velPower) * Mathf.Sign(speedDif);
        _body.AddForce(movement * Vector2.right, ForceMode2D.Force);

        SpriteFlip();
    }
    
    private void SpriteFlip()
    {
        if (_body.linearVelocityX < -0.1f)
        {
            VisualRoot.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (_body.linearVelocityX > 0.1f)
        {
            VisualRoot.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    public void TryJump()
    {
        if (isGrounded() && CanJump())
        {
            _body.linearVelocity = new Vector2(_body.linearVelocity.x, 0f);
            _body.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    public bool CanJump()
    {
        var horizontalVelocity = new Vector2(_body.linearVelocity.x, 0f);
        RaycastHit2D hit =  Physics2D.Raycast(transform.position, horizontalVelocity, 4f, GroundLayer);
        //distanceはよくわかんないけどデフォで1f

        return hit;
    }
    
    public void OnDamaged(int damage)
    {
        EffectManager.Instance.PlayEffect(transform.position);
        _hp -= damage;
    }
}