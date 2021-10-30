using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単なキャラクター管理
/// </summary>
public class Character : MonoBehaviour
{
    public enum StateType
    {
        Grounded,
        InAir,
    }
    public enum ConditionType
    {
        Idle,
        Stun,
    }

    StateType _state;
    ConditionType _cond;
    Rigidbody _rbody;

    public StateType State => _state;
    public ConditionType Condition => _cond;

    private void Awake()
    {
        _state = StateType.Grounded;
        _cond = ConditionType.Idle;

        _rbody = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// ダメージを受けた
    /// </summary>
    /// <param name="dmg"></param>
    public void Damage(int dmg)
    {
        DamagePopup.Pop(gameObject, dmg, Color.red);
    }

    private void Update()
    {
        StateCheck();
    }

    /// <summary>
    /// 打ち上げ
    /// </summary>
    public void ShootUp()
    {
        //後で適当に力はもらったりする
        _rbody.AddForce(new Vector3(0, 1, 0) * 10, ForceMode.Impulse);
        SetState(StateType.InAir);
    }

    /// <summary>
    /// ステート移動
    /// </summary>
    /// <param name="dmg"></param>
    void SetState(StateType s)
    {
        _state = s;
    }

    void StateCheck()
    {
        if(Mathf.Abs(_rbody.velocity.y) < 0.001f)
        {
            SetState(StateType.Grounded);
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
        Debug.Log(collision.gameObject.name);
    }
}
