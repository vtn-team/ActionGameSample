using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具発射クラス(敵)
/// </summary>
public class EnemyShooter : Shooter
{
    [SerializeField] float _interval = 2.0f;
    float _timer = 0.0f;

    void Update()
    {
        if (_interval <= 0.0f) return;

        _timer += Time.deltaTime;
        if (_timer > _interval)
        {
            _timer -= _interval;
            Shot();
        }
    }
}
