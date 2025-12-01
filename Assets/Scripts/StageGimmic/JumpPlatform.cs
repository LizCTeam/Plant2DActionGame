using UnityEngine;
using UnityEngine.Serialization;

public class JumpPlatform : BasicBehaviour
{
    [SerializeField]
    private float _jumpForce = 0f;
    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
}