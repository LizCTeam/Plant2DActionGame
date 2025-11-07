using UnityEngine;

public class CarrotAbility : BasicBehaviour, IHasAbility
{
    public GameObject Position;
    public GameObject Projectile;
    
    public void UseAbility()
    {
        Debug.Log("Used Ability");
        Instantiate(Projectile, Position.transform.position, Quaternion.identity);
    }
}