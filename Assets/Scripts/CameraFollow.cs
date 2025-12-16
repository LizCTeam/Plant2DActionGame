using System.Collections;
using UnityEngine;

public class CameraFollow : BasicBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _thresholdY = 10f;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private float _addY = 2f;

    private bool _isFollowing = true;

    private bool hoge1;
    private bool hoge2;

    private Vector3 _initialCameraPos;
    private Vector3 _playerPos;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Character _playerCharacter;
    [SerializeField] private Player _playermove;

    protected override void OnStart()
    {
        base.OnStart();

        _initialCameraPos = transform.position;

        hoge1 = _playermove.IsNowJump;
        hoge2 = _playermove.IsInJumpArea;

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



        if (_playerCharacter.isGrounded() && !hoge1)
        {
            hoge1 = true;
            
        }

        if (!_playermove.IsInJumpArea)
        { 
            if (playerAct.Jump.WasPressedThisFrame()) // ジャンプボタンが押された瞬間
            {
                hoge1 = false;
                hoge1 = true;
                _playerPos.y = _player.position.y;
                StartCoroutine(FollowCameraOff());// <- ジャンプボタンが押された瞬間だからギリギリ動作するぜ
                
            }
        }

        

        if (hoge1)
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
        if(_player.position.y > _playerPos.y)
        {
            hoge1 = true;
        }
        hoge1 = false;
    }

    

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 500), $"{_isFollowing}");
        GUI.Label(new Rect(10, 50, 1000, 500), $"{_playermove.IsInJumpArea}");
    }
}