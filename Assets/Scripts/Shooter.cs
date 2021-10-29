using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具発射クラス
/// </summary>
public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject _shooter = null;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(_shooter, transform.position + transform.forward * 2, transform.rotation);
        }
    }
}
