using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具発射クラス
/// </summary>
public class Shooter : MonoBehaviour
{
    [SerializeField] float _power = 3.0f;
    Character _character;


    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    protected void Shot()
    {
        var shotObj = Instantiate(_character.Bullets[_character.Index].BulletPrefab, transform.position + transform.forward * 2, transform.rotation);
        var hitCtrl = shotObj.GetComponent<HitCtrl>();
        var wave = shotObj.GetComponent <Wave>();
        hitCtrl.SetParameter(_character.Bullets[_character.Index].Damage, _power);
        wave.Set(_character.Bullets[_character.Index].Range);
    }
}
