using UnityEngine;
using UnityEngine.Serialization;

public class JumpPlatform : BasicBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField] 
    private GameObject playerObject;
    [SerializeField]
    private float _jumpForce = 0f;

    private float _playerJumpSpeed;
    
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
            print("Enter");
            Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
}