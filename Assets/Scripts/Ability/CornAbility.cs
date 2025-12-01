using UnityEngine;

public class CornAbility : BasicBehaviour, IHasAbility
{
    public GameObject Parent;
    public GameObject Position;
    public GameObject CornObject;
    
    public void UseAbility()
    {
        Debug.Log("Used Ability 2");
        var corn = Instantiate(CornObject, Position.transform.position, Quaternion.identity);
        var scale = corn.transform.localScale;
        scale.x = Mathf.Sign(Parent.transform.localScale.x) * Mathf.Abs(Parent.transform.localScale.x);
        corn.transform.localScale = scale;
        corn.transform.parent = Parent.transform;
    }
}