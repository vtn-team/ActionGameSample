using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単なキャラクター管理
/// </summary>
public class Character : MonoBehaviour
{
    [SerializeField] int _charId = 1; //変えないこと
    [SerializeField] int _hp = 100;
    [SerializeField] int _criRate = 80;
    Rigidbody _rbody;
    //float _invincibleTimer = 0;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="dmg"></param>
    public void Damage(int dmg)
    {
        DamagePopup.Pop(gameObject, dmg, Color.red);
        _hp -= dmg;
        if(_hp <= 0)
        {
            GameController.Instance.GameOver(_charId);
        }
    }

    /// <summary>
    /// ノックバックする
    /// </summary>
    /// <param name="pow">ノックバックする威力</param>
    public void HitBack(float pow)
    {
        if (!Setting.HasKnockback) return;

        //都合に応じて関数を変えること
        _rbody.AddForce(-transform.forward * pow, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
