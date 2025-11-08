using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : BasicBehaviour
{
    [SerializeField]
    private Camera targetCamera; //対象とするカメラ
 
    [SerializeField]
    private Vector2 aspectVec;
    
    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
        var targetAspect = aspectVec.x / aspectVec.y; //目的のアスペクト比
 
        var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率
 
        var viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成
 
        if (magRate < 1)
        {
            viewportRect.width = magRate; //使用する横幅を変更
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
        }
        else
        {
            viewportRect.height = 1 / magRate; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央余生
        }
 
        targetCamera.rect = viewportRect;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}