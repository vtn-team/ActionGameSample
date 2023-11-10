using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具発射クラス(敵)
/// </summary>
public class EnemyShooter : MonoBehaviour
{
    [SerializeField] float _interval = 2.0f;
    [SerializeField] GameObject _shooter = null;

    float _timer = 0.0f;

    void Update()
    {
        if (_interval <= 0.0f) return;

        _timer += Time.deltaTime;
        if (_timer > _interval)
        {
            _timer -= _interval;
            Instantiate(_shooter, transform.position + transform.forward * 2, transform.rotation);
        }
    }
}
