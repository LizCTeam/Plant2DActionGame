using UnityEngine;

public class CarrotAbility : BasicBehaviour, IHasAbility
{
    public GameObject Position;
    public GameObject Projectile;
    public float CarrotSpeed = 8f;
    
    public void UseAbility()
    {
        Debug.Log("Used Ability");
        GameObject Obj = Instantiate(Projectile, Position.transform.position, Quaternion.identity);
        CarrotMissile carrot = Obj.GetComponent<CarrotMissile>();
        carrot.speed = CarrotSpeed;
    }
}