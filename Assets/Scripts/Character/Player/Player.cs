using UnityEngine;

public class Player : Character
{
    //speedはインスペクターからいじれます
    public float jumpSpeed = 8f;
    public float speedMultiplier = 1f;
    
    public bool isButtonPress;
    public GameObject VisualRoot;
    
    //ここにStart()関数を入れますがEventHandlerをつけていないので出来ません。書かないでね。
    protected override void OnStart()
    {
        base.OnStart();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        var visualScale = VisualRoot.transform.localScale;
        
        _body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * _speed * speedMultiplier, _body.linearVelocity.y);
        
        if (_body.linearVelocity.x < 0f)
        {
            visualScale.x = -Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
        else if (_body.linearVelocity.x > 0f)
        {
            visualScale.x = Mathf.Abs(visualScale.x);
            VisualRoot.transform.localScale = visualScale;
        }
        
        if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            _body.linearVelocityY += jumpSpeed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isButtonPress = true;
        }
        else
        {
            isButtonPress = false;
        }
    }
}
