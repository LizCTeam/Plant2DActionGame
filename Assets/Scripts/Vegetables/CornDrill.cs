using DG.Tweening;
using UnityEngine;

public class CornDrill : BasicBehaviour
{
    [SerializeField]
    private float _useTimer;
    [SerializeField]
    private float _speedMultiplier=10f;
    [SerializeField]
    private float _scaleMultiplier = 2f;

    private Player _player;
    private PlayerController _playerController;

    protected override void OnStart()
    {
        base.OnStart();
        GameObject _playerObject = GameObject.FindWithTag("Player");
        _player = _playerObject?.GetComponent<Player>();
        _playerController = _playerObject?.GetComponent<PlayerController>();
        //_player.speedMultiplier = this._speedMultiplier;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        var playerFireAct = _playerController.playerAct.Fire;
        if (playerFireAct != null)
        {
            Shoot();
        }
        //_useTimer -= Time.deltaTime;
        //if (_useTimer <= 0f)
        //{
        //    _useTimer = 0f;
        //    Destroy(this.gameObject);
        //}
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
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

        _useTimer -= Time.deltaTime;

        //X軸方向に移動するアニメーションを追加
        Tween moveTween = transform.DOMoveX(transform.position.x + _speedMultiplier, _useTimer).SetEase(Ease.Linear);

        //スケールを変化させるアニメーションを追加
        Tween scaleTween=transform.DOScale(_scaleMultiplier,_useTimer).SetEase(Ease.Linear);

        //シーケンスに2つのTweenを追加
        sequence.Join(moveTween);
        sequence.Join(scaleTween);

        //シーケンス完了時にコールバックを設定
        sequence.OnComplete(OnDestroy);
    }

    private void OnDestroy()
    {
        //_player.speedMultiplier = 1.0f;
        //ここにオブジェクト破壊の処理を追加
        Destroy(this.gameObject);
    }
}