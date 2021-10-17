using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    [SerializeField] bool _hasCameraShake = true;
    [SerializeField] bool _hasParticleEffect = true;
    [SerializeField] bool _hasHitStop = true;
    [SerializeField] bool _hasKnockback = true;
    [SerializeField] bool _hasDamageUI = true;

    static public bool HasCameraShake => _instance._hasCameraShake;
    static public bool HasParticleEffect => _instance._hasParticleEffect;
    static public bool HasHitStop => _instance._hasHitStop;
    static public bool HasKnockback => _instance._hasKnockback;
    static public bool HasDamageUI => _instance._hasDamageUI;
    

    static Setting _instance;
    private void Awake()
    {
        _instance = this;
    }
}
