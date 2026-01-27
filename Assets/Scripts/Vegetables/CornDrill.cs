using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CornDrill : BasicBehaviour
{
    [SerializeField]
    private float _useTimer;
    [SerializeField]
    private float _distance = 10f;

    [SerializeField] 
    private float _delay = 0.1f;
    [HideInInspector]
    public float _scaleMultiplier = 1f;
    
    [HideInInspector]
    public Hitbox OwnerHitbox;

    [HideInInspector]
    public int Damage
    {
        set
        {
            if (OwnerHitbox != null)
            {
                OwnerHitbox.damage = value;
            }
        }
        get => OwnerHitbox?.damage ?? 0;
    }
    
    private Player _player;
    private PlayerController _playerController;
    private Collider2D _collider2D;

    protected override void OnAwake()
    {
        base.OnAwake();
        OwnerHitbox = GetComponentInChildren<Hitbox>();
        _collider2D = GetComponentInChildren<Collider2D>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        _collider2D.enabled = false;
        SoundManagerSingleton.Instance.PlaySound("Shoot");
        GameObject _playerObject = GameObject.FindWithTag("Player");
        _player = _playerObject?.GetComponent<Player>();
        _playerController = _playerObject?.GetComponent<PlayerController>();
        transform.localScale = new Vector3(_scaleMultiplier, _scaleMultiplier, _scaleMultiplier);
        var playerFireAct = _playerController.playerAct.Fire;
        if (playerFireAct != null)
        {
            StartCoroutine(DelayEnableCollision(_delay));
            Shoot();
        }
        //_player.speedMultiplier = this._speedMultiplier;
    }

    private IEnumerator DelayEnableCollision(float delay)
    {
        yield return new WaitForSeconds(delay); 
        _collider2D.enabled = true;
    }

    private void Shoot()
    {
        // Implement shooting logic here
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //if (rb != null)
        //{
        //    rb.linearVelocity = Vector2.left * -_player.transform.GetChild(0).localScale.x * _speedMultiplier;
        //}

        //Sequenceのインスタンスを作成
        var sequence = DOTween.Sequence();
        var dir = Math.Sign(_player.VisualRoot.transform.localScale.x);

        _useTimer -= Time.deltaTime;

        //X軸方向に移動するアニメーションを追加
        Tween moveTween = transform.DOMoveX(transform.position.x + _distance * dir, _useTimer)
            .SetEase(Ease.OutQuad);

        var scale = transform.localScale;
        scale.x *= dir;
        transform.localScale = scale;
        
        ////スケールを変化させるアニメーションを追加
        //Tween scaleTween=transform.DOScale(_scaleMultiplier,_useTimer).SetEase(Ease.Linear);

        //シーケンスに2つのTweenを追加
        sequence.Join(moveTween);
        //sequence.Join(scaleTween);

        //シーケンス完了時にコールバックを設定
        sequence.OnComplete(OnDestroy);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            OnDestroy();
        }
    }

    private void OnDestroy()
    {
        //_player.speedMultiplier = 1.0f;
        //ここにオブジェクト破壊の処理を追加
        Destroy(this.gameObject);
    }
}