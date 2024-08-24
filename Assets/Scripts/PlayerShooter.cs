using System;
using UnityEngine;

public class PlayerShooter : Shooter
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shot();
        }
    }
}
