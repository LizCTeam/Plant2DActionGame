using UnityEngine;

public class DestoryOnAnimationEndComporment : BasicBehaviour
{
    public void OnAnimationEnd()
    {
        Destroy(this.gameObject);
    }
}