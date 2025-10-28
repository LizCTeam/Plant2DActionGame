using UnityEngine;

public class Player : Character
{
    //speedはインスペクターからいじれます
    
    [SerializeField]
    private float jumpSpeed = 8f;
    
    //ここにStart()関数を入れますがEventHandlerをつけていないので出来ません。書かないでね。
    protected override void OnStart()
    {
        base.OnStart();
    }
    
    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y);
        
        Debug.Log(isGrounded());
        
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocityY += jumpSpeed;
        }
    }
}
