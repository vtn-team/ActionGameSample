using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単なキャラクター管理
/// </summary>
public class Character : MonoBehaviour
{
    public delegate void LifeChange(int diff);

    [SerializeField] int _charId = 1; //変えないこと
    [SerializeField] int _hp = 100;
    [SerializeField] int _criRate = 80;
    Rigidbody _rbody;

    public int HP => _hp;
    public int MaxHP { get; protected set; }
    LifeChange _lifeChange;

    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        MaxHP = _hp;
    }

    public void SetLifeChangeDelegate(LifeChange dlg)
    {
        _lifeChange += dlg;
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="dmg"></param>
    public void Damage(int dmg)
    {
        DamagePopup.Pop(gameObject, dmg, Color.red);
        _hp -= dmg;
        _lifeChange?.Invoke(dmg);

        if (_hp <= 0)
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
