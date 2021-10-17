using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    Rigidbody _rbody;
    //float _invincibleTimer = 0;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    public void Damage(int dmg)
    {
        DamagePopup.Pop(gameObject, dmg, Color.red);
    }

    public void HitBack(float pow)
    {
        if (!Setting.HasKnockback) return;

        //都合に応じて関数を変えること
        _rbody.AddForce(-transform.forward * pow, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
