using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    //speedはインスペクターからいじれます
    public float jumpSpeed = 8f;
    public float speedMultiplier = 1f;
    private Vector2 moveInput = Vector2.zero;
    
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

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput =  context.ReadValue<Vector2>();
        _body.linearVelocity = new Vector2(moveInput.x * _speed * speedMultiplier, _body.linearVelocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            _body.linearVelocityY += jumpSpeed;
        }
    }

    private void OnUniqueAction(InputAction.CallbackContext context)
    {
        isButtonPress = isButtonPress != isButtonPress;
    }
}
