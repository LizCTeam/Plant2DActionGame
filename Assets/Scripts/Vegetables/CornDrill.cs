using System;
using UnityEngine;

public class CornDrill : BasicBehaviour
{
    [SerializeField]
    private float _useTimer;
    [SerializeField]
    private float _speedMultiplier;
    
    private Player _player;
    
    protected override void OnStart()
    {
        base.OnStart();
        GameObject _playerObject = GameObject.FindWithTag("Player");
        _player = _playerObject?.GetComponent<Player>();
        _player.speedMultiplier = this._speedMultiplier;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        _useTimer -= Time.deltaTime;
        if (_useTimer <= 0f)
        {
            _useTimer = 0f;
            Destroy(this.gameObject);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }

    private void OnDestroy()
    {
        _player.speedMultiplier = 1.0f;
    }
}