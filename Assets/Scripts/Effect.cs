using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡易的なエフェクト管理
/// NOTE: 消える際の管理
/// </summary>
public class Effect : MonoBehaviour
{
    [SerializeField] float _life = 2.0f;

    float _time = 0.0f;

    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _life)
        {
            Destroy(gameObject);
        }
    }
}
