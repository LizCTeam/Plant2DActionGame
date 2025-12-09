using UnityEngine;

public class FollowCamera : BasicBehaviour
{
    public Transform player;
    Vector3 prePlayerPos;//前フレームでのプレイヤーの座標位置
    

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if( player.transform.position!= prePlayerPos)
        {

            transform.position = new Vector3(player.transform.position.x,player.transform.position.y+2);
            prePlayerPos = player.transform.position;

            
            
            
        }
        

    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}