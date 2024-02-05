using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 簡単な飛び道具発射クラス
/// </summary>
public class Shooter : MonoBehaviour
{
    [SerializeField] int _damage = 100;
    [SerializeField] float _power = 3.0f;
    [SerializeField] GameObject _shooter = null;
    
    protected void Shot()
    {
        var shotObj = Instantiate(_shooter, transform.position + transform.forward * 2, transform.rotation);
        var hitCtrl = shotObj.GetComponent<HitCtrl>();
        hitCtrl.SetParameter(_damage, _power);
    }
}
