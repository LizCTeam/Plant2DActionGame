using UnityEngine;
using UnityEngine.InputSystem;

public class Character : BasicBehaviour
{
    [SerializeField]
    protected float speed = 4f;
    
    public LayerMask GroundLayer;
    protected Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OnStart()
    {
        base.OnStart();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    protected bool isGrounded()
    {
        //BoxColliderにあるCastを使って地面の判定をboolで返します。
        BoxCollider2D c = GetComponent<BoxCollider2D>();
        return Physics2D.BoxCast(c.bounds.center, c.bounds.size, 0f, Vector2.down, .1f, GroundLayer);
    }
}
