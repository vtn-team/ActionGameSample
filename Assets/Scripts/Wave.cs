using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具クラス
/// </summary>
public class Wave : MonoBehaviour
{
    [SerializeField] float _speed = 2.0f;
    float _life;

    float _time = 0.0f;

    public void Set(float life)
    {
        _life = life;
    }

    void Update()
    {
        _time += Time.deltaTime;
        transform.position += transform.forward * _speed * Time.timeScale;
        
        if(_time >= _life)
        {
            Destroy(gameObject);
        }
    }
}
