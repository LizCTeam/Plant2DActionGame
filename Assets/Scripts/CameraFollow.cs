using UnityEngine;

public class CameraFollow : BasicBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float _minY = 0f;
    [SerializeField] private float _thresholdY = 10f;
    [SerializeField] private float _smoothSpeed = 5f;

    private Vector3 _initialCameraPos;

    protected override void OnStart()
    {
        base.OnStart();

        _initialCameraPos = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position;

        // xŽ²‚Íí‚É’Ç]
        targetPos.x = player.position.x + 6f;

        if(player.position.y > _thresholdY)
        {
            targetPos.y = Mathf.Lerp(transform.position.y,player.position.y, Time.deltaTime * _smoothSpeed);
        }
        else if(player.position.y > _minY && _thresholdY > player.position.y)
        {
            targetPos.y = Mathf.Lerp(transform.position.y,_initialCameraPos.y,Time.deltaTime * _smoothSpeed);
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
}