using System.Collections;
using UnityEngine;

public class CameraFollow : BasicBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField]private Rigidbody2D _playerRb2D;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _thresholdY = 10f;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private float _addY = 2f;

    private bool _isFollowing = true;

    private bool _JumpFlag;

    private Vector3 _initialCameraPos;
    private Vector3 _playerPos;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Character _playerCharacter;
    [SerializeField] private Player _playermove;

    protected override void OnStart()
    {
        base.OnStart();

        _initialCameraPos = transform.position;

        _JumpFlag = _playermove.IsNowJump;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position;
        float lerpTargetY = 0;

        var playerAct = _playerController.playerAct;

        // x軸は常に追従
        targetPos.x = _player.position.x + 6f;

        //if(player.position.y > _thresholdY)
        //{
        //    lerpTargetY = player.position.y + _addY;
        //}
        //else if(player.position.y > _minY && _thresholdY > player.position.y)
        //{
        //    targetPos.y = _initialCameraPos.y;
        //}

        bool _isGrounded = _playerCharacter.isGrounded();

        if (_isGrounded)
        {
            _JumpFlag = true;
            _playerPos.y = _player.position.y;

        }
        else
        {
            if (_player.position.y < _playerPos.y)
            {
                _JumpFlag = true;
            }
        }

        if (!_playermove.IsInJumpArea)
        {
            if (playerAct.Jump.WasPressedThisFrame()) // ジャンプボタンが押された瞬間
            {
                _JumpFlag = false;
                StartCoroutine(FollowCameraOff());// <- ジャンプボタンが押された瞬間だからギリギリ動作するぜ
            }
        }
        else if (_playermove.IsInJumpArea)
        {
            if (playerAct.Jump.WasPressedThisFrame()) // ジャンプボタンが押された瞬間
            {
                _JumpFlag = true;
                _JumpFlag = true;
            }
        }

        
       

        //if(playerAct.Jump.WasReleasedThisFrame())
        //{
        //    if (_playermove.IsInJumpArea|| _playerRb2D.linearVelocity.y > 0)
        //    {
        //        _JumpFlag = true;
        //    }
        //    else
        //    {
        //        _JumpFlag = false;
        //    }
        //    _playerPos.y = _player.position.y;
        //    StartCoroutine(FollowCameraOff());// <- ジャンプボタンが押された瞬間だからギリギリ動作するぜ
        //}

        if (_JumpFlag)
        {
            lerpTargetY = _player.position.y + _addY;

            targetPos.y = Mathf.Lerp(transform.position.y, lerpTargetY, Time.deltaTime * _smoothSpeed);
        }



        transform.position = targetPos;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private IEnumerator FollowCameraOff()
    {
        yield return new WaitForSeconds(0.1f);
        if (_player.position.y > _playerPos.y)
        {
            _JumpFlag = true;
        }
        _JumpFlag = false;
    }



    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 500), $"{_JumpFlag}");
        GUI.Label(new Rect(10, 50, 1000, 500), $"{_playermove.IsInJumpArea}");
    }
}