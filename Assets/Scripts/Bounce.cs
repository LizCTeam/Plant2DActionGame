using UnityEngine;
using UnityEngine.Serialization;

public class Bounce : BasicBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField]
    private GameObject playerObject;
    [SerializeField] private float _bounceForce = 0f;

    private float _playerBounceSpeed;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("OnTriggerEnter");
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Enter");
            Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
            body.AddForce(transform.up * _bounceForce, ForceMode2D.Impulse);
        }
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
