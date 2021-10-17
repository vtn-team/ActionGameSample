using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] float _speed = 2.0f;
    [SerializeField] float _life = 5.0f;

    float _time = 0.0f;

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
