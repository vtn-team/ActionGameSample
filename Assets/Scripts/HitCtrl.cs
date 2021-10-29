using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃ヒット時のコントローラ
/// </summary>
public class HitCtrl : MonoBehaviour
{
    Collider _collider;
    ActionCtrl _actCtrl;
    int _actionId;

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();
    }
    
    /// <summary>
    /// 管理クラス設定
    /// </summary>
    /// <param name="actCtrl"></param>
    /// <param name="actId"></param>
    public void SetCtrl(ActionCtrl actCtrl, int actId)
    {
        _actCtrl = actCtrl;
        _actionId = actId;
    }

    /// <summary>
    /// ヒット時のコールバック
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        EffectManager.PlayEffect("Hit", this.transform);

        //ここ、本格的に実装する際はキャッシュするなど、工夫は必要
        Character chara = other.gameObject.GetComponent<Character>();
        _actCtrl.HitCallback(chara, _actionId);
    }
}
