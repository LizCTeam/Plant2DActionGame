using UnityEngine;

public class Mouse : Enemy
{
    public bool startFacingRight = false;
    
    [SerializeField]
    private SpriteRenderer _renderer;
    
    private Vector2 _raycastOffset = new Vector2(0.0f, 0.0f);
    private int _direction = -1; // -1: 左, +1: 右
    private float _wallCheckDistance = 1.0f;
    
    void Awake()
    {
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }
        _direction = startFacingRight ? 1 : -1;
    }
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (IsWallAhead())
        {
            FlipDirection();
        }

        // 横速度を設定（縦方向は物理任せ）
        _body.linearVelocity = new Vector2(_direction * _speed, _body.linearVelocity.y);
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
    
    private bool IsWallAhead()
    {
        Vector2 origin = (Vector2)transform.position + _raycastOffset;
        Vector2 dir = new Vector2(_direction, 0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, _wallCheckDistance, GroundLayer);

        return hit.collider != null;
    }
    
    private void FlipDirection()
    {
        _direction *= -1;
        _renderer.flipX = !_renderer.flipX;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & GroundLayer) != 0)
        {
            foreach (var contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    FlipDirection();
                    break;
                }
            }
        }
    }
}