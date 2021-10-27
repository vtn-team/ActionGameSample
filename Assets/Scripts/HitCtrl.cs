using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCtrl : MonoBehaviour
{
    Collider _collider;
    ActionCtrl _actCtrl;
    int _actionId;

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();
    }
    
    void Update()
    {
    }

    public void SetCtrl(ActionCtrl actCtrl, int actId)
    {
        _actCtrl = actCtrl;
        _actionId = actId;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        EffectManager.PlayEffect("Hit", this.transform);

        //ここ、本格的に実装する際はキャッシュするなど、工夫は必要
        Character chara = other.gameObject.GetComponent<Character>();
        _actCtrl.HitCallback(chara, _actionId);
    }
}
