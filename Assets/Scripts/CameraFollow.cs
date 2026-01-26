using UnityEngine;

public class CameraFollow : BasicBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float _offsetX = 0f;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _thresholdY = 10f;
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private float _addY = 2f;

    private Vector3 _initialCameraPos;

    protected override void OnStart()
    {
        base.OnStart();

        _initialCameraPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position;
        float lerpTargetY = 0;

        // x���͏�ɒǏ]
        targetPos.x = player.position.x + _offsetX;

        //if(player.position.y > _thresholdY)
        //{
        //    lerpTargetY = player.position.y + _addY;
        //}
        //else if(player.position.y > _minY && _thresholdY > player.position.y)
        //{
        //    targetPos.y = _initialCameraPos.y;
        //}

        lerpTargetY = player.position.y + _addY;

        targetPos.y = Mathf.Lerp(transform.position.y,lerpTargetY, Time.deltaTime * _smoothSpeed);

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
}