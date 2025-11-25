using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    public float speedMultiplier = 1.5f;
    
    protected override void OnStart()
    {
        base.OnStart();
    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public int GetHp()
    {
        return this._hp;
    }
}
